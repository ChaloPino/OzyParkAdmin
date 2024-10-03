namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// El detalle del turno de caja.
/// </summary>
public sealed record DetalleTurnoInfo
{
    /// <summary>
    /// Si es abono.
    /// </summary>
    public bool EsAbono { get; set; }

    /// <summary>
    /// El tipo del movimiento.
    /// </summary>
    public string Tipo { get; set; } = string.Empty;

    /// <summary>
    /// La fecha del movimiento.
    /// </summary>
    public DateOnly Fecha { get; set; }

    /// <summary>
    /// La hora del movimiento.
    /// </summary>
    public TimeSpan Hora { get; set; }

    /// <summary>
    /// El aka de la forma de pago.
    /// </summary>
    public string FormaPagoAka { get; set; } = string.Empty;

    /// <summary>
    /// La forma de pago.
    /// </summary>
    public string FormaPago { get; set; } = string.Empty;

    /// <summary>
    /// El monto del movimiento.
    /// </summary>
    public decimal Monto { get; set; }

    /// <summary>
    /// El monto del movimiento con signo.
    /// </summary>
    public decimal MontoConSigno { get; set; }

    /// <summary>
    /// El número de referencia.
    /// </summary>
    public string? NumeroReferencia { get; set; }

    /// <summary>
    /// El supervisor.
    /// </summary>
    public string? Supervisor { get; set; }

    /// <summary>
    /// El usuario.
    /// </summary>
    public string Usuario { get; set; } = string.Empty;

    /// <summary>
    /// Si tiene acción.
    /// </summary>
    public bool TieneAccion { get; set; }

    /// <summary>
    /// El nombre del movimiento.
    /// </summary>
    public string Movimiento { get; set; } = string.Empty;

    /// <summary>
    /// El orden de despliegue del movimiento.
    /// </summary>
    public int Orden { get; set; }

}
