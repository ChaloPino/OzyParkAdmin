namespace OzyParkAdmin.Domain.Ventas;

/// <summary>
/// El ticket generado por una orden de compra.
/// </summary>
public sealed record TicketOrdenInfo
{
    /// <summary>
    /// El id de venta.
    /// </summary>
    public string VentaId { get; set; } = string.Empty;

    /// <summary>
    /// El id del ticket.
    /// </summary>
    public string TicketId { get; set; } = string.Empty;

    /// <summary>
    /// El nombre del servicio.
    /// </summary>
    public string Servicio { get; set; } = string.Empty;

    /// <summary>
    /// La cantidad de pasajeros.
    /// </summary>
    public int NumeroPasajeros { get; set; }

    /// <summary>
    /// El inicio de vigencia del ticket.
    /// </summary>
    public DateTime InicioVigencia { get; set; }

    /// <summary>
    /// El fin de vigencia del ticket.
    /// </summary>
    public DateTime FinVigencia { get; set; }

    /// <summary>
    /// Si el ticket ya está usado.
    /// </summary>
    public bool EsUsado { get; set; }

    /// <summary>
    /// El tipo de anulación del ticket.
    /// <para>N - no está anulado.</para>
    /// <para>T - está anulado.</para>
    /// </summary>
    public string TipoAnulacion { get; set; } = "N";

    /// <summary>
    /// Valor que indica si se puede descargar un ticket.
    /// </summary>
    /// <remarks>
    /// Solo se puede descargar si el ticket no está usado y no está anulado.
    /// </remarks>
    public bool PuedeDescargarse => !EsUsado && string.Equals(TipoAnulacion, "N", StringComparison.OrdinalIgnoreCase);
}