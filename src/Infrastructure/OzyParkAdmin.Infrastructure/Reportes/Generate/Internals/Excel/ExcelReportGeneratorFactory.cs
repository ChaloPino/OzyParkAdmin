using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Listed;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.MasterDetail;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Paginated;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Pivoted;
using System.Diagnostics;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel;
internal static class ExcelReportGeneratorFactory
{
    public static ITypedExcelReportGenerator Create(ReportType type)
    {
        return type switch
        {
            ReportType.Listed => new ListedExcelReportGenerator(),
            ReportType.MasterDetail => new MasterDetailExcelReportGenerator(),
            ReportType.Paginated => new PaginatedExcelReportGenerator(),
            ReportType.Pivoted => new PivotedExcelReportGenerator(),
            ReportType.Chart => throw new NotSupportedException("Los reporte de tipo dashboard no tiene soporte para generarse en Excel."),
            _ => throw new UnreachableException(),
        };
    }
}
