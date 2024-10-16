using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html;

internal abstract class TypedHtmlReportGenerator<TReport> : ITypedHtmlReportGenerator
    where TReport : Report
{
    public HtmlFormattedReport FormatToHtml(TReport report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user)
    {
        (DataTable dataTable, long recordTotals) = ExtractDataTable(report, dataSet, filter);
        return PopulateData(report, filter, dataTable, recordTotals, user);
    }
    protected abstract (DataTable dataTable, long totalRecords) ExtractDataTable(TReport report, DataSet dataSet, ReportFilter filter);
    protected abstract HtmlFormattedReport PopulateData(TReport report, ReportFilter filter, DataTable dataTable, long totalRecords, ClaimsPrincipal user);

    HtmlFormattedReport ITypedHtmlReportGenerator.FormatToHtml(Report report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user) =>
        FormatToHtml((TReport)report, filter, dataSet, user);
}