using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Plantillas;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Tickets;

namespace OzyParkAdmin.Application.Tickets.Download;

/// <summary>
/// El manejador de <see cref="DownloadTicket"/>.
/// </summary>
public sealed class DownloadTicketHandler : CommandHandler<DownloadTicket, DownloadedTicket>
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
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public DownloadTicketHandler(
        IOzyParkAdminContext context,
        TicketManager ticketManager,
        IDocumentGenerator documentGenerator,
        IClientIpService clientIpService,
        ILogger<DownloadTicketHandler> logger)
        : base(logger)
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
    protected override async Task<ResultOf<DownloadedTicket>> ExecuteAsync(DownloadTicket command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        ResultOf<Ticket> result = await _ticketManager.ReprintTicketAsync(
            command.TicketId,
            command.User.ToInfo(),
            _clientIpService.GetIpClient(),
            cancellationToken);

        return await result.BindAsync(
            onSuccess: (ticket, token) => SaveAndGenerateDocumentAsync(ticket, command, token),
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<DownloadedTicket>> SaveAndGenerateDocumentAsync(Ticket ticket, DownloadTicket request, CancellationToken cancellationToken)
    {
        _context.Update(ticket);
        await _context.SaveChangesAsync(cancellationToken);
        var result = await GenerateDocumentAsync(ticket, request, cancellationToken);
        return await result.BindAsync(
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

        return result.Bind(
            onSuccess: (content) => CreateDownloadedTicket(ticket, content),
            onFailure: failure => failure);
    }

    private static ResultOf<DownloadedTicket> CreateDownloadedTicket(Ticket ticket, byte[] content) =>
        new DownloadedTicket(ticket.Id, Convert.ToBase64String(content));
}
