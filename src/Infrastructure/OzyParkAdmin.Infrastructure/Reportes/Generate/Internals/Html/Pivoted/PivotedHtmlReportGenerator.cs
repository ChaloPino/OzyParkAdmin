using NReco.PivotData;
using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Pivoted;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.Pivoted;
internal sealed class PivotedHtmlReportGenerator : TypedHtmlReportGenerator<PivotedReport>
{
    protected override (DataTable dataTable, long totalRecords) ExtractDataTable(PivotedReport report, DataSet dataSet, ReportFilter filter)
    {
        DataTable dataTable = dataSet.Tables[0];
        long totalRecords = dataTable.Rows.Count;
        return (dataTable, totalRecords);
    }

    protected override HtmlFormattedReport PopulateData(PivotedReport report, ReportFilter filter, DataTable dataTable, long totalRecords, ClaimsPrincipal user)
    {
        List<PivotedMember> pivotedMembers = report.PivotedMembers.Where(member => member.IsAccessibleByUser(user)).OrderBy(p => p.PivotType).ThenBy(p => p.Order).ToList();

        IPivotTable pivotTable = PivotFactory.CreatePivotTable(pivotedMembers, dataTable);

        return PivotedHtmlFormattedReport.Create(pivotTable, report);
    }
}
