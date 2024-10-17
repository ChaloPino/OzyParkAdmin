using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Domain.Reportes.MasterDetails;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Utilities;
using System.Data;
using System.Globalization;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Application.Reportes;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.MasterDetail;
internal partial class MasterDetailExcelBuilder
{
    private static class Helper
    {
        internal static IEnumerable<Column> CreateColumns(Dictionary<int, List<ExcelColumn>> excelColumns, DataSet dataSet, MasterDetailReport report)
        {
            Dictionary<int, Col> cols = [];

            ExcelBuilderHelper.CreateColumns(excelColumns[0], dataSet, report.MasterResultIndex, report.DataSource.Name, cols);

            if (report.HasDetail)
            {
                if (report.HasDynamicDetails)
                {
                    List<ReportDetail> details = [.. report.Details.OrderBy(d => d.DetailId)];

                    for (int i = 1; i < dataSet.Tables.Count; i++)
                    {
                        int index = i - 1;
                        ReportDetail detail = details[index % details.Count];
                        ExcelBuilderHelper.CreateColumns(excelColumns[detail.DetailId], dataSet, i, null, cols);
                    }
                }
                else if (report.Details is not null)
                {
                    foreach (ReportDetail detail in report.Details)
                    {
                        ExcelBuilderHelper.CreateColumns(excelColumns[detail.DetailId], dataSet, detail.DetailResultSetIndex, detail.DetailDataSource?.Name, cols);
                    }
                }
            }

            return cols.Values.Select(col => new Column
            {
                Min = new UInt32Value((uint)col.Min),
                Max = new UInt32Value((uint)col.Max),
                Width = new DoubleValue(col.Width),
                CustomWidth = new BooleanValue(true),
                BestFit = new BooleanValue(true)
            }).ToList();
        }

        public static void CreateRows(ExcelReportTemplate template, MasterDetailReport report, DataSet dataSet, Dictionary<int, List<ExcelColumn>> columns, SharedTable sharedTable, List<Row> list, Dictionary<int, int> titlePositions)
        {
            int rowIndex = 1;
            int count = columns.Max(v =>
            {
                if (v.Key == 0)
                {
                    return report.IsTabular ? 2 : v.Value.Count;
                }

                ReportDetail detail = report.Details!.First(d => d.DetailId == v.Key);

                return detail.IsTabular ? 2 : v.Value.Count;
            });

            string span = $"1:{count.ToString(CultureInfo.InvariantCulture)}";

            // Filters and Title
            if (template.HasHeader)
            {
                // Title
                Cell headerTitleCell = new()
                {
                    CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(0)}{rowIndex}"),
                    StyleIndex = new UInt32Value(2U),
                    DataType = new EnumValue<CellValues>(CellValues.SharedString),
                    CellValue = new CellValue("0"),
                };

                List<Cell> titleCells = columns.Skip(1).Select((_, i) => new Cell
                {
                    CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(i + 1)}{rowIndex}"),
                    StyleIndex = new UInt32Value(2U)
                }).ToList();

                titleCells.Insert(0, headerTitleCell);

                Row title = new(titleCells)
                {
                    RowIndex = new UInt32Value((uint)rowIndex++),
                    Spans = new ListValue<StringValue>([new(span)]),
                    DyDescent = new DoubleValue(0.3)
                };

                list.Add(title);
                // Dejar una línea en blanco
                rowIndex++;
            }

