
namespace OzyParkAdmin.Domain.Tramos;

/// <summary>
/// El repositorio de <see cref="Tramo"/>.
/// </summary>
public interface ITramoRepository
{
    /// <summary>
    /// Busca los tramos que coincidan con <paramref name="tramoIds"/>.
    /// </summary>
    /// <param name="tramoIds">Los id de tramos.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de tramos.</returns>
    Task<IEnumerable<Tramo>> FindByIdsAsync(int[] tramoIds, CancellationToken cancellationToken);

    /// <summary>
    /// Lista todos los tramos.
    /// </summary>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de tramos.</returns>
    Task<List<TramoInfo>> ListTramosAsync(CancellationToken cancellationToken);
}