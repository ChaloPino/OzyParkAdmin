using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.EscenariosCupo;

/// <summary>
/// El repositorio de <see cref="EscenarioCupo"/>.
/// </summary>
public interface IEscenarioCupoRepository
{
    /// <summary>
    /// Busca si un escenrario tiene un cupo asociado.
    /// </summary>
    /// <param name="id">El identificador del escenario de cupo a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El escenario de cupo si es encontrado, de lo contrario, null.</returns>
    Task<bool> HasCupoRelated(int id, CancellationToken cancellationToken);


    /// <summary>
    /// Busca un escenario de cupo por su id.
    /// </summary>
    /// <param name="id">El identificador del escenario de cupo a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El escenario de cupo si es encontrado, de lo contrario, null.</returns>
    Task<EscenarioCupo?> FindByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Busca varios escenarios de cupo por sus ids.
    /// </summary>
    /// <param name="ids">Los identificadores de los escenarios de cupo a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Una colección de los escenarios de cupo encontrados.</returns>
    Task<IEnumerable<EscenarioCupo>> FindByIdsAsync(int[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Busca escenarios de cupo que coincidan con los criterios de búsqueda.
    /// </summary>
    /// <param name="centrosCostoId">Una lista opcional de identificadores de centros de costo para filtrar.</param>
    /// <param name="zonasId">Una lista opcional de identificadores de zonas para filtrar.</param>
    /// <param name="searchText">Un texto opcional para buscar en los campos de nombre, zona o centro de costo.</param>
    /// <param name="filterExpressions">Las expresiones de filtro adicionales que se deben aplicar.</param>
    /// <param name="sortExpressions">Las expresiones de ordenamiento que se deben aplicar.</param>
    /// <param name="page">El número de la página de resultados a devolver.</param>
    /// <param name="pageSize">El tamaño de la página de resultados.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Una lista paginada de información detallada de escenarios de cupo.</returns>
    Task<PagedList<EscenarioCupoFullInfo>> SearchAsync(
        int[]? centrosCostoId,
        int[]? zonasId,
        string? searchText,
        FilterExpressionCollection<EscenarioCupo> filterExpressions,
        SortExpressionCollection<EscenarioCupo> sortExpressions,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Lista todos los escenarios de cupo filtrados opcionalmente por centro de costo.
    /// </summary>
    /// <param name="centroCostoIds">Una lista opcional de identificadores de centros de costo.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Una lista de escenarios de cupo encontrados.</returns>
    Task<List<EscenarioCupoInfo>> ListAsync(int[]? centroCostoIds, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene el último id del escenario cupo.
    /// </summary>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El último id utilizado por un escenario de cupo.</returns>
    Task<int> GetLastIdAsync(CancellationToken cancellationToken);
}
