using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.MasterDetails;
using OzyParkAdmin.Domain.Reportes.Pdf;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.MasterDetail;
internal sealed class MasterDetailPdfBuilder : PdfBuilderBase<MasterDetailReport>
{
    protected override string CreateHtml(MasterDetailReport report, ReportFilter reportFilter, PdfReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        StringWriter writer = new();

        MasterDetailHtmlWriter htmlWriter = new(writer)
        {
            OnPreWrite = (xmlWriter) => WriteHtmlTitle(xmlWriter, report, template)
        };

        htmlWriter.Write(report, dataSet, user);
        return writer.ToString();
    }
}