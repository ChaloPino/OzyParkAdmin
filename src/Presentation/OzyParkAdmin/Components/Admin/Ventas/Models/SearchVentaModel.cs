namespace OzyParkAdmin.Components.Admin.Ventas.Models;

/// <summary>
/// El modelo para buscar las ventas.
/// </summary>
public sealed class SearchVentaModel
{
    /// <summary>
    /// La fecha a buscar.
    /// </summary>
    public DateTime? Fecha { get; set; } = DateToSearch;

    /// <summary>
    /// El número de la orden.
    /// </summary>
    public string? NumeroOrden { get; set; }

    /// <summary>
    /// El número de venta.
    /// </summary>
    public string? VentaId { get; set; }

    /// <summary>
    /// El número del ticket.
    /// </summary>
    public string? TicketId { get; set; }

    /// <summary>
    /// La dirección de correo electrónico.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// El número de teléfono.
    /// </summary>
    public string? Telefono { get; set; }

    /// <summary>
    /// El nombre del cliente.
    /// </summary>
    public string? Nombres { get; set; }

    /// <summary>
    /// Los apellidos del cliente.
    /// </summary>
    public string? Apellidos { get; set; }

    internal static DateTime DateToSearch => DateTime.Today.AddDays(-7);
}