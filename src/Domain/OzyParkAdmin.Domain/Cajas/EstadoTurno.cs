namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// El estado de un turno.
/// </summary>
public enum EstadoTurno
{
    /// <summary>
    /// Turno abierto.
    /// </summary>
    Abierto = 0,

    /// <summary>
    /// Cerrado por el ejecutivo pendiente a revisión y ajustes.
    /// </summary>
    Pendiente = 1,

    /// <summary>
    /// Cerrado por el supervisor después de revisión y ajustes.
    /// </summary>
    Cerrado = 2,
}