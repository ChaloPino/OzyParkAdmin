using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Excel;
using System.Data;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel;
internal abstract class TypedExcelReportGenerator<TReport> : ITypedExcelReportGenerator
    where TReport : Report
{
    protected TypedExcelReportGenerator(ExcelBuilderBase<TReport> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        Builder = builder;
    }

    protected ExcelBuilderBase<TReport> Builder { get; }

    public ExcelFormattedReport FormatToExcel(Report report, ReportFilter filter, DataSet dataSet, ClaimsPrincipal user)
    {
        byte[]? content = null;
        string? fileName = null;

        ExcelReportTemplate? template = report.Templates.OfType<ExcelReportTemplate>().FirstOrDefault();

        if (template is not null)
        {
            (content, fileName) = CreateExcel(report, filter, template, dataSet, user);
        }

        return ExcelFormattedReport.Create(report.Type, fileName, content);
    }

    private (byte[]? Content, string? FileName) CreateExcel(Report report, ReportFilter filter, ExcelReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        byte[]? content = CreateExcel((TReport)report, filter, template, dataSet, user);
        string? fileName = null;

        if (content is not null)
        {
            fileName = ResolveFileName(report, filter, template);
        }

        return (content, fileName);
    }

    protected abstract byte[]? CreateExcel(TReport report, ReportFilter filter, ExcelReportTemplate template, DataSet dataSet, ClaimsPrincipal user);

    private static string ResolveFileName(Report report, ReportFilter filter, ExcelReportTemplate? template) =>
        $"{FileNameUtils.ResolveFileName(report, filter, template)}.xlsx";
}
