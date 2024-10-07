using MassTransit.Mediator;
using OzyParkAdmin.Domain.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Tickets.Download;

/// <summary>
/// Descarga el documento de un ticket.
/// </summary>
/// <param name="TicketId">El id del ticket a descargar.</param>
/// <param name="VentaId">El id de la venta.</param>
/// <param name="User">El usuario que realiza la consulta.</param>
public sealed record DownloadTicket(string TicketId, string VentaId, ClaimsPrincipal User) : Request<ResultOf<DownloadedTicket>>;
