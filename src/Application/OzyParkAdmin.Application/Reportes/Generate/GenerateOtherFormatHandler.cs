using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Reportes.Generate;

/// <summary>
/// El manejador de <see cref="GenerateOtherFormat"/>.
/// </summary>
public sealed class GenerateOtherFormatHandler : BaseQueryHandler<GenerateOtherFormat, ReportGenerated>
{
    private readonly IReportGenerator _reportGenerator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="GenerateOtherFormatHandler"/>.
    /// </summary>
    /// <param name="reportGenerator">El <see cref="IReportGenerator"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public GenerateOtherFormatHandler(IReportGenerator reportGenerator, ILogger<GenerateOtherFormatHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(reportGenerator);
        _reportGenerator = reportGenerator;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<ReportGenerated>> ExecuteAsync(GenerateOtherFormat query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _reportGenerator.GenerateOhterFormatReportAsync(query.Aka, query.Format, query.Filter, query.User, cancellationToken);
    }
}
