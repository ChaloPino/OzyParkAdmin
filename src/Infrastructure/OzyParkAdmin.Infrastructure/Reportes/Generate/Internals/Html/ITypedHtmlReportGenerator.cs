using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html;

internal interface ITypedHtmlReportGenerator
{
    HtmlFormattedReport FormatToHtml(Report report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user);
}
