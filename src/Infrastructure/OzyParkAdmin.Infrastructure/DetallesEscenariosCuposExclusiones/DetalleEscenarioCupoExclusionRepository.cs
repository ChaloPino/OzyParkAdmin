using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.DetallesEscenariosCuposExclusiones;
public sealed class DetalleEscenarioCupoExclusionRepository(OzyParkAdminContext context) : Repository<DetalleEscenarioCupoExclusion>(context), IDetalleEscenarioCupoExclusionRepository
{

    /// <summary>
    /// Obtiene las exclusiones de fechas para un escenario de cupo específico.
    /// </summary>
    public async Task<IEnumerable<DetalleEscenarioCupoExclusion>> GetExclusionesByEscenarioCupoIdAsync(int escenarioCupoId, CancellationToken cancellationToken)
    {
        return await EntitySet
            .AsSplitQuery()
            .Where(x => x.EscenarioCupoId == escenarioCupoId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<PagedList<DetalleEscenarioCupoExclusionFullInfo>> SearchAsync(
        int[]? serviciosIds,
        int[]? canalesVentaIds,
        int[]? diasSemanaIds,
        int escenarioCupoId,
        string? searchText,
        FilterExpressionCollection<DetalleEscenarioCupoExclusion> filterExpressions,
        SortExpressionCollection<DetalleEscenarioCupoExclusion> sortExpressions,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filterExpressions);
        ArgumentNullException.ThrowIfNull(sortExpressions);

        // Base query para buscar escenarios de cupo
        var query = Context.Set<DetalleEscenarioCupoExclusion>().AsNoTracking();

        //Filtrar para el escenario cupo que se está pasando primeramente
        query = query.Where(ec => ec.EscenarioCupoId == escenarioCupoId);

        // Filtrar por centros de costo si se proporciona
        if (serviciosIds is not null && serviciosIds.Any())
        {
            query = query.Where(ec => serviciosIds.Contains(ec.ServicioId));
        }

        // Filtrar por canales de venta si se proporciona
        if (canalesVentaIds is not null && canalesVentaIds.Any())
        {
            query = query.Where(ec => canalesVentaIds.Contains(ec.CanalVentaId));
        }

        // Filtrar por días de la semana si se proporciona
        if (diasSemanaIds is not null && diasSemanaIds.Any())
        {
            query = query.Where(ec => diasSemanaIds.Contains(ec.DiaSemanaId));
        }

        // Filtrar por texto de búsqueda si se proporciona
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(ec => ec.Servicio.Nombre.Contains(searchText) || ec.CanalVenta.Nombre.Contains(searchText) || ec.DiaSemana.Aka.Contains(searchText));
        }

        // Aplicar filtros adicionales mediante las expresiones de filtro proporcionadas
        query = filterExpressions.Where(query);

        // Contar el total de resultados antes de la paginación
        int totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        // Ordenar la consulta según las expresiones de ordenamiento proporcionadas
        query = sortExpressions.Sort(query);

        // Aplicar la paginación
        var paginatedQuery = query.Skip(page * pageSize).Take(pageSize);

        // Proyectar los resultados a EscenarioCupoFullInfo
        var items = await paginatedQuery.Select(ec => new DetalleEscenarioCupoExclusionFullInfo
        {

            EscenarioCupoId = ec.EscenarioCupoId,
            ServicioId = ec.ServicioId,
            CanalVentaId = ec.CanalVentaId,
            DiaSemanaId = ec.DiaSemanaId,
            CanalVentaNombre = ec.CanalVenta.Nombre,
            ServicioNombre = ec.Servicio.Nombre,
            DiaSemanaNombre = ec.DiaSemana.Aka,
            HoraInicio = ec.HoraInicio,
            HoraFin = ec.HoraFin


        }).ToListAsync();

        return new PagedList<DetalleEscenarioCupoExclusionFullInfo>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }


}
