using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf;
internal sealed class PdfReportGenerator : FormatReportGenerator
{
    protected override IFormattedReport GenerateReport(Report report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user)
    {
        ITypedPdfReportGenerator generator = PdfReportGeneratorFactory.Create(report.Type);
        return generator.FormatToPdf(report, filter, dataSet, user);
    }
}
