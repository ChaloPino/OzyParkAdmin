using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Pdf;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf;
internal abstract class TypedPdfReportGenerator<TReport> : ITypedPdfReportGenerator
    where TReport : Report
{
    protected TypedPdfReportGenerator(PdfBuilderBase<TReport> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        Builder = builder;
    }

    public PdfBuilderBase<TReport> Builder { get; }

    public PdfFormattedReport FormatToPdf(Report report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user)
    {
        byte[]? content = null;
        string? fileName = null;

        PdfReportTemplate? template = report.Templates.OfType<PdfReportTemplate>().FirstOrDefault();

        if (template is not null)
        {
            (content, fileName) = CreatePdf(report, filter, template, dataSet, user);
        }

        return PdfFormattedReport.Create(report.Type, fileName, content);
    }

    private (byte[]? Content, string? FileName) CreatePdf(Report report, ReportFilter filter, PdfReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        byte[]? content = CreateExcel((TReport)report, filter, template, dataSet, user);
        string? fileName = null;

        if (content is not null)
        {
            fileName = ResolveFileName(report, filter, template);
        }

        return (content, fileName);
    }

    protected abstract byte[]? CreateExcel(TReport report, ReportFilter filter, PdfReportTemplate template, DataSet dataSet, ClaimsPrincipal user);

    private static string ResolveFileName(Report report, ReportFilter filter, PdfReportTemplate? template) =>
        $"{FileNameUtils.ResolveFileName(report, filter, template)}.pdf";
}
