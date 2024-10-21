using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Pdf;
using OzyParkAdmin.Domain.Reportes.Pivoted;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.Pivoted;
internal sealed class PivotedPdfReportGenerator : TypedPdfReportGenerator<PivotedReport>
{
    public PivotedPdfReportGenerator()
        : base(new PivotedPdfBuilder())
    {
    }

    protected override byte[]? CreateExcel(PivotedReport report, ReportFilter filter, PdfReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        DataTable dataTable = dataSet.Tables[0];
        return Builder.Build(report, filter, template, dataTable, user);
    }
}
