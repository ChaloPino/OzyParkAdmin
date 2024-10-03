namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// Resumen del turno de caja.
/// </summary>
public sealed record ResumenTurnoInfo
{
    /// <summary>
    /// Si es abono.
    /// </summary>
    public bool EsAbono { get; set; }

    /// <summary>
    /// El tipo de movimiento.
    /// </summary>
    public string Tipo { get; set; } = string.Empty;

    /// <summary>
    /// El movimiento.
    /// </summary>
    public string Movimiento { get; set; } = string.Empty;

    /// <summary>
    /// La cantidad del movimiento.
    /// </summary>
    public int Cantidad { get; set; }

    /// <summary>
    /// El monto del movimiento.
    /// </summary>
    public decimal Monto { get; set; }
}