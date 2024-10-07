using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Domain.Tickets;

/// <summary>
/// El ticket reimpreso.
/// </summary>
public sealed class TicketReimpreso
{
    /// <summary>
    /// El id del ticket reimpreso.
    /// </summary>
    public string TicketId { get; private init; } = string.Empty;

    /// <summary>
    /// La fecha de reimpresión.
    /// </summary>
    public DateTime FechaReimpresion { get; private init; }

    /// <summary>
    /// El id de usuario que reimprimió.
    /// </summary>
    public Guid UserId { get; private init; }

    /// <summary>
    /// El nombre del usuario que reimprimió.
    /// </summary>
    public string UserName { get; private init; } = string.Empty;

    /// <summary>
    /// La ip desde donde se realizó la reimpresión.
    /// </summary>
    public string Ip { get; private init; } = string.Empty;

    internal static TicketReimpreso Create(Ticket ticket, Usuario usuario, string ip, DateTime? fechaReimpresion = null) =>
        new()
        {
            TicketId = ticket.Id,
            FechaReimpresion = fechaReimpresion ?? DateTime.Now,
            UserId = usuario.Id,
            UserName = usuario.UserName,
            Ip = ip,
        };
}
