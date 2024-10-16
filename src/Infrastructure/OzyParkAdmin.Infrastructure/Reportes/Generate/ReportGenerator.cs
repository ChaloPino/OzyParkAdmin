using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate;

/// <summary>
/// Implementación de <see cref="IReportRepository"/>.
/// </summary>
public sealed class ReportGenerator : IReportGenerator
{
    private readonly IReportRepository _repository;
    private readonly IFormatReportGeneratorProvider _reportExecutorProvider;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ReportGenerator"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IReportRepository"/>.</param>
    /// <param name="reportExecutorProvider">El <see cref="IFormatReportGeneratorProvider"/></param>
    public ReportGenerator(IReportRepository repository, IFormatReportGeneratorProvider reportExecutorProvider)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
        _reportExecutorProvider = reportExecutorProvider;
    }

    /// <inheritdoc/>
    public async Task<ReportResult> GenerateHtmlReportAsync(string aka, ReportFilter filter, ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var result = await _repository.FindReportByAkaAsync(aka, user.GetRoles(), cancellationToken);
        return result.Match(
            onSuccess: report => GenerateHtmlReport(report, filter, user),
            onFailure: failure => failure);
    }

    private ReportResult GenerateHtmlReport(Report report, ReportFilter filter, ClaimsPrincipal user)
    {
        IFormatReportGenerator reportExecutor = _reportExecutorProvider.GetGenerator(ActionType.Html);
        IFormattedReport formattedReport = reportExecutor.GenerateReport(report, filter, user);
        return formattedReport.Generate();
    }

    /// <inheritdoc/>
    public async Task<ReportResult> GenerateOhterFormatReportAsync(string aka, ActionType format, ReportFilter filter, ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var result = await _repository.FindReportByAkaAsync(aka, user.GetRoles(), cancellationToken);
        return result.Match(
            onSuccess: report => GenerateOhterFormatReport(report, format, filter, user),
            onFailure: failure => failure);
    }

    private ReportResult GenerateOhterFormatReport(Report report, ActionType format, ReportFilter filter, ClaimsPrincipal user)
    {
        IFormatReportGenerator reportExecutor = _reportExecutorProvider.GetGenerator(format);
        IFormattedReport formattedReport = reportExecutor.GenerateReport(report, filter, user);
        return formattedReport.Generate();
    }
}
