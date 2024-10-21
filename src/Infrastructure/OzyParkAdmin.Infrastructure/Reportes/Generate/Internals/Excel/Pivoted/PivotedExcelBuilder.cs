using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using NReco.PivotData;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Domain.Reportes.Pivoted;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Utilities;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;
using System.Data;
using System.Security.Claims;
using OzyParkAdmin.Domain.Reportes.Filters;
using OzyParkAdmin.Application.Reportes;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Pivoted;
internal partial class PivotedExcelBuilder : ExcelBuilderBase<PivotedReport>
{
    public override byte[]? Build(PivotedReport report, ReportFilter reportFilter, ExcelReportTemplate template, DataTable dataTable, IEnumerable<DataRow> dataRows, ClaimsPrincipal user)
    {
        List<ExcelFilter> excelFilters = report.Filters
                                            .Where(f => f is not HiddenFilter)
                                            .OrderBy(f => f.Order)
                                            .Select(f => new ExcelFilter(f, f.GetText(reportFilter.GetFilter(f.Id)))).ToList();

        List<PivotedMember> pivotedMembers = [.. report.PivotedMembers.Where(member => member.IsAccessibleByUser(user)).OrderBy(member => member.PivotType).ThenBy(member => member.Order)];
        List<PivotedMember> valuePivotedMembers = pivotedMembers.Where(pm => pm.PivotType == PivotType.Value).ToList();

        List<ExcelColumn> excelColumns = pivotedMembers.ConvertAll(pm => new ExcelColumn(pm));

        List<ExcelColumn> rowColumns = (from ec in excelColumns
                                        join pm in pivotedMembers on ec.Id equals pm.Id
                                        where pm.PivotType == PivotType.Row
                                        select ec)
                                        .ToList();

        List<ExcelColumn> columnColumns = (from ec in excelColumns
                                           join pm in pivotedMembers on ec.Id equals pm.Id
                                           where pm.PivotType == PivotType.Column
                                           select ec)
                                           .ToList();

        List<ExcelColumn> valueColumns = (from ec in excelColumns
                                          join pm in pivotedMembers on ec.Id equals pm.Id
                                          where pm.PivotType == PivotType.Value
                                          select ec)
                                          .ToList();

        IPivotTable pivotTable = PivotFactory.CreatePivotTable(pivotedMembers, dataTable);

        SharedTable sharedTable = SharedTableHelper.CreateSharedTable(template, excelFilters);
        Style style = Helper.CreateStyle(template, excelFilters, excelColumns, rowColumns, columnColumns, valueColumns);
        List<Row> data = Helper.CreateRows(template, excelFilters, out int startRow);
        List<Column> cols = [];
        List<MergeCell> mergeCells = [];
        SheetDimension sheetDimension = new();

        PivotTableExcelWriter writer = new()
        {
            StartCellRow = startRow,
            TotalsColumn = report.IncludeTotalsColumn,
            TotalsRow = report.IncludeTotalsRow,
            GrandTotal = report.IncludeGrandTotal,
            SubtotalColumns = pivotedMembers.Exists(pc => pc.PivotType == PivotType.Column && pc.ShowTotal is not null && pc.ShowTotal.Value),
            SubtotalRows = pivotedMembers.Exists(pc => pc.PivotType == PivotType.Row && pc.ShowTotal is not null && pc.ShowTotal.Value),
            SubtotalDimensions = pivotedMembers.Where(pm => pm.ShowTotal is not null && pm.ShowTotal.Value).Select(pm => pm.GetFullName()).ToArray(),
            OnApplyRowHeaderStyle = (cell, dim, isSubtotal) =>
            {
                ExcelColumn? member = excelColumns.Find(pm => pm.Header == dim);

                if (member is not null)
                {
                    cell.StyleIndex = isSubtotal ? new UInt32Value((uint)member.TotalHeaderStyleId) : new UInt32Value((uint)member.HeaderStyleId);
                }
            },
            OnApplyColumnHeaderStyle = (cell, dim, isSubtotal) =>
            {
                ExcelColumn? member = excelColumns.Find(pm => pm.Header == dim);

                if (member is not null)
                {
                    cell.StyleIndex = isSubtotal ? new UInt32Value((uint)member.TotalHeaderStyleId) : new UInt32Value((uint)member.HeaderStyleId);
                }
            },
            OnApplyMeasureHeaderStyles = (cell, index) =>
            {
                ExcelColumn valuePivotedMember = valueColumns[index];
                cell.StyleIndex = new UInt32Value((uint)valuePivotedMember.HeaderStyleId);
            },
            OnApplyTotalHeaderStyles = (cell) => cell.StyleIndex = new UInt32Value(1U),
            OnApplyDimensionLabelStyles = (cell) => cell.StyleIndex = new UInt32Value(1U),
            OnApplyCellStyle = (cell, aggregator, aggrIndex, index, isSubTotal) =>
            {
                ExcelColumn member = valueColumns[aggrIndex];

                if (isSubTotal)
                {
                    cell.StyleIndex = new UInt32Value((uint)(((index - startRow) & 1) == 1 ? member.TotalStyleId : member.AlternateTotalStyleId));
                }
                else
                {
                    bool styleApplied = false;
                    if (member.HasConditionalStyle)
                    {
                        if (member.EvaluateSuccessCondition(member.Type, aggregator.Value))
                        {
                            if (member.SuccessStyleId.HasValue)
                            {
                                cell.StyleIndex = new UInt32Value((uint)member.SuccessStyleId.Value);
                                styleApplied = true;
                            }
                        }
                        else if (member.WarningStyle.HasValue && member.EvaluateWarningCondition(member.Type, aggregator.Value))
                        {
                            if (member.WarningStyleId.HasValue)
                            {
                                cell.StyleIndex = new UInt32Value((uint)member.WarningStyleId.Value);
                                styleApplied = true;
                            }
                        }
                        else
                        {
                            if (member.ErrorStyleId.HasValue)
                            {
                                cell.StyleIndex = new UInt32Value((uint)member.ErrorStyleId.Value);
                                styleApplied = true;
                            }
                        }
                    }
                    if (!styleApplied)
                    {
                        cell.StyleIndex = new UInt32Value((uint)(((index - startRow) & 1) == 1 ? member.StyleId : member.AlternateStyleId));
                    }
                }
            },
            FormatKey = (key, dim) =>
            {
                ExcelColumn? member = excelColumns.Find(pm => pm.Header == dim);
                return member is not null && key is not null ? ExcelHelper.GetRawValue(key) : key;
            },
            FormatValue = (aggregator, _) => ExcelHelper.GetRawValue(aggregator.Value),
            FormatMeasureHeader = (aggregator, index) => aggregator.FormatAggregator(valuePivotedMembers, index),
        };

        int totalColumns = writer.Write(pivotTable, data, cols, sharedTable, mergeCells, sheetDimension);

        if (template.HasHeader)
        {
            mergeCells.Add(new MergeCell
            {
                Reference = new StringValue(ExcelHelper.GetRange(ExcelHelper.GetColumnLetter(0), 1, ExcelHelper.GetColumnLetter(totalColumns), 1))
            });
        }

        Worksheet worksheet = Helper.CreateWorksheet(data, cols, mergeCells, sheetDimension);
        return BuildExcel(report.Aka, style, sharedTable, worksheet);
    }
}