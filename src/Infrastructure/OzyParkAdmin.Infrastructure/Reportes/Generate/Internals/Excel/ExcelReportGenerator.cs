using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel;
internal sealed class ExcelReportGenerator : FormatReportGenerator
{
    protected override IFormattedReport GenerateReport(Report report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user)
    {
        ITypedExcelReportGenerator generator = ExcelReportGeneratorFactory.Create(report.Type);
        return generator.FormatToExcel(report, filter, dataSet, user);
    }
}
