using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Domain.Reportes.Listed;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Paginated;
internal sealed class PaginatedExcelReportGenerator : TypedExcelReportGenerator<PaginatedReport>
{
    public PaginatedExcelReportGenerator()
        : base(new ExcelBuilder<PaginatedReport>())
    {
    }

    protected override byte[]? CreateExcel(PaginatedReport report, ReportFilter filter, ExcelReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        DataTable dataTable = ExtractDataTable(report, dataSet);
        List<DataRow> rows = dataTable.Sort(report.ServerSide && !report.IsSortingInDatabase && filter.SortExpressions.HasExpressions, filter.SortExpressions);
        return Builder.Build(report, filter, template, dataTable, rows, user);
    }

    private static DataTable ExtractDataTable(PaginatedReport report, DataSet dataSet)
    {
        return report switch
        {
            { ServerSide: true, TotalRecordsDataSource: null, DataResultIndex: null } => throw new InvalidOperationException("Si el reporte se ejecuta en el lado del servidor, debe asociarse la 'Fuente de datos para totales' o el 'Íncide de resultado de datos'."),
            { ServerSide: true, DataResultIndex: not null, TotalRecordsResultIndex: not null } => dataSet.Tables[report.DataResultIndex.Value],
            _ => dataSet.Tables[0],
        };
    }
}
