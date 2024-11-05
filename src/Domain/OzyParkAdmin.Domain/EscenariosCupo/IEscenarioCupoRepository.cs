

namespace OzyParkAdmin.Domain.EscenariosCupo;

/// <summary>
/// El repositorio de <see cref="EscenarioCupo"/>.
/// </summary>
public interface IEscenarioCupoRepository
{
    /// <summary>
    /// Busca un escenario de cupo por su id.
    /// </summary>
    /// <param name="id">El id del escenario de cupo a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El escenario de cupo si existe.</returns>
    Task<EscenarioCupo?> FindByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Busca varios escenarios de cupo por sus ids.
    /// </summary>
    /// <param name="ids">Los ids de escenarios de cupos a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de escenarios de cupo.</returns>
    Task<IEnumerable<EscenarioCupo>> FindByIdsAsync(int[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Lista todos los escenarios de cupo.
    /// </summary>
    /// <param name="centroCostoIds">Los ids de centro de costo al que pertenecen los escenarios de cupo.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de escenarios de cupo.</returns>
    Task<List<EscenarioCupoInfo>> ListAsync(int[]? centroCostoIds, CancellationToken cancellationToken);

}
