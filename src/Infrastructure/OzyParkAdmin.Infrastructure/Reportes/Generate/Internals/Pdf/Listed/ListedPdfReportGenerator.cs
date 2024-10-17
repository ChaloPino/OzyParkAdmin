using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Listed;
using OzyParkAdmin.Domain.Reportes.Pdf;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.Listed;
internal sealed class ListedPdfReportGenerator : TypedPdfReportGenerator<ListedReport>
{
    public ListedPdfReportGenerator()
        : base(new PdfBuilder<ListedReport>())
    {
    }

    protected override byte[]? CreateExcel(ListedReport report, ReportFilter filter, PdfReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        DataTable dataTable = dataSet.Tables[0];
        return Builder.Build(report, filter, template, dataTable, user);
    }
}
