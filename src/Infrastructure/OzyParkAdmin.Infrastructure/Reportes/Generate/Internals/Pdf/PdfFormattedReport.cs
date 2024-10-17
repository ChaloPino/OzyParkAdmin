using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf;

internal sealed class PdfFormattedReport : IFormattedReport
{
    private PdfFormattedReport(ReportType reportType)
    {
        Type = reportType;
    }
    public ActionType Format => ActionType.Pdf;

    public ReportType Type { get; }

    public string? FileName { get; set; }

    public byte[]? Content { get; set; }

    public static PdfFormattedReport Create(ReportType reportType, string? fileName, byte[]? content) =>
        new PdfFormattedReport(reportType)
        {
            FileName = fileName,
            Content = content
        };

    public ReportGenerated Generate()
    {
        return new ReportGenerated
        {
            Format = Format,
            Type = Type,
            FileName = FileName,
            Content = Content,
            MimeType = "application/pdf",
        };
    }
}