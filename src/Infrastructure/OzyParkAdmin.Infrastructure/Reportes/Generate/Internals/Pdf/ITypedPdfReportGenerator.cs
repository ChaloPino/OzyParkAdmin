using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf;
internal interface ITypedPdfReportGenerator
{
    PdfFormattedReport FormatToPdf(Report report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user);
}
