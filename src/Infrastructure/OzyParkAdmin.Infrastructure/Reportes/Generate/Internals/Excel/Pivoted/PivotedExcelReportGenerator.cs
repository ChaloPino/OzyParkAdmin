using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Domain.Reportes.Pivoted;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Pivoted;
internal sealed class PivotedExcelReportGenerator : TypedExcelReportGenerator<PivotedReport>
{
    public PivotedExcelReportGenerator()
        : base(new PivotedExcelBuilder())
    {
    }

    protected override byte[]? CreateExcel(PivotedReport report, ReportFilter filter, ExcelReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        DataTable dataTable = dataSet.Tables[0];
        return Builder.Build(report, filter, template, dataTable, dataTable.AsEnumerable(), user);
    }
}
