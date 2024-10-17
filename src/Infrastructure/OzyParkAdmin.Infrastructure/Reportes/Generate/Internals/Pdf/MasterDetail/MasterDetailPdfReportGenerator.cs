using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.MasterDetails;
using OzyParkAdmin.Domain.Reportes.Pdf;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.MasterDetail;
internal sealed class MasterDetailPdfReportGenerator : TypedPdfReportGenerator<MasterDetailReport>
{
    public MasterDetailPdfReportGenerator()
        : base(new MasterDetailPdfBuilder())
    {
    }

    protected override byte[]? CreateExcel(MasterDetailReport report, ReportFilter filter, PdfReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        return Builder.Build(report, filter, template, dataSet, user);
    }
}
