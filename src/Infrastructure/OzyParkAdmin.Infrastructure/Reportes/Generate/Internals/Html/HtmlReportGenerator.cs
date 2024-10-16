using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html;
internal sealed class HtmlReportGenerator : FormatReportGenerator
{
    protected override IFormattedReport GenerateReport(Report report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user)
    {
        ITypedHtmlReportGenerator generator = HtmlReportGeneratorFactory.Create(report.Type);
        return generator.FormatToHtml(report, filter, dataSet, user);
    }
}
