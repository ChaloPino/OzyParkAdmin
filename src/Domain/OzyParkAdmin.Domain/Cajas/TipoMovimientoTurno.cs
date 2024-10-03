namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// El tipo de movimiento del turno.
/// </summary>
public enum TipoMovimientoTurno
{
    /// <summary>
    /// Movimiento de apertura.
    /// </summary>
    Apertura = 1,

    /// <summary>
    /// Movimiento de cierre.
    /// </summary>
    Cierre = 2,

    /// <summary>
    /// Movimiento de remesa.
    /// </summary>
    Remesa = 3,

    /// <summary>
    /// Movimiento de depósito.
    /// </summary>
    Deposito = 4,

    /// <summary>
    /// Movimiento de retiro.
    /// </summary>
    Retiro = 5,

    /// <summary>
    /// Movimiento de cierre por POS
    /// </summary>
    CierrePOS = 6,
}