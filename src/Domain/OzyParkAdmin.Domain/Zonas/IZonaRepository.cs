
namespace OzyParkAdmin.Domain.Zonas;

/// <summary>
/// El repositorio de <see cref="Zona"/>.
/// </summary>
public interface IZonaRepository
{
    /// <summary>
    /// Busca todas las zonas que coincidan con <paramref name="zonaIds"/>.
    /// </summary>
    /// <param name="zonaIds">Los id de las zonas a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de zonas.</returns>
    Task<IEnumerable<Zona>> FindByIdsAsync(int[] zonaIds, CancellationToken cancellationToken);

    /// <summary>
    /// Lista todas las zonas.
    /// </summary>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de zonas.</returns>
    Task<List<ZonaInfo>> ListZonasAsync(CancellationToken cancellationToken);
}
