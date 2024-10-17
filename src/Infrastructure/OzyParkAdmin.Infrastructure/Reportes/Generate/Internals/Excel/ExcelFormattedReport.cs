using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel;
internal sealed class ExcelFormattedReport : IFormattedReport
{
    private ExcelFormattedReport(ReportType reportType)
    {
        Type = reportType;
    }

    public ActionType Format => ActionType.Excel;

    public ReportType Type { get; }

    public string? FileName { get; set; }

    public byte[]? Content { get; set; }

    public static ExcelFormattedReport Create(ReportType reportType, string? fileName, byte[]? content) =>
        new(reportType)
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
            MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        };
    }
}
