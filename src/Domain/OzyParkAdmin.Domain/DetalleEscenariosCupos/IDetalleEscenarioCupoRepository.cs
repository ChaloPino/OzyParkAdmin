namespace OzyParkAdmin.Domain.DetallesEscenariosCupos;

/// <summary>
/// Repositorio para manejar las operaciones de acceso a datos de <see cref="DetalleEscenarioCupo"/>.
/// </summary>
public interface IDetalleEscenarioCupoRepository
{
    /// <summary>
    /// Obtiene los detalles asociados a un escenario de cupo por su ID.
    /// </summary>
    Task<IEnumerable<DetalleEscenarioCupo>> GetDetallesByEscenarioCupoIdAsync(int escenarioCupoId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene los detalles asociados a un escenario de cupo por su ID.
    /// </summary>
    Task<IEnumerable<DetalleEscenarioCupo>> FindByIdsAsync(int escenarioCupoId, CancellationToken cancellationToken);
    /// <summary>
    /// Agrega múltiples detalles asociados a un escenario de cupo.
    /// </summary>
    Task AddDetallesAsync(IEnumerable<DetalleEscenarioCupo> detalles, CancellationToken cancellationToken);

    /// <summary>
    /// Actualiza un detalle asociado a un escenario de cupo.
    /// </summary>
    Task UpdateDetalleAsync(DetalleEscenarioCupo detalle, CancellationToken cancellationToken);

    /// <summary>
    /// Elimina múltiples detalles asociados a un escenario de cupo.
    /// </summary>
    Task RemoveDetallesAsync(IEnumerable<DetalleEscenarioCupo> detalles, CancellationToken cancellationToken);
}