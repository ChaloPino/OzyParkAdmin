using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Domain.Reportes.Listed;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Listed;
internal sealed class ListedExcelReportGenerator : TypedExcelReportGenerator<ListedReport>
{
    public ListedExcelReportGenerator() : base(new ExcelBuilder<ListedReport>())
    {
    }
    protected override byte[]? CreateExcel(ListedReport report, ReportFilter filter, ExcelReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        DataTable dataTable = dataSet.Tables[0];

        List<DataRow> rows = dataTable.Sort(report.ServerSide && !report.IsSortingInDatabase && filter.SortExpressions.HasExpressions, filter.SortExpressions);
        return Builder.Build(report, filter, template, dataTable, rows, user);
    }
}
