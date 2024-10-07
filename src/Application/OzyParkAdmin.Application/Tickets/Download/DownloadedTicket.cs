namespace OzyParkAdmin.Application.Tickets.Download;

/// <summary>
/// Contiene el documento del ticket en base64.
/// </summary>
/// <param name="TicketId">El id del ticket.</param>
/// <param name="Base64">El contenido del ticket en base64.</param>
public sealed record DownloadedTicket(string TicketId, string Base64);
