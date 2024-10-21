using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Data;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Excel;
using System.Globalization;
using OzyParkAdmin.Domain.Reportes.MasterDetails;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Utilities;
internal static class ExcelBuilderHelper
{
    internal static Worksheet CreateWorksheet(int colCount, int rowCount, int filterCount, bool hasTotals, bool hasHeader, IEnumerable<Row> data, IEnumerable<Column> cols)
    {
        // Suma uno para contar la cabecera
        rowCount++;

        if (hasTotals)
        {
            rowCount++;
        }

        MergeCells? mergeCells = null;

        if (hasHeader)
        {
            // Suma para contar el título
            rowCount++;
            // Suma todos los filtros
            rowCount += filterCount;
            // Suma una línea en blanco
            rowCount++;

            string mergeRef = ExcelHelper.GetRange(ExcelHelper.GetColumnLetter(0), 1, ExcelHelper.GetColumnLetter(colCount - 1), 1);

            MergeCell[] merges =
            [
                new()
                {
                    Reference = new StringValue(mergeRef)
                }
            ];

            mergeCells = new MergeCells(merges);
        }

        string dimensionRef = ExcelHelper.GetRange(ExcelHelper.GetColumnLetter(0), 1, ExcelHelper.GetColumnLetter(colCount - 1), rowCount);

        SheetView sheetView = new()
        {
            TabSelected = new BooleanValue(true),
            WorkbookViewId = new UInt32Value((uint)0),
        };

        SheetDimension sheetDimension = new()
        {
            Reference = new StringValue(dimensionRef)
        };
        SheetViews sheetViews = new(sheetView);
        SheetFormatProperties sheetFormatProperties = new()
        {
            BaseColumnWidth = new UInt32Value((uint)10),
            DefaultRowHeight = new DoubleValue(14.4),
            DyDescent = new DoubleValue(0.3)
        };

        SheetData sheetData = new(data);

        Columns columns = new(cols);

        List<OpenXmlElement> elements =
        [
            sheetDimension,
            sheetViews,
            sheetFormatProperties,
            columns,
            sheetData
        ];

        if (mergeCells != null)
        {
            elements.Add(mergeCells);
        }

        return new(elements);
    }

