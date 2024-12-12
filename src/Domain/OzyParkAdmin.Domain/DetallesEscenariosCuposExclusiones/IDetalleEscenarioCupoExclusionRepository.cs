using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
public interface IDetalleEscenarioCupoExclusionRepository
{
    /// <summary>
    /// Obtiene todas las exclusiones para un escenario de cupo específico.
    /// </summary>
    /// <param name="escenarioCupoId">El identificador del escenario de cupo.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Una lista de <see cref="DetalleEscenarioCupoExclusion"/>.</returns>
    Task<IEnumerable<DetalleEscenarioCupoExclusion>> GetExclusionesByEscenarioCupoIdAsync(int escenarioCupoId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene todas las exclusiones para un escenario de cupo específico.
    /// </summary>
    /// <param name="escenarioCupoId">El identificador del escenario de cupo.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Una lista de <see cref="DetalleEscenarioCupoExclusionFullInfo"/>.</returns>
    Task<IEnumerable<DetalleEscenarioCupoExclusionFullInfo>> GetExclusionesInfoByEscenarioCupoIdAsync(int escenarioCupoId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene todas las exclusiones para un escenario de cupo específico.
    /// </summary>
    /// <param name="escenarioCupoId">El identificador del escenario de cupo.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Una lista de <see cref="DetalleEscenarioCupoExclusionFullInfo"/>.</returns>
    Task<IEnumerable<DetalleEscenarioCupoExclusionFullInfo>> List(int escenarioCupoId, CancellationToken cancellationToken);

    /// <summary>
    /// Busca escenarios de cupo que coincidan con los criterios de búsqueda.
    /// </summary>
    /// <param name="serviciosIds">Lista de los servicios IDS a los que se asignó.</param>
    /// <param name="canalesVentaIds">Lista de los canales de venta IDS a los que se asignó.</param>
    /// <param name="diasSemanaIds">Lista de los días de semana IDS a los que se asignó.</param>
    /// <param name="searchText">Un texto opcional para buscar en los campos de nombre, zona o centro de costo.</param>
    /// <param name="filterExpressions">Las expresiones de filtro adicionales que se deben aplicar.</param>
    /// <param name="sortExpressions">Las expresiones de ordenamiento que se deben aplicar.</param>
    /// <param name="page">El número de la página de resultados a devolver.</param>
    /// <param name="pageSize">El tamaño de la página de resultados.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Una lista paginada de información detallada de escenarios de cupo.</returns>
    Task<PagedList<DetalleEscenarioCupoExclusionFullInfo>> SearchAsync(
        int[]? serviciosIds,
        int[]? canalesVentaIds,
        int[]? diasSemanaIds,
        int escenarioCupoId,
        string? searchText,
        FilterExpressionCollection<DetalleEscenarioCupoExclusion> filterExpressions,
        SortExpressionCollection<DetalleEscenarioCupoExclusion> sortExpressions,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