            CreateRowsDetail(dataSet, columns[0], 0, report.TitleInReport, report.IsTabular, report.MasterResultIndex, report.DataSource.Name, sharedTable, list, titlePositions, ref rowIndex);
            if (report.HasDetail)
            {
                if (report.HasDynamicDetails && report.Details is not null)
                {
                    List<ReportDetail> details = [.. report.Details.OrderBy(d => d.DetailId)];

                    for (int i = 1; i < dataSet.Tables.Count; i++)
                    {
                        int index = i - 1;
                        ReportDetail detail = details[index % details.Count];
                        CreateRowsDetail(dataSet, columns[detail.DetailId], detail.DetailId, detail.Title, detail.IsTabular, i, null, sharedTable, list, titlePositions, ref rowIndex);
                    }
                }
                else if (report.Details is not null)
                {
                    foreach (ReportDetail detail in report.Details)
                    {
                        CreateRowsDetail(dataSet, columns[detail.DetailId], detail.DetailId, detail.Title, detail.IsTabular, detail.DetailResultSetIndex, detail.DetailDataSource?.Name, sharedTable, list, titlePositions, ref rowIndex);
                    }
                }
            }
        }

        private static void CreateRowsDetail(DataSet dataSet, List<ExcelColumn> columns, int detailId, string? title, bool isTabular, int? index, string? name, SharedTable sharedTable, List<Row> list, Dictionary<int, int> titlePositions, ref int rowIndex)
        {
            int rowIdx = rowIndex;
            DataTable? dataTable = index.HasValue ? dataSet.Tables[index.Value] : dataSet.Tables[name];
            int count = isTabular ? 2 : columns.Count;
            string span = $"1:{count.ToString(CultureInfo.InvariantCulture)}";

            if (!string.IsNullOrEmpty(title))
            {
                // SubTitle
                Cell subTitleCell = new()
                {
                    CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(0)}{rowIndex}"),
                    StyleIndex = new UInt32Value(3U),
                    DataType = new EnumValue<CellValues>(CellValues.SharedString),
                    CellValue = new CellValue(sharedTable[title].ToString(CultureInfo.InvariantCulture))
                };
                Row subTitle = new(subTitleCell)
                {
                    RowIndex = new UInt32Value((uint)rowIndex),
                    Spans = new ListValue<StringValue>([new(span)]),
                    DyDescent = new DoubleValue(0.3)
                };
                titlePositions.Add(detailId, rowIndex);
                rowIdx++;
                list.Add(subTitle);
            }
            if (isTabular)
            {
                int idx = 0;
                columns.ForEach(column =>
                {
                    Cell[] cells =
                    [
                    new Cell
                    {
                        CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(0)}{rowIdx}"),
                        StyleIndex = new UInt32Value((uint)column.HeaderStyleId),
                        DataType = new EnumValue<CellValues>(CellValues.SharedString),
                        CellValue = new CellValue(column.HeaderSharedTable.ToString()!)
                    },
                    new Cell
                    {
                        CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(1)}{rowIdx}"),
                    }
                    ];

                    if (dataTable is not null)
                    {
                        ApplyStyleAndValueToCell(cells[1], column, dataTable.Rows[0], idx++);
                    }

                    Row row = new(cells)
                    {
                        RowIndex = new UInt32Value((uint)rowIdx++),
                        Spans = new ListValue<StringValue>([new(span)]),
                        DyDescent = new DoubleValue(0.3)
                    };

                    list.Add(row);
                });
            }
            else
            {
                // Header
                var headerCells = columns.Select((column, i) => new Cell
                {
                    CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(i)}{rowIdx}"),
                    StyleIndex = new UInt32Value((uint)column.HeaderStyleId),
                    DataType = new EnumValue<CellValues>(CellValues.SharedString),
                    CellValue = new CellValue(column.HeaderSharedTable.ToString()!)
                }).ToArray();

                Row headerRow = new(headerCells)
                {
                    RowIndex = new UInt32Value((uint)rowIdx++),
                    Spans = new ListValue<StringValue>([new(span)]),
                    DyDescent = new DoubleValue(0.3)
                };
                list.Add(headerRow);

                // Records
                int idx = 0;

                dataTable?.AsEnumerable().ToList().ForEach(row =>
                {
                    Cell[] cells = columns.Select((column, i) =>
                    {
                        Cell cell = new()
                        {
                            CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(i)}{rowIdx}"),
                        };
                        ApplyStyleAndValueToCell(cell, column, row, idx);
                        return cell;
                    }).ToArray();

                    Row bodyRow = new(cells)
                    {
                        RowIndex = new UInt32Value((uint)rowIdx++),
                        Spans = new ListValue<StringValue>([new(span)]),
                        DyDescent = new DoubleValue(0.3)
                    };
                    list.Add(bodyRow);
                    idx++;
                });
            }
            // Fila en blanco
            rowIdx++;
            rowIndex = rowIdx;
        }

        private static void ApplyStyleAndValueToCell(Cell cell, ExcelColumn column, DataRow row, int index)
        {
            bool styleApplied = false;

            if (column.HasConditionalStyle)
            {
                if (column.EvaluateSuccessCondition(column.Type, row[column.Name]))
                {
                    if (column.SuccessStyleId.HasValue)
                    {
                        cell.StyleIndex = new UInt32Value((uint)column.SuccessStyleId.Value);
                        styleApplied = true;
                    }
                }
                else if (column.WarningStyle.HasValue && column.EvaluateWarningCondition(column.Type, row[column.Name]))
                {
                    if (column.WarningStyleId.HasValue)
                    {
                        cell.StyleIndex = new UInt32Value((uint)column.WarningStyleId.Value);
                        styleApplied = true;
                    }
                }
                else
                {
                    if (column.ErrorStyleId.HasValue)
                    {
                        cell.StyleIndex = new UInt32Value((uint)column.ErrorStyleId.Value);
                        styleApplied = true;
                    }
                }
            }
            if (!styleApplied)
            {
                cell.StyleIndex = new UInt32Value((index & 1) == 1 ? (uint)column.StyleId : (uint)column.AlternateStyleId);
            }
            if (row[column.Name] != null)
            {
                string? value = ExcelHelper.GetValue(row, column);

                if (value is not null)
                {
                    cell.CellValue = new CellValue(value);
                }

                if (column.IsText())
                {
                    cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                }
            }
        }

        internal static void CreateStyle(ExcelReportTemplate template, MasterDetailReport report, Dictionary<int, List<ExcelColumn>> excelColumns, Style style)
        {
            style.NumberFormats = StyleHelper.CreateNumberingFormats(excelColumns.Values.SelectMany(c => c.Select(v => v.ExcelFormat)).ToList());
            style.Fonts = StyleHelper.CreateFonts(template, report, report.Details);
            style.Fills = StyleHelper.CreateFills();
            style.Borders = StyleHelper.CreateBorders();
            style.CellStyleFormats = StyleHelper.CreateCellStyleFormats();
            style.CellFormats = CreateCellFormats(template, report, report.Details, excelColumns.Values.SelectMany(c => c).ToList(), style.CellStyleFormats.ToArray());
            style.CellStyles = StyleHelper.CreateCellStyles();
            style.DifferentialFormats = StyleHelper.CreateDifferentialFormats();
            style.TableStyles = StyleHelper.CreateTableStyles();
        }

        private static IEnumerable<CellFormat> CreateCellFormats(ExcelReportTemplate template, MasterDetailReport report, IEnumerable<ReportDetail> details, List<ExcelColumn> excelColumns, CellFormat[] baseCellFormats)
        {
            List<CellFormat> cellFormats = [];
            StyleHelper.CreateHeaderCellFormats(excelColumns, cellFormats);
            StyleHelper.CreateTitleFormats(template, report.TitleInReport, details, cellFormats);
            StyleHelper.CreateBodyCellFormats(excelColumns, cellFormats);
            StyleHelper.CreateConditionalCellFormats(excelColumns, cellFormats, baseCellFormats);
            return [.. cellFormats];
        }

        internal static void CreateSharedTable(ExcelReportTemplate template, MasterDetailReport report, DataSet dataSet, ClaimsPrincipal user, SharedTable sharedTable, Dictionary<int, List<ExcelColumn>> excelColumns)
        {
            if (template.HasHeader && !string.IsNullOrEmpty(template.HeaderTitle))
            {
                _ = sharedTable.AddString(template.HeaderTitle);
            }

            if (!string.IsNullOrEmpty(report.TitleInReport))
            {
                _ = sharedTable.AddString(report.TitleInReport);
            }
            if (report.HasDetail && report.Details.Any())
            {
                foreach (var detail in report.Details.Where(d => !string.IsNullOrEmpty(d.Title)))
                {
                    _ = sharedTable.AddString(detail.Title!);
                }
            }

            List<ExcelColumn> columns = SharedTableHelper.CreateSharedTable(report.Columns, user, sharedTable);
            excelColumns.Add(0, columns);

            SharedTableHelper.CreateSharedTable(dataSet, report.MasterResultIndex, report.DataSource.Name, sharedTable, columns);
            if (report.HasDetail)
            {
                if (report.HasDynamicDetails && report.Details is not null)
                {
                    var details = report.Details.OrderBy(d => d.DetailId).ToList();
                    foreach (ReportDetail detail in details)
                    {
                        List<ExcelColumn> detailColumns = SharedTableHelper.CreateSharedTable(detail.DetailColumns, user, sharedTable);
                        excelColumns.Add(detail.DetailId, detailColumns);
                    }
                    for (int i = 1; i < dataSet.Tables.Count; i++)
                    {
                        int index = i - 1;
                        ReportDetail detail = details[index % details.Count];
                        SharedTableHelper.CreateSharedTable(dataSet, i, null, sharedTable, excelColumns[detail.DetailId]);
                    }
                }
                else if (report.Details is not null)
                {
                    foreach (ReportDetail detail in report.Details)
                    {
                        List<ExcelColumn> detailColumns = SharedTableHelper.CreateSharedTable(detail.DetailColumns, user, sharedTable);
                        excelColumns.Add(detail.DetailId, detailColumns);
                        SharedTableHelper.CreateSharedTable(dataSet, detail.DetailResultSetIndex, detail.DetailDataSource?.Name, sharedTable, detailColumns);
                    }
                }
            }
        }

        internal static Worksheet CreateWorksheet(DataSet dataSet, Dictionary<int, List<ExcelColumn>> excelColumns, MasterDetailReport report, bool hasHeader, IEnumerable<Row> rows, IEnumerable<Column> cols, IEnumerable<MergeCell> merges)
        {
            // Calcula la columna
            int colCount = report.IsTabular ? 2 : excelColumns[0].Count;

            if (report.HasDetail && report.Details.Any())
            {
                foreach (var detail in report.Details)
                {
                    colCount = detail.IsTabular ? Math.Max(colCount, 2) : Math.Max(colCount, excelColumns[detail.DetailId].Count);
                }
            }

            // Suma uno para contar la cabecera
            int rowCount = 0;
            if (hasHeader)
            {
                rowCount = 2; // Considerando el espacio
            }
            if (!string.IsNullOrWhiteSpace(report.TitleInReport))
            {
                rowCount++;
            }

            DataTable dataTable = report.MasterResultIndex.HasValue
                ? dataSet.Tables[report.MasterResultIndex.Value]
                : dataSet.Tables[report.DataSource.Name]!;

            if (report.IsTabular)
            {
                rowCount += excelColumns[0].Count;
            }
            else
            {
                rowCount += dataTable.Rows.Count + 1; // Considerando cabecera
            }

            if (report.HasDetail)
            {
                if (report.HasDynamicDetails)
                {
                    List<ReportDetail> details = [.. report.Details.OrderBy(d => d.DetailId)];

                    for (int i = 1; i < dataSet.Tables.Count; i++)
                    {
                        int index = i - 1;
                        ReportDetail detail = details[index % details.Count];
                        rowCount++; // Considerando espacio.
                        if (!string.IsNullOrEmpty(detail.Title))
                        {
                            rowCount++;
                        }
                        dataTable = dataSet.Tables[i];
                        if (detail.IsTabular)
                        {
                            rowCount += excelColumns[detail.DetailId].Count;
                        }
                        else
                        {
                            rowCount += dataTable.Rows.Count + 1; // Considerando cabecera
                        }
                    }
                }
                else if (report.Details is not null)
                {
                    foreach (var detail in report.Details)
                    {
                        rowCount++; // Considerando espacio.
                        if (!string.IsNullOrEmpty(detail.Title))
                        {
                            rowCount++;
                        }

                        dataTable = detail.DetailResultSetIndex.HasValue
                            ? dataSet.Tables[detail.DetailResultSetIndex.Value]
                            : dataSet.Tables[detail.DetailDataSource?.Name]!;

                        if (detail.IsTabular)
                        {
                            rowCount += excelColumns[detail.DetailId].Count;
                        }
                        else
                        {
                            rowCount += dataTable.Rows.Count + 1; // Considerando cabecera
                        }
                    }
                }
            }

            string dimensionRef = ExcelHelper.GetRange(ExcelHelper.GetColumnLetter(0), 1, ExcelHelper.GetColumnLetter(colCount - 1), rowCount);

            SheetView sheetView = new()
            {
                TabSelected = new BooleanValue(true),
                WorkbookViewId = new UInt32Value(0u),
            };

            SheetDimension sheetDimension = new()
            {
                Reference = new StringValue(dimensionRef)
            };
            SheetViews sheetViews = new(sheetView);
            SheetFormatProperties sheetFormatProperties = new()
            {
                BaseColumnWidth = new UInt32Value(10u),
                DefaultRowHeight = new DoubleValue(14.4),
                DyDescent = new DoubleValue(0.3)
            };

            SheetData sheetData = new(rows);

            Columns columns = new(cols);

            List<OpenXmlElement> elements =
            [
                sheetDimension,
            sheetViews,
            sheetFormatProperties,
            columns,
            sheetData
            ];

            if (merges.Any())
            {
                elements.Add(new MergeCells(merges)
                {
                    Count = new UInt32Value((uint)merges.Count())
                });
            }

            return new(elements);
        }
    }
}
