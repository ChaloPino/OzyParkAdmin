using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;

namespace OzyParkAdmin.Domain.Repositories;

/// <summary>
/// Interfaz del repositorio para la entidad <see cref="DetalleEscenarioCupoExclusionFecha"/>.
/// </summary>
public interface IDetalleEscenarioCupoExclusionFechaRepository
{
    /// <summary>
    /// Obtiene todas las exclusiones por fecha para un escenario de cupo específico.
    /// </summary>
    /// <param name="escenarioCupoId">El identificador del escenario de cupo.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Una lista de <see cref="DetalleEscenarioCupoExclusionFecha"/>.</returns>
    Task<IEnumerable<DetalleEscenarioCupoExclusionFecha>> GetExclusionesByEscenarioCupoIdAsync(int escenarioCupoId, CancellationToken cancellationToken);


    /// <summary>
    /// Obtiene todas las exclusiones por fecha para un escenario de cupo específico.
    /// </summary>
    /// <param name="escenarioCupoId">El identificador del escenario de cupo.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Una lista de <see cref="DetalleEscenarioCupoExclusionFecha"/>.</returns>
    Task<IEnumerable<DetalleEscenarioCupoExclusionFecha>> GetSimpleExclusionesByEscenarioCupoIdAsync(int escenarioCupoId, CancellationToken cancellationToken);

}
