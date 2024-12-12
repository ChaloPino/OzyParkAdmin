using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Zonas;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.EscenariosCupo;

/// <summary>
/// El repositorio de <see cref="EscenarioCupo"/>.
/// </summary>
public class EscenarioCupoRepository(OzyParkAdminContext context) : Repository<EscenarioCupo>(context), IEscenarioCupoRepository
{

    /// <summary>
    /// Método que retorna una consulta base de `EscenarioCupo`, incluyendo detalles y exclusiones por fecha.
    /// </summary>
    /// <returns>Una consulta de `IQueryable` con los `Include` necesarios.</returns>
    private IQueryable<EscenarioCupo> GetBaseQuery()
    {
        return Context.Set<EscenarioCupo>()
            .Include(x => x.CentroCosto)
            .AsNoTracking();
    }

    /// <inheritdoc/>
    public async Task<EscenarioCupo?> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        // Usar el método GetBaseQuery para incluir detalles y exclusionesFecha asociadas al escenario de cupo
        return await GetBaseQuery()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }


    /// <inheritdoc/>
    public async Task<EscenarioCupo?> FindEscenarioAsync(int centroCostoId, int? zonaId, string nombre, CancellationToken cancellationToken)
    {
        // Incluir detalles y exclusionesFecha en la búsqueda por centro de costo, zona y nombre
        return await Context.Set<EscenarioCupo>()
            .FirstOrDefaultAsync(e => e.CentroCosto.Id == centroCostoId &&
                                      (zonaId == null || e.Zona!.Id == zonaId) &&
                                      e.Nombre == nombre, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EscenarioCupo>> FindEscenariosAsync(int centroCostoId, int? zonaId, CancellationToken cancellationToken)
    {
        // Buscar escenarios de cupo por centro de costo y zona, e incluir detalles y exclusionesFecha
        return await Context.Set<EscenarioCupo>()
            .Where(e => e.CentroCosto.Id == centroCostoId &&
                        (zonaId == null || e.Zona != null && e.Zona.Id == zonaId))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsWithSimilarNameAsync(string nombre, int? excludeId, CancellationToken cancellationToken)
    {
        // Verificar si existe un escenario de cupo con un nombre similar, excluyendo un ID específico si es necesario
        return await Context.Set<EscenarioCupo>()
            .AsNoTracking()
            .AnyAsync(e => e.Nombre == nombre && (!excludeId.HasValue || e.Id != excludeId.Value), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<PagedList<EscenarioCupoFullInfo>> SearchAsync(
        int[]? centrosCostoId,
        int[]? zonasId,
        string? searchText,
        FilterExpressionCollection<EscenarioCupo> filterExpressions,
        SortExpressionCollection<EscenarioCupo> sortExpressions,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        // Validar que las expresiones de filtro y de ordenamiento no sean nulas
        ArgumentNullException.ThrowIfNull(filterExpressions, nameof(filterExpressions));
        ArgumentNullException.ThrowIfNull(sortExpressions, nameof(sortExpressions));

        // Base query para buscar escenarios de cupo
        var query = Context.Set<EscenarioCupo>()
            .AsNoTracking();

        // Filtrar por centros de costo si se proporciona
        if (centrosCostoId is not null && centrosCostoId.Any())
        {
            query = query.Where(ec => centrosCostoId.Contains(ec.CentroCosto.Id));
        }

        // Filtrar por zonas si se proporciona
        if (zonasId is not null && zonasId.Any())
        {
            query = query.Where(ec => ec.Zona != null && zonasId.Contains(ec.Zona.Id));
        }

        // Filtrar por texto de búsqueda si se proporciona
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(ec =>
                ec.Nombre.Contains(searchText) ||
                (ec.Zona != null && ec.Zona.Descripcion.Contains(searchText)) ||
                ec.CentroCosto.Descripcion.Contains(searchText));
        }

        // Aplicar filtros adicionales mediante las expresiones de filtro proporcionadas
        query = filterExpressions.Where(query);

        // Contar el total de resultados antes de la paginación
        int totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        // Ordenar la consulta según las expresiones de ordenamiento proporcionadas
        query = sortExpressions.Sort(query);

        // Aplicar la paginación
        var paginatedQuery = query
            .Skip(page * pageSize).Take(pageSize);

        // Proyectar los resultados a `EscenarioCupoFullInfo`
        var items = await paginatedQuery.Select(ec => new EscenarioCupoFullInfo
        {
            Id = ec.Id,
            Nombre = ec.Nombre,
            CentroCosto = new CentroCostoInfo
            {
                Id = ec.CentroCosto.Id,
                Descripcion = ec.CentroCosto.Descripcion
            },
            Zona = ec.Zona != null ? new ZonaInfo
            {
                Id = ec.Zona.Id,
                Descripcion = ec.Zona.Descripcion
            } : null,
            EsHoraInicio = ec.EsHoraInicio,
            MinutosAntes = ec.MinutosAntes,
            EsActivo = ec.EsActivo
        })
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);

        foreach (EscenarioCupoFullInfo info in items)
        {
            info.TienCupoAsociado = await HasCupoRelated(info.Id, cancellationToken).ConfigureAwait(false);
        }

        // Construir el objeto paginado
        return new PagedList<EscenarioCupoFullInfo>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }

    /// <inheritdoc/>
    public async Task<List<EscenarioCupoInfo>> ListAsync(int[]? centroCostoIds, CancellationToken cancellationToken)
    {
        // Lista los escenarios de cupo, filtrando por centros de costo si se proporcionan
        return await Context.Set<EscenarioCupo>()
            .Include(x => x.CentroCosto)
            .Where(e => centroCostoIds == null || centroCostoIds.Contains(e.CentroCosto.Id))
            .Select(e => new EscenarioCupoInfo { Id = e.Id, Nombre = e.Nombre })
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> GetLastIdAsync(CancellationToken cancellationToken)
    {
        // Obtiene el último ID del escenario cupo
        return await Context.Set<EscenarioCupo>().AsNoTracking().MaxAsync(e => e.Id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(EscenarioCupo escenarioCupo, CancellationToken cancellationToken)
    {
        // Agrega un nuevo escenario cupo al contexto
        await Context.Set<EscenarioCupo>().AddAsync(escenarioCupo, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddRangeAsync(IEnumerable<EscenarioCupo> escenariosCupos, CancellationToken cancellationToken)
    {
        // Agrega varios escenarios cupos al contexto
        await Context.Set<EscenarioCupo>().AddRangeAsync(escenariosCupos, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddDetallesAsync(IEnumerable<DetalleEscenarioCupoInfo> detalles, CancellationToken cancellationToken)
    {
        // Convertir DetalleEscenarioCupoInfo a DetalleEscenarioCupo y agregar al contexto
        var detallesEntidades = detalles.Select(d =>
            DetalleEscenarioCupo.Create(
                d.EscenarioCupoId,
                d.ServicioId,
                d.TopeDiario,
                d.UsaSobreCupo,
                d.HoraMaximaVenta!.Value,
                d.HoraMaximaRevalidacion!.Value,
                d.UsaTopeEnCupo,
                d.TopeFlotante
            )).ToList();

        await Context.Set<DetalleEscenarioCupo>().AddRangeAsync(detallesEntidades, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(EscenarioCupo escenarioCupo, CancellationToken cancellationToken)
    {
        // Elimina un escenario cupo del contexto
        Context.Set<EscenarioCupo>().Remove(escenarioCupo);
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        // Guarda los cambios pendientes en la base de datos
        await Context.SaveChangesAsync(cancellationToken);
    }




    /// <inheritdoc/>
    public async Task<IEnumerable<EscenarioCupo>> FindByIdsAsync(int[] ids, CancellationToken cancellationToken)
    {
        return await GetBaseQuery()
            .Where(e => Enumerable.Contains(ids, e.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasCupoRelated(int id, CancellationToken cancellationToken)
    {
        return await Context.Set<Cupo>()
            .AnyAsync(x => x.EscenarioCupo.Id == id);

    }
}
