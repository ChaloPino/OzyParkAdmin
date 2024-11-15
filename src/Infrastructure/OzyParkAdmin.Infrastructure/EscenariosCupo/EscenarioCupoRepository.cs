using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Zonas;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.EscenariosCupo;

/// <summary>
/// El repositorio de <see cref="EscenarioCupo"/>.
/// </summary>
public class EscenarioCupoRepository(OzyParkAdminContext context) : Repository<EscenarioCupo>(context), IEscenarioCupoRepository
{
    public async Task AddDetallesAsync(IEnumerable<DetalleEscenarioCupoInfo> detalles, CancellationToken cancellationToken)
    {
        // Validar que los detalles no sean nulos
        ArgumentNullException.ThrowIfNull(detalles, nameof(detalles));

        // Convertir los DetalleEscenarioCupoInfo a DetalleEscenarioCupo usando el método Create
        var detallesEntidades = detalles.Select(d =>
        {
            return DetalleEscenarioCupo.Create(
                d.EscenarioCupoId,
                d.ServicioId,
                d.TopeDiario,
                d.UsaSobreCupo,
                d.HoraMaximaVenta!.Value,
                d.HoraMaximaRevalidacion!.Value,
                d.UsaTopeEnCupo,
                d.TopeFlotante
            );
        }).ToList();

        // Agregar los detalles al contexto
        await Context.Set<DetalleEscenarioCupo>().AddRangeAsync(detallesEntidades, cancellationToken);
    }

    public async Task<bool> ExistsWithSimilarNameAsync(string nombre, int? excludeId, CancellationToken cancellationToken)
    {
        return await Context.Set<EscenarioCupo>()
            .AsNoTracking()
            .Where(e => e.Nombre == nombre && (excludeId == null || e.Id != excludeId))
            .AnyAsync(cancellationToken);
    }

    public async Task<EscenarioCupo?> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await Context.Set<EscenarioCupo>()
            .Include(e => e.DetallesEscenarioCupo)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<EscenarioCupo>> FindByIdsAsync(int[] ids, CancellationToken cancellationToken)
    {
        return await Context.Set<EscenarioCupo>()
            .Where(e => ids.Contains(e.Id))
            .Include(e => e.DetallesEscenarioCupo)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EscenarioCupo>> FindEscenariosAsync(int centroCostoId, int? zonaId, CancellationToken cancellationToken)
    {
        return await Context.Set<EscenarioCupo>()
            .Where(e => e.CentroCosto.Id == centroCostoId && (zonaId == null || e.Zona!.Id == zonaId))
            .Include(e => e.DetallesEscenarioCupo)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<EscenarioCupo>> FindEscenariosAsync(IEnumerable<EscenarioCupoFullInfo> escenariosCupos, CancellationToken cancellationToken)
    {
        // Implementa la lógica para buscar escenarios basándote en EscenarioCupoFullInfo
        throw new NotImplementedException();
    }

    public async Task<EscenarioCupo?> FindEscenarioAsync(int centroCostoId, int? zonaId, string nombre, CancellationToken cancellationToken)
    {
        return await Context.Set<EscenarioCupo>()
            .FirstOrDefaultAsync(e => e.CentroCosto.Id == centroCostoId && (zonaId == null || e.Zona!.Id == zonaId) && e.Nombre == nombre, cancellationToken);
    }

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
        ArgumentNullException.ThrowIfNull(filterExpressions, nameof(filterExpressions));
        ArgumentNullException.ThrowIfNull(sortExpressions, nameof(sortExpressions));

        // Base query para buscar escenarios de cupo
        var query = Context.Set<EscenarioCupo>().AsNoTracking();

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
        var paginatedQuery = query.Skip(page * pageSize).Take(pageSize);

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
            EsActivo = ec.EsActivo,
            Detalles = ec.DetallesEscenarioCupo.Select(d => new DetalleEscenarioCupoInfo
            {
                EscenarioCupoId = d.EscenarioCupoId,
                ServicioId = d.ServicioId,
                Servicio = new ServicioInfo
                {
                    Id = d.Servicio.Id,
                    Aka = d.Servicio.Aka,
                    Nombre = d.Servicio.Nombre
                },
                TopeDiario = d.TopeDiario,
                UsaSobreCupo = d.UsaSobreCupo,
                HoraMaximaVenta = d.HoraMaximaVenta,
                HoraMaximaRevalidacion = d.HoraMaximaRevalidacion,
                UsaTopeEnCupo = d.UsaTopeEnCupo,
                TopeFlotante = d.TopeFlotante
            }).ToList()
        }).ToListAsync(cancellationToken).ConfigureAwait(false);

