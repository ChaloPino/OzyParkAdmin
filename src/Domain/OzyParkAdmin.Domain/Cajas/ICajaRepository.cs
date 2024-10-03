
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// El repositorio de <see cref="Caja"/>.
/// </summary>
public interface ICajaRepository
{
    /// <summary>
    /// Busca el detalle de una apertura de día de una caja.
    /// </summary>
    /// <param name="aperturaCajaId">El id de la apertura de día.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El detalle de la apertura de día si existe.</returns>
    Task<AperturaCajaDetalleInfo?> FindAperturaCajaDetalleAsync(Guid aperturaCajaId, CancellationToken cancellationToken);

    /// <summary>
    /// Busca las cajas que coincidan con los <paramref name="cajaIds"/>.
    /// </summary>
    /// <param name="cajaIds">Los id de caja.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de cajas.</returns>
    Task<IEnumerable<Caja>> FindByIdsAsync(int[] cajaIds, CancellationToken cancellationToken);

    /// <summary>
    /// Busca una apertura de día de una caja por si id.
    /// </summary>
    /// <param name="diaId">El id de la apertrua de día de la caja.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El turno de caja si existe.</returns>
    Task<AperturaDia?> FindAperturaDiaAsync(Guid diaId, CancellationToken cancellationToken);

    /// <summary>
    /// Lista todas las cajas.
    /// </summary>
    /// <param name="centroCostoIds">Los ids de centros de costo.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de cajas.</returns>
    Task<List<CajaInfo>> ListCajasAsync(int[]? centroCostoIds, CancellationToken cancellationToken);

    /// <summary>
    /// Busca todas las aperturas de caja que cumplan con los criterios de búsqueda.
    /// </summary>
    /// <param name="centroCostoId">El id de centro de costo a buscar.</param>
    /// <param name="searchText">El texto de búsqueda.</param>
    /// <param name="searchDate">El día de búsqueda.</param>
    /// <param name="filterExpressions">Las expresiones de filtrado.</param>
    /// <param name="sortExpressions">Las expresiones de ordenamiento.</param>
    /// <param name="page">La página actual.</param>
    /// <param name="pageSize">El tamaño de la página actual.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista paginada de aperturas de caja.</returns>
    Task<PagedList<AperturaCajaInfo>> SearchAperturaCajasAsync(int centroCostoId, string? searchText, DateOnly searchDate, FilterExpressionCollection<AperturaCajaInfo> filterExpressions, SortExpressionCollection<AperturaCajaInfo> sortExpressions, int page, int pageSize, CancellationToken cancellationToken);
}
