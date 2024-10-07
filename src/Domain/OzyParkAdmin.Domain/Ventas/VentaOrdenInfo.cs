namespace OzyParkAdmin.Domain.Ventas;

/// <summary>
/// La información de la venta con orden de compra.
/// </summary>
public sealed record VentaOrdenInfo
{
    /// <summary>
    /// El id de la venta.
    /// </summary>
    public string VentaId { get; set; } = string.Empty;

    /// <summary>
    /// El nombre del cliente.
    /// </summary>
    public string Nombres { get; set; } = string.Empty;

    /// <summary>
    /// Los apellidos del cliente.
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// La dirección de correo electrónico del cliente.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// El número de teléfono del cliente.
    /// </summary>
    public string Telefono { get; set; } = string.Empty;

    /// <summary>
    /// La fecha de la venta.
    /// </summary>
    public DateTime FechaVenta { get; set; }

    /// <summary>
    /// La caja donde se hizo la venta.
    /// </summary>
    public int CajaId { get; set; }

    /// <summary>
    /// La franquicia a la que pertenece la caja.
    /// </summary>
    public int FranquiciaId { get; set; }

    /// <summary>
    /// Los tickets que se vendieron en la venta.
    /// </summary>
    public List<TicketOrdenInfo> Tickets { get; set; } = [];
}
