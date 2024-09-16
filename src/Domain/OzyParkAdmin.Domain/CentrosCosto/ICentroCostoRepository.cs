
namespace OzyParkAdmin.Domain.CentrosCosto;

/// <summary>
/// El repositorio de <see cref="CentroCosto"/>.
/// </summary>
public interface ICentroCostoRepository
{
    /// <summary>
    /// Busca todos los centros de costo activos que coincidan con los ids.
    /// </summary>
    /// <param name="centroCostoIds">Los ids de centros de costo.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de centros de costo.</returns>
    Task<List<CentroCosto>> ListCentrosCostoAsync(int[]? centroCostoIds, CancellationToken cancellationToken);
}
