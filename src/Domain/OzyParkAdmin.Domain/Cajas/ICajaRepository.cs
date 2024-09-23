
namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// El repositorio de <see cref="Caja"/>.
/// </summary>
public interface ICajaRepository
{
    /// <summary>
    /// Busca las cajas que coincidan con los <paramref name="cajaIds"/>.
    /// </summary>
    /// <param name="cajaIds">Los id de caja.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de cajas.</returns>
    Task<IEnumerable<Caja>> FindByIdsAsync(int[] cajaIds, CancellationToken cancellationToken);

    /// <summary>
    /// Lista todas las cajas.
    /// </summary>
    /// <param name="centroCostoIds">Los ids de centros de costo.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de cajas.</returns>
    Task<List<CajaInfo>> ListCajasAsync(int[]? centroCostoIds, CancellationToken cancellationToken);
}
