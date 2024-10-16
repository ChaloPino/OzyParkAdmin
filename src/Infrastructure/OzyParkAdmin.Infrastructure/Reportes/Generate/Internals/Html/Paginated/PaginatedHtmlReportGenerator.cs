using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Listed;
using System.Data;
using System.Globalization;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.Paginated;
internal sealed class PaginatedHtmlReportGenerator : TypedHtmlReportGenerator<PaginatedReport>
{
    protected override (DataTable dataTable, long totalRecords) ExtractDataTable(PaginatedReport report, DataSet dataSet, ReportFilter filter)
    {
        DataTable dataTable;
        long totalRecords;

        if (report.ServerSide && report.TotalRecordsDataSource == null && report.DataResultIndex == null)
        {
            throw new InvalidOperationException("Si el reporte se ejecuta en el lado del servidor, debe asociarse la 'Fuente de datos para totales' o el 'Íncide de resultado de datos'.");
        }

        if (report.ServerSide && report.TotalRecordsDataSource != null)
        {
            DataSet totalDataSet = DataSetExecutor.ExecuteDataSet(report.TotalRecordsDataSource, report, filter);
            dataTable = dataSet.Tables[0];
            totalRecords = Convert.ToInt64(totalDataSet.Tables[0].Rows[0], CultureInfo.InvariantCulture);
        }
        else if (report.ServerSide && report.DataResultIndex.HasValue && report.TotalRecordsResultIndex.HasValue)
        {
            dataTable = dataSet.Tables[report.DataResultIndex.Value];
            totalRecords = Convert.ToInt64(dataSet.Tables[report.TotalRecordsResultIndex.Value].Rows[0], CultureInfo.InvariantCulture);
        }
        else
        {
            dataTable = dataSet.Tables[0];
            totalRecords = dataTable.Rows.Count;
        }
        return (dataTable, totalRecords);
    }

    protected override HtmlFormattedReport PopulateData(PaginatedReport report, ReportFilter filter, DataTable dataTable, long totalRecords, ClaimsPrincipal user)
    {
        List<Column>? columns = [..report.Columns
               .Where(column => column.IsAccessibleByUser(user))
               .OrderBy(c => c.Order)];

        List<DataRow> rowCollection = LocalPaginationAndSorting(report, filter, dataTable);

        return PaginatedHtmlFormattedReport.Create(rowCollection, columns, totalRecords);
    }

    private static List<DataRow> LocalPaginationAndSorting(PaginatedReport report, ReportFilter reportFilter, DataTable dataTable)
    {
        List<DataRow> rowCollection = [..dataTable.AsEnumerable()];

        if (report.ServerSide && !report.IsPaginationInDatabase)
        {
            rowCollection = [..dataTable.Select()
                .Skip(reportFilter.Page * reportFilter.PageSize)
                .Take(reportFilter.PageSize)];
        }

        return rowCollection.Sort(report.ServerSide && !report.IsSortingInDatabase && report.CanSort, reportFilter.SortExpressions);
    }
}
