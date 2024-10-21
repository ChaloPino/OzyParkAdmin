using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.ExclusionesCupo;

/// <summary>
/// El repositorio de <see cref="FechaExcluidaCupo"/>.
/// </summary>
public interface IFechaExcluidaCupoRepository
{
    /// <summary>
    /// Busca fechas excluidas dado el <paramref name="centroCostoId"/>, los <paramref name="canalesVentaId"/> y las <paramref name="fechas"/>.
    /// </summary>
    /// <param name="centroCostoId">El id del centro de costo.</param>
    /// <param name="canalesVentaId">Los ids de los canales de venta.</param>
    /// <param name="fechas">Las fechas.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Las fechas excluidas que coinciden</returns>
    Task<IEnumerable<FechaExcluidaCupo>> FindFechasExcluidasAsync(int centroCostoId, int[] canalesVentaId, DateOnly[] fechas, CancellationToken cancellationToken);

    /// <summary>
    /// Busca fechas excluidas.
    /// </summary>
    /// <param name="fechasExcluidas">La información de las fechas excluidas de cupos que se quiere encontrar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Las fechas excluidas que coinciden</returns>
    Task<IEnumerable<FechaExcluidaCupo>> FindFechasExcluidasAsync(IEnumerable<FechaExcluidaCupoFullInfo> fechasExcluidas, CancellationToken cancellationToken);

    /// <summary>
    /// Busca fechas excluidas que coincidan con los criterios de búsqueda.
    /// </summary>
    /// <param name="centrosCostoId">Los ids de centros de costo a buscar.</param>
    /// <param name="searchText">El texto a buscar.</param>
    /// <param name="filterExpressions">Las expresiones de filtrado.</param>
    /// <param name="sortExpressions">Las expresiones de ordenamiento.</param>
    /// <param name="page">La página actual.</param>
    /// <param name="pageSize">El tamaño de la página actual.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista paginada con las fechas excluidas que coinciden con los criterios de búsqueda.</returns>
    Task<PagedList<FechaExcluidaCupoFullInfo>> SearchAsync(
        int[]? centrosCostoId,
        string? searchText,
        FilterExpressionCollection<FechaExcluidaCupo> filterExpressions,
        SortExpressionCollection<FechaExcluidaCupo> sortExpressions,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
