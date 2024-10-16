using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Listed;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.Listed;
internal sealed class ListedHtmlReportGenerator : TypedHtmlReportGenerator<ListedReport>
{
    protected override (DataTable dataTable, long totalRecords) ExtractDataTable(ListedReport report, DataSet dataSet, ReportFilter filter)
    {
        DataTable dataTable = dataSet.Tables[0];
        long recordTotals = dataTable.Rows.Count;
        return (dataTable, recordTotals);
    }

    protected override HtmlFormattedReport PopulateData(ListedReport report, ReportFilter filter, DataTable dataTable, long totalRecords, ClaimsPrincipal user)
    {
        List<Column> columns = [..report.Columns
            .Where(column => column.IsAccessibleByUser(user))
            .OrderBy(c => c.Order)];

        List<DataRow> rowCollection = LocalSorting(report, filter, dataTable);

        return ListedHtmlFormattedReport.Create(rowCollection, columns);
    }

    private static List<DataRow> LocalSorting(ListedReport report, ReportFilter filter, DataTable dataTable) =>
        dataTable.Sort(report.ServerSide && !report.IsSortingInDatabase && report.CanSort, filter.SortExpressions);
}