    internal static IDictionary<string, object?>? CreateTotals(IList<ColumnBase> columns, IList<DataRow> rows)
    {
        return rows.Aggregate(columns, false);
    }
    internal static IEnumerable<Column> CreateColumns(IList<ExcelColumn> columns, IList<DataRow> rows, IDictionary<string, object?>? totals)
    {
        Dictionary<int, Col> cols = columns.Select((column, i) =>
        {
            Col col = new()
            {
                Min = i + 1,
                Max = i + 1
            };

            if (column.Header is not null)
            {
                col.Width = ExcelHelper.MeasureText(column.Header);
            }

            return new KeyValuePair<int, Col>(i, col);
        }).ToDictionary(k => k.Key, v => v.Value);

        foreach (DataRow row in rows)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                ExcelColumn column = columns[i];
                if (row[column.Name] != null)
                {
                    cols[i].Width = Math.Max(cols[i].Width, ExcelHelper.MeasureText(row[column.Name].ToString()!));
                }
            }
        }
        if (totals is not null)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                ExcelColumn column = columns[i];
                if (totals.TryGetValue(column.Name, out object? value) && value is not null)
                {
                    cols[i].Width = Math.Max(cols[i].Width, ExcelHelper.MeasureText(value.ToString()!));
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

    internal static void CreateColumns(List<ExcelColumn> columns, DataSet dataSet, int? index, string? name, Dictionary<int, Col> cols)
    {
        DataTable? dataTable = index.HasValue ? dataSet.Tables[index.Value] : dataSet.Tables[name];

        if (dataTable is not null)
        {
            int idx = 0;
            columns.ForEach(column =>
            {
                if (!cols.TryGetValue(idx, out Col? value))
                {
                    Col col = new()
                    {
                        Min = idx + 1,
                        Max = idx + 1,
                    };

                    if (column.Header is not null)
                    {
                        col.Width = ExcelHelper.MeasureText(column.Header);
                    }

                    cols.Add(idx, col);
                }
                else
                {
                    double width = column.Header is not null ? ExcelHelper.MeasureText(column.Header) : 0D;
                    value.Width = Math.Max(value.Width, width);
                }
                idx++;
            });

            dataTable.AsEnumerable().ToList().ForEach(row =>
            {
                idx = 0;
                columns.ForEach(column =>
                {
                    if (!cols.TryGetValue(idx, out Col? value))
                    {
                        cols.Add(idx, new Col
                        {
                            Min = idx + 1,
                            Max = idx + 1,
                            Width = ExcelHelper.MeasureText(row[column.Name].ToString()!)
                        });
                    }
                    else
                    {
                        value.Width = Math.Max(value.Width, ExcelHelper.MeasureText(row[column.Name].ToString()!));
                    }
                    idx++;
                });
            });
        }
    }

    internal static IEnumerable<Row> CreateRowData(ExcelReportTemplate template, IList<ExcelFilter> filters, IList<ExcelColumn> columns, IList<DataRow> rows, IDictionary<string, object?>? totals)
    {
        List<Row> list = [];
        int rowIndex = 1;

        string span = $"1:{columns.Count.ToString(CultureInfo.InvariantCulture)}";

        // Filters and Title
        if (template.HasHeader)
        {
            // Title
            Cell headerTitleCell = new()
            {
                CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(0)}{rowIndex}"),
                StyleIndex = new UInt32Value(2U),
                DataType = new EnumValue<CellValues>(CellValues.SharedString),
                CellValue = new CellValue("0")
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
                Spans = new ListValue<StringValue>([new StringValue(span)]),
                DyDescent = new DoubleValue(0.3)
            };

            list.Add(title);

            foreach (ExcelFilter filter in filters)
            {
                Cell[] cells =
                [
                    new Cell
                    {
                        CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(0)}{rowIndex}"),
                        StyleIndex = new UInt32Value((uint)filter.HeaderStyleId),
                        DataType = new EnumValue<CellValues>(CellValues.SharedString),
                        CellValue = new CellValue(filter.HeaderSharedTable.ToString())
                    },
                    new Cell
                    {
                        CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(1)}{rowIndex}"),
                        StyleIndex = new UInt32Value((uint)filter.StyleId),
                    }
                ];

                string? value = filter.GetValue();

                if (value is not null)
                {
                    if (filter.HasTextValue())
                    {
                        cells[1].DataType = new EnumValue<CellValues>(CellValues.SharedString);

                        if (filter.SharedTable.HasValue)
                        {
                            cells[1].CellValue = new CellValue(filter.SharedTable.Value.ToString());
                        }
                    }
                    else
                    {
                        cells[1].CellValue = new CellValue(value);
                    }
                }

                Row filterRow = new(cells)
                {
                    RowIndex = new UInt32Value((uint)rowIndex++),
                    Spans = new ListValue<StringValue>([new StringValue(span)]),
                    DyDescent = new DoubleValue(0.3)
                };

                list.Add(filterRow);
            }

            // Dejar una línea en blanco
            rowIndex++;
        }

        // Donde debe empezar para las fórmulas.
        int startBodyRow = rowIndex + 1;

        // Header
        List<Cell> headerCells = columns.Select((column, i) => new Cell
        {
            CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(i)}{rowIndex}"),
            StyleIndex = new UInt32Value((uint)column.HeaderStyleId),
            DataType = new EnumValue<CellValues>(CellValues.SharedString),
            CellValue = column.HeaderSharedTable.HasValue ? new CellValue(column.HeaderSharedTable.Value.ToString(CultureInfo.InvariantCulture)) : null,
        }).ToList();

        Row header = new(headerCells)
        {
            RowIndex = new UInt32Value((uint)rowIndex++),
            Spans = new ListValue<StringValue>([new StringValue(span)]),
            DyDescent = new DoubleValue(0.3),
        };

        list.Add(header);

        // Rows
        IEnumerable<Row> itemData = rows.Select((row, index) =>
        {
            List<Cell> bodyCells = columns.Select((column, i) =>
            {
                Cell cell = new()
                {
                    CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(i)}{rowIndex}"),
                };

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
                    else if (column.EvaluateWarningCondition(column.Type, row[column.Name]))
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
                return cell;
            }).ToList();

            Row body = new(bodyCells)
            {
                RowIndex = new UInt32Value((uint)rowIndex++),
                Spans = new ListValue<StringValue>([new StringValue(span)]),
                DyDescent = new DoubleValue(0.3),
            };
            return body;
        }).ToList();

        list.AddRange(itemData);

        if (totals is not null)
        {
            List<Cell> footerCells = columns.Select((column, i) =>
            {
                Cell cell = new()
                {
                    CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(i)}{rowIndex}"),
                    StyleIndex = column.FooterStyleId.HasValue ? new UInt32Value((uint)column.FooterStyleId.Value) : null,
                };
                if (column.AggregationType.HasValue)
                {
                    if (totals[column.Name] != null)
                    {
                        if (column.IsText())
                        {
                            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                        }
                        string? value = ExcelHelper.GetValue(totals, column);

                        if (value is not null)
                        {
                            cell.CellValue = new CellValue(value);
                        }
                    }

                    string columnLetter = ExcelHelper.GetColumnLetter(i);
                    string? formula = ExcelHelper.GetSubtotal(column, ExcelHelper.GetRange(columnLetter, startBodyRow, columnLetter, startBodyRow - 1 + rows.Count));

                    if (formula is not null)
                    {
                        cell.CellFormula = new CellFormula(formula);
                    }
                }
                return cell;
            }).ToList();

            Row footer = new(footerCells)
            {
                RowIndex = new UInt32Value((uint)rowIndex++),
                Spans = new ListValue<StringValue>([new StringValue(span)]),
                ThickTop = new BooleanValue(true),
                Height = new DoubleValue(15.0),
                DyDescent = new DoubleValue(0.3),
            };
            list.Add(footer);
        }
        return list;
    }

    public static IEnumerable<MergeCell> CreateMerges(MasterDetailReport report, ExcelReportTemplate template, Dictionary<int, List<ExcelColumn>> columns, Dictionary<int, int> titlePositions)
    {
        List<MergeCell> merges = [];

        if (template.HasHeader)
        {
            int count = columns.Max(c =>
            {
                if (c.Key == 0)
                {
                    return report.IsTabular ? 2 : c.Value.Count;
                }

                ReportDetail detail = report.Details!.First(d => d.DetailId == c.Key);
                return detail.IsTabular ? 2 : c.Value.Count;
            }) - 1;

            merges.Add(new MergeCell
            {
                Reference = $"{ExcelHelper.GetRange(ExcelHelper.GetColumnLetter(0), 1, ExcelHelper.GetColumnLetter(count), 1)}",
            });
        }
        if (!string.IsNullOrEmpty(report.TitleInReport))
        {
            int count = report.IsTabular ? 2 : columns[0].Count;
            int row = titlePositions[0];
            merges.Add(new MergeCell
            {
                Reference = $"{ExcelHelper.GetRange(ExcelHelper.GetColumnLetter(0), row, ExcelHelper.GetColumnLetter(count), row)}",
            });
        }
        if (report.HasDetail && report.Details is not null)
        {
            foreach (ReportDetail detail in report.Details)
            {
                if (!string.IsNullOrEmpty(detail.Title))
                {
                    int count = detail.IsTabular ? 1 : columns[detail.DetailId].Count - 1;
                    int row = titlePositions[detail.DetailId];
                    merges.Add(new MergeCell
                    {
                        Reference = $"{ExcelHelper.GetRange(ExcelHelper.GetColumnLetter(0), row, ExcelHelper.GetColumnLetter(count), row)}",
                    });
                }
            }
        }

        return merges;
    }
}
