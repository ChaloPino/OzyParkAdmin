using MassTransit.Mediator;

namespace OzyParkAdmin.Application.Reportes.Generate;

/// <summary>
/// El manejador de <see cref="GenerateOtherFormat"/>.
/// </summary>
public sealed class GenerateOtherFormatHandler : MediatorRequestHandler<GenerateOtherFormat, ReportResult>
{
    private readonly IReportGenerator _reportGenerator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="GenerateOtherFormatHandler"/>.
    /// </summary>
    /// <param name="reportGenerator">El <see cref="IReportGenerator"/>.</param>
    public GenerateOtherFormatHandler(IReportGenerator reportGenerator)
    {
        ArgumentNullException.ThrowIfNull(reportGenerator);
        _reportGenerator = reportGenerator;
    }

    /// <inheritdoc/>
    protected override async Task<ReportResult> Handle(GenerateOtherFormat request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _reportGenerator.GenerateOhterFormatReportAsync(request.Aka, request.Format, request.Filter, request.User, cancellationToken);
    }
}
