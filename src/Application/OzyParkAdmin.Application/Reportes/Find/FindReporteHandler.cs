using MassTransit.Mediator;
using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Reportes.Find;

/// <summary>
/// El manejador de <see cref="FindReporte"/>.
/// </summary>
public sealed class FindReporteHandler : MediatorRequestHandler<FindReporte, ResultOf<Report>>
{
    private readonly IReportRepository _repository;
    private readonly ILogger<FindReporteHandler> _logger;

    /// <summary>
    /// Crea una nueva instancia de <see cref="FindReporteHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IReportRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public FindReporteHandler(IReportRepository repository, ILogger<FindReporteHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(logger);
        _repository = repository;
        _logger = logger;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Report>> Handle(FindReporte request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request);
            return await _repository.FindReportByAkaAsync(request.Aka, request.User.GetRoles(), cancellationToken);
        }
        catch (Exception ex)
        {
            Guid ticket = Guid.NewGuid();
            _logger.LogError(ex, "Error al conseguir el reporte {ReportAka}. Ticket asociado: {TicketId}", request.Aka, ticket);
            return new Unknown(ticket, [ex.InnerException is not null ? ex.InnerException.Message : ex.Message]);
        }
    }
}
