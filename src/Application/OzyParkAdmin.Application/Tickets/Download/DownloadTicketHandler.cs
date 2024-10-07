using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Plantillas;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Tickets;

namespace OzyParkAdmin.Application.Tickets.Download;

/// <summary>
/// El manejador de <see cref="DownloadTicket"/>.
/// </summary>
public sealed class DownloadTicketHandler : MediatorRequestHandler<DownloadTicket, ResultOf<DownloadedTicket>>
{
    private readonly IOzyParkAdminContext _context;
    private readonly TicketManager _ticketManager;
    private readonly IDocumentGenerator _documentGenerator;
    private readonly IClientIpService _clientIpService;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DownloadTicketHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="ticketManager">El <see cref="TicketManager"/>.</param>
    /// <param name="documentGenerator">El <see cref="IDocumentGenerator"/>.</param>
    /// <param name="clientIpService">El <see cref="IClientIpService"/>.</param>
    public DownloadTicketHandler(IOzyParkAdminContext context, TicketManager ticketManager, IDocumentGenerator documentGenerator, IClientIpService clientIpService)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(ticketManager);
        ArgumentNullException.ThrowIfNull(documentGenerator);
        ArgumentNullException.ThrowIfNull(clientIpService);
        _context = context;
        _ticketManager = ticketManager;
        _documentGenerator = documentGenerator;
        _clientIpService = clientIpService;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<DownloadedTicket>> Handle(DownloadTicket request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        ResultOf<Ticket> result = await _ticketManager.ReprintTicketAsync(
            request.TicketId,
            request.User.ToInfo(),
            _clientIpService.GetIpClient(),
            cancellationToken);

        return await result.MatchResultOfAsync(
            onSuccess: (ticket, token) =>  SaveAndGenerateDocumentAsync(ticket, request, token),
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<DownloadedTicket>> SaveAndGenerateDocumentAsync(Ticket ticket, DownloadTicket request, CancellationToken cancellationToken)
    {
        _context.Update(ticket);
        await _context.SaveChangesAsync(cancellationToken);
        var result = await GenerateDocumentAsync(ticket, request, cancellationToken);
        return await result.MatchResultOfAsync(
            onSuccess: SaveGenerationAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<DownloadedTicket>> SaveGenerationAsync(DownloadedTicket downloadedTicket, CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
        return downloadedTicket;
    }

    private async Task<ResultOf<DownloadedTicket>> GenerateDocumentAsync(Ticket ticket, DownloadTicket request, CancellationToken cancellationToken)
    {
        ResultOf<byte[]> result = await _documentGenerator.GenerateTicketDocumentAsync(ticket, request.VentaId, cancellationToken);

        return result.MatchResultOf(
            onSuccess: (content) => CreateDownloadedTicket(ticket, content),
            onFailure: failure => failure);
    }

    private static ResultOf<DownloadedTicket> CreateDownloadedTicket(Ticket ticket, byte[] content) =>
        new DownloadedTicket(ticket.Id, Convert.ToBase64String(content));
}
