namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// Información de servicios que se crearon en el día.
/// </summary>
public sealed record ServicioDiaInfo
{
    /// <summary>
    /// El número de venta.
    /// </summary>
    public string VentaId { get; set; } = string.Empty;

    /// <summary>
    /// El ejecutivo que realizó la venta.
    /// </summary>
    public string Ejecutivo { get; set; } = string.Empty;

    /// <summary>
    /// La fecha de la venta.
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// La hora de la venta.
    /// </summary>
    public TimeSpan Hora { get; set; }

    /// <summary>
    /// El monto total por la venta del servicio.
    /// </summary>
    public decimal Monto { get; set; }

    /// <summary>
    /// La cantidad de tickets del servicio.
    /// </summary>
    public int Cantidad { get; set; }

    /// <summary>
    /// El nombre del servicio.
    /// </summary>
    public string Servicio { get; set; } = string.Empty;

    /// <summary>
    /// La cantidad de guías.
    /// </summary>
    public int Guias { get; set; }
}
