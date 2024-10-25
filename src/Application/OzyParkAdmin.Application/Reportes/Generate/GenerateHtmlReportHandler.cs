using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Reportes.Generate;

/// <summary>
/// El manejador de <see cref="GenerateHtmlReport"/>.
/// </summary>
public sealed class GenerateHtmlReportHandler : BaseQueryHandler<GenerateHtmlReport, ReportGenerated>
{
    private readonly IReportGenerator _reportExecutor;

    /// <summary>
    /// Crea una nueva instancia de <see cref="GenerateHtmlReportHandler"/>.
    /// </summary>
    /// <param name="reportGenerator">El <see cref="IReportGenerator"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public GenerateHtmlReportHandler(IReportGenerator reportGenerator, ILogger<GenerateHtmlReportHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(reportGenerator);
        ArgumentNullException.ThrowIfNull(logger);
        _reportExecutor = reportGenerator;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<ReportGenerated>> ExecuteAsync(GenerateHtmlReport query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return  await _reportExecutor.GenerateHtmlReportAsync(query.Aka, query.Filter, query.User, cancellationToken);
    }
}
