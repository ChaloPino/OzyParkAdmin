namespace OzyParkAdmin.Domain.Tickets;

/// <summary>
/// El repositorio de <see cref="ITicketRepository"/>.
/// </summary>
public interface ITicketRepository
{
    /// <summary>
    /// Busca un ticket dado su id.
    /// </summary>
    /// <param name="ticketId">El id del ticket a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El ticket si es que existe.</returns>
    Task<Ticket?> FindByIdAsync(string ticketId, CancellationToken cancellationToken);
}