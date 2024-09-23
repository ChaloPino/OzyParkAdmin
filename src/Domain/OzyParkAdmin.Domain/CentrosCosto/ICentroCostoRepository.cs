namespace OzyParkAdmin.Domain.CentrosCosto;

/// <summary>
/// El repositorio de <see cref="CentroCosto"/>.
/// </summary>
public interface ICentroCostoRepository
{
    /// <summary>
    /// Busca un centro de costo por su id.
    /// </summary>
    /// <param name="centroCostoId">El id del centro de costo a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El centro de costo si existe.</returns>
    Task<CentroCosto?> FindByIdAsync(int centroCostoId, CancellationToken cancellationToken);

    /// <summary>
    /// Busca centros de costos que coincidan con <paramref name="centroCostoIds"/>.
    /// </summary>
    /// <param name="centroCostoIds">Los id de centros de costo a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de centros de costo.</returns>
    Task<IEnumerable<CentroCosto>> FindByIdsAsync(int[] centroCostoIds, CancellationToken cancellationToken);

    /// <summary>
    /// Busca todos los centros de costo activos que coincidan con los ids.
    /// </summary>
    /// <param name="centroCostoIds">Los ids de centros de costo.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de centros de costo.</returns>
    Task<List<CentroCostoInfo>> ListCentrosCostoAsync(int[]? centroCostoIds, CancellationToken cancellationToken);
}
