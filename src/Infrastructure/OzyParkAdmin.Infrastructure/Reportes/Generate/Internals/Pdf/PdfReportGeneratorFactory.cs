using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.Listed;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.MasterDetail;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.Paginated;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.Pivoted;
using System.Diagnostics;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf;
internal static class PdfReportGeneratorFactory
{
    public static ITypedPdfReportGenerator Create(ReportType type) =>
        type switch
        {
            ReportType.Listed => new ListedPdfReportGenerator(),
            ReportType.MasterDetail => new MasterDetailPdfReportGenerator(),
            ReportType.Paginated => new PaginatedPdfReportGenerator(),
            ReportType.Pivoted => new PivotedPdfReportGenerator(),
            ReportType.Chart => throw new NotSupportedException("Los reporte de tipo dashboard no tiene soporte para generarse en Pdf."),
            _ => throw new UnreachableException(),
        };
}