        // Construir el objeto paginado
        return new PagedList<EscenarioCupoFullInfo>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }


    public async Task<List<EscenarioCupoInfo>> ListAsync(int[]? centroCostoIds, CancellationToken cancellationToken)
    {
        return await Context.Set<EscenarioCupo>()
            .Where(e => centroCostoIds == null || centroCostoIds.Contains(e.CentroCosto.Id))
            .Select(e => new EscenarioCupoInfo { Id = e.Id, Nombre = e.Nombre })
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetLastIdAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<EscenarioCupo>().MaxAsync(e => e.Id, cancellationToken);
    }

    public async Task AddAsync(EscenarioCupo escenarioCupo, CancellationToken cancellationToken)
    {
        await Context.Set<EscenarioCupo>().AddAsync(escenarioCupo, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<EscenarioCupo> escenariosCupos, CancellationToken cancellationToken)
    {
        await Context.Set<EscenarioCupo>().AddRangeAsync(escenariosCupos, cancellationToken);
    }

    public async Task AddDetallesAsync(IEnumerable<DetalleEscenarioCupo> detalles, CancellationToken cancellationToken)
    {
        await Context.Set<DetalleEscenarioCupo>().AddRangeAsync(detalles, cancellationToken);
    }

    public async Task RemoveAsync(EscenarioCupo escenarioCupo, CancellationToken cancellationToken)
    {
        Context.Set<EscenarioCupo>().Remove(escenarioCupo);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(EscenarioCupo escenarioCupo, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(escenarioCupo, nameof(escenarioCupo));

        // Buscar la entidad existente en el contexto
        var existingEntity = await Context.Set<EscenarioCupo>()
            .Include(e => e.DetallesEscenarioCupo) // Incluir detalles relacionados
            .FirstOrDefaultAsync(e => e.Id == escenarioCupo.Id, cancellationToken);

        if (existingEntity == null)
        {
            throw new InvalidOperationException($"No se encontró un EscenarioCupo con Id {escenarioCupo.Id}");
        }

        // Usar CreateOrUpdate para actualizar la entidad existente
        var updateResult = EscenarioCupo.CreateOrUpdate(
            id: existingEntity.Id,
            escenarioExistente: existingEntity,
            centroCosto: escenarioCupo.CentroCosto,
            zona: escenarioCupo.Zona,
            nombre: escenarioCupo.Nombre,
            esHoraInicio: escenarioCupo.EsHoraInicio,
            minutosAntes: escenarioCupo.MinutosAntes,
            esActivo: escenarioCupo.EsActivo
        );

        if (updateResult.IsFailure(out var failure))
        {
            throw new InvalidOperationException($"Falló la actualización del EscenarioCupo: {failure}");
        }

        // Actualizar los detalles
        UpdateDetalles(existingEntity, escenarioCupo.DetallesEscenarioCupo);

        // Marcar la entidad como modificada
        Context.Entry(existingEntity).State = EntityState.Modified;

        // Guardar cambios
        await Context.SaveChangesAsync(cancellationToken);
    }

    private static void UpdateDetalles(EscenarioCupo existingEntity, IEnumerable<DetalleEscenarioCupo> nuevosDetalles)
    {
        // Eliminar detalles no incluidos en la nueva lista
        var detallesParaEliminar = existingEntity.DetallesEscenarioCupo
            .Where(d => !nuevosDetalles.Any(nd => nd.ServicioId == d.ServicioId))
            .ToList();

        foreach (var detalle in detallesParaEliminar)
        {
            existingEntity.DetallesEscenarioCupo.Remove(detalle);
        }

        // Actualizar o agregar nuevos detalles
        foreach (var nuevoDetalle in nuevosDetalles)
        {
            var detalleExistente = existingEntity.DetallesEscenarioCupo
                .FirstOrDefault(d => d.ServicioId == nuevoDetalle.ServicioId);

            if (detalleExistente != null)
            {
                // Actualizar propiedades del detalle existente
                detalleExistente.Update(nuevoDetalle);
            }
            else
            {
                // Agregar nuevo detalle
                existingEntity.DetallesEscenarioCupo.Add(nuevoDetalle);
            }
        }
    }
}
