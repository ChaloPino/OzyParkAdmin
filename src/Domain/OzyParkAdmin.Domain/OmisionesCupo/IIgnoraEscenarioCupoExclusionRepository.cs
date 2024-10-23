using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.OmisionesCupo;

/// <summary>
/// El repositorio de <see cref="IgnoraEscenarioCupoExclusion"/>.
/// </summary>
public interface IIgnoraEscenarioCupoExclusionRepository
{
    /// <summary>
    /// Busca varios <see cref="IgnoraEscenarioCupoExclusion"/> dados sus claves primarias.
    /// </summary>
    /// <param name="keys">Las claves primarias.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de <see cref="IgnoraEscenarioCupoExclusion"/>.</returns>
    Task<IEnumerable<IgnoraEscenarioCupoExclusion>> FindByKeysAsync(IEnumerable<(int EscenarioCupoId, int CanalVentaId, DateOnly Fecha)> keys, CancellationToken cancellationToken);

    /// <summary>
    /// Busca varias omisiones de exclusión de escenarios de cupo que coincidan con los criterios de búsqueda.
    /// </summary>
    /// <param name="searchText">Un texto libre para buscar en el escenario y el canal de venta.</param>
    /// <param name="filterExpressions">Las expresiones de filtrado.</param>
    /// <param name="sortExpressions">Las expresiones de ordenamiento.</param>
    /// <param name="page">La página actual.</param>
    /// <param name="pageSize">El tamaño de la página actual.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista paginada de <see cref="IgnoraEscenarioCupoExclusion"/>.</returns>
    Task<PagedList<IgnoraEscenarioCupoExclusionFullInfo>> SearchAsync(string? searchText, FilterExpressionCollection<IgnoraEscenarioCupoExclusion> filterExpressions, SortExpressionCollection<IgnoraEscenarioCupoExclusion> sortExpressions, int page, int pageSize, CancellationToken cancellationToken);
}