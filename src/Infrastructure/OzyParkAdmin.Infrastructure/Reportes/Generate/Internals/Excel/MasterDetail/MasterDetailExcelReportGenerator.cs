using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Domain.Reportes.MasterDetails;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.MasterDetail;
internal sealed class MasterDetailExcelReportGenerator : TypedExcelReportGenerator<MasterDetailReport>
{
    public MasterDetailExcelReportGenerator()
        : base(new MasterDetailExcelBuilder())
    {
    }

    protected override byte[]? CreateExcel(MasterDetailReport report, ReportFilter filter, ExcelReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        return Builder.Build(report, filter, template, dataSet, user);
    }
}
