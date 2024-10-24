using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate;

/// <summary>
/// Implementación de <see cref="IReportRepository"/>.
/// </summary>
public sealed class ReportGenerator : IReportGenerator, IInfrastructure
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
    public async Task<ResultOf<ReportGenerated>> GenerateHtmlReportAsync(string aka, ReportFilter filter, ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var result = await _repository.FindReportByAkaAsync(aka, user.GetRoles(), cancellationToken);
        return result.Bind(
            onSuccess: report => GenerateHtmlReport(report, filter, user),
            onFailure: failure => failure);
    }

    private ResultOf<ReportGenerated> GenerateHtmlReport(Report report, ReportFilter filter, ClaimsPrincipal user)
    {
        IFormatReportGenerator reportExecutor = _reportExecutorProvider.GetGenerator(ActionType.Html);
        IFormattedReport formattedReport = reportExecutor.GenerateReport(report, filter, user);
        return formattedReport.Generate();
    }

    /// <inheritdoc/>
    public async Task<ResultOf<ReportGenerated>> GenerateOhterFormatReportAsync(string aka, ActionType format, ReportFilter filter, ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var result = await _repository.FindReportByAkaAsync(aka, user.GetRoles(), cancellationToken);
        return result.Match(
            onSuccess: report => GenerateOhterFormatReport(report, format, filter, user),
            onFailure: failure => failure);
    }

    private ResultOf<ReportGenerated> GenerateOhterFormatReport(Report report, ActionType format, ReportFilter filter, ClaimsPrincipal user)
    {
        IFormatReportGenerator reportExecutor = _reportExecutorProvider.GetGenerator(format);
        IFormattedReport formattedReport = reportExecutor.GenerateReport(report, filter, user);
        return formattedReport.Generate();
    }
}
