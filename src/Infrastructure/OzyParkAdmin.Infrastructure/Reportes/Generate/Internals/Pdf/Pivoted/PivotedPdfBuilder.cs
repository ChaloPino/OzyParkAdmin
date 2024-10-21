using NReco.PivotData;
using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Filters;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Pdf;
using OzyParkAdmin.Domain.Reportes.Pivoted;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;
using System.Data;
using System.Globalization;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.Pivoted;
internal sealed class PivotedPdfBuilder : PdfBuilderBase<PivotedReport>
{
    protected override string CreateHtml(PivotedReport report, ReportFilter reportFilter, PdfReportTemplate template, DataTable dataTable, ClaimsPrincipal user)
    {
        List<Filter> filters = [.. report.Filters.OrderBy(f => f.Order)];
        List<PivotedMember> pivotedMembers = [.. report.PivotedMembers.Where(member => member.IsAccessibleByUser(user)).OrderBy(p => p.PivotType).ThenBy(p => p.Order)];

        IPivotTable pivotTable = PivotFactory.CreatePivotTable(pivotedMembers, dataTable);

        List<PivotedMember> valuePivotedMembers = pivotedMembers.Where(pm => pm.PivotType == PivotType.Value).ToList();

        StringWriter stringWriter = new();

        PivotTableHtmlWriter htmlWriter = new(stringWriter)
        {
            OnPreWrite = (writer) => WriteHtmlTitleAndFilter(writer, report, filters, reportFilter, template),
            OnPostWrite = WriteEndHtml,
            TotalsColumn = report.IncludeTotalsColumn,
            TotalsRow = report.IncludeTotalsRow,
            GrandTotal = report.IncludeGrandTotal,
            SubtotalColumns = pivotedMembers.Exists(pc => pc.PivotType == PivotType.Column && pc.ShowTotal is not null && pc.ShowTotal.Value),
            SubtotalRows = pivotedMembers.Exists(pc => pc.PivotType == PivotType.Row && pc.ShowTotal is not null && pc.ShowTotal.Value),
            SubtotalDimensions = pivotedMembers.Where(pm => pm.ShowTotal is not null && pm.ShowTotal.Value).Select(pm => pm.GetFullName()).ToArray(),
            RenderTheadTbody = template.RepeatHeaderInEachPage,
            FormatKey = (key, dim) =>
            {
                PivotedMember? member = pivotedMembers.Find(pm => pm.GetFullName() == dim);

                return member is not null && !string.IsNullOrEmpty(member.Format)
                    ? string.Format(CultureInfo.CurrentCulture, string.Concat("{0:", member.Format, "}"), key)
                    : Convert.ToString(key, CultureInfo.CurrentCulture);
            },
            FormatValue = (aggregator, index) =>
            {
                PivotedMember valuePivotedMember = valuePivotedMembers[index];
                return aggregator switch
                {
                    null => null,
                    _ => !string.IsNullOrEmpty(valuePivotedMember.Format)
                            ? string.Format(CultureInfo.CurrentCulture, string.Concat("{0:", valuePivotedMember.Format, "}"), aggregator.Value)
                            : Convert.ToString(aggregator.Value, CultureInfo.CurrentCulture),
                };
            },
            FormatMeasureHeader = (aggregator, index) => aggregator.FormatAggregator(valuePivotedMembers, index),
            OnApplyValueCellStyle = (valueCell) =>
            {
                if (!valueCell.IsSubTotal)
                {
                    PivotedMember valuePivotedMember = valuePivotedMembers[valueCell.AggregatorIndex];
                    if (valueCell.Aggregator is not null && valuePivotedMember.HasConditionalStyle)
                    {
                        if (valuePivotedMember.SuccessStyle is not null && valuePivotedMember.EvaluateSuccessCondition(valuePivotedMember.Column.Type, valueCell.Aggregator.Value))
                        {
                            if (valuePivotedMember.TryGenerateSuccessCssStyle(out string? successClass))
                            {
                                valueCell.AddCssClass(successClass);
                            }
                        }
                        else if (valuePivotedMember.WarningStyle is not null && valuePivotedMember.EvaluateWarningCondition(valuePivotedMember.Column.Type, valueCell.Aggregator.Value))
                        {
                            if (valuePivotedMember.TryGenerateWarningCssStyle(out string? warningClass))
                            {
                                valueCell.AddCssClass(warningClass);
                            }
                        }
                        else
                        {
                            if (valuePivotedMember.TryGenerateErrorCssStyle(out string? errorClass))
                            {
                                valueCell.AddCssClass(errorClass);
                            }
                        }
                    }
                }
            }
        };

        htmlWriter.OnPreWrite = (writer) => WriteHtmlTitleAndFilter(writer, report, filters, reportFilter, template);
        htmlWriter.OnPostWrite = WriteEndHtml;

        htmlWriter.Write(pivotTable);

        return stringWriter.ToString();
    }
}