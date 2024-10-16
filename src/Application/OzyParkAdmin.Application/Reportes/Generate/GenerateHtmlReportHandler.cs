using MassTransit.Mediator;

namespace OzyParkAdmin.Application.Reportes.Generate;

/// <summary>
/// El manejador de <see cref="GenerateHtmlReport"/>.
/// </summary>
public sealed class GenerateHtmlReportHandler : MediatorRequestHandler<GenerateHtmlReport, ReportResult>
{
    private readonly IReportGenerator _reportExecutor;

    /// <summary>
    /// Crea una nueva instancia de <see cref="GenerateHtmlReportHandler"/>.
    /// </summary>
    /// <param name="reportGenerator">El <see cref="IReportGenerator"/>.</param>
    public GenerateHtmlReportHandler(IReportGenerator reportGenerator)
    {
        ArgumentNullException.ThrowIfNull(reportGenerator);
        _reportExecutor = reportGenerator;
    }

    /// <inheritdoc/>
    protected override async Task<ReportResult> Handle(GenerateHtmlReport request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _reportExecutor.GenerateHtmlReportAsync(request.Aka, request.Filter, request.User, cancellationToken);
    }
}
