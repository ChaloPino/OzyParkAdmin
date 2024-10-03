namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// El estado del movimiento de turno.
/// </summary>
public enum EstadoMovimientoTurno
{
    /// <summary>
    /// Estado normal del movimiento.
    /// </summary>
    Activo = 0,

    /// <summary>
    /// Estado anulado del movimiento.
    /// </summary>
    Anulado = 1,
}