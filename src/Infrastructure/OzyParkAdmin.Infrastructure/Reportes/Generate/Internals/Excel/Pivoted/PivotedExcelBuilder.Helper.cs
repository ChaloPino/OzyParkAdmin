using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Utilities;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Pivoted;
internal partial class PivotedExcelBuilder
{
    private static class Helper
    {
        private const string mainNamespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        private const string markupCompatibilityNamespace = "http://schemas.openxmlformats.org/markup-compatibility/2006";
        private const string spreadsheetmlacNamespace = "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac";
        private const string spreadsheetmlrevisionNamespace = "http://schemas.microsoft.com/office/spreadsheetml/2014/revision";
        private const string spreadsheetmlrevision2Namespace = "http://schemas.microsoft.com/office/spreadsheetml/2015/revision2";
        private const string spreadsheetmlrevision3Namespace = "http://schemas.microsoft.com/office/spreadsheetml/2016/revision3";

        internal static Worksheet CreateWorksheet(List<Row> data, List<Column> cols, List<MergeCell> merges, SheetDimension sheetDimension)
        {
            List<OpenXmlElement> elements =
            [
                sheetDimension
            ];

            SheetView sheetView = new()
            {
                TabSelected = new BooleanValue(true),
                WorkbookViewId = new UInt32Value((uint)0),
            };
            SheetViews sheetViews = new(sheetView);
            elements.Add(sheetViews);

            SheetFormatProperties sheetFormatProperties = new()
            {
                BaseColumnWidth = new UInt32Value((uint)10),
                DefaultRowHeight = new DoubleValue(14.4),
                DyDescent = new DoubleValue(0.3)
            };

            elements.Add(sheetFormatProperties);

            if (cols.Count != 0)
            {
                Columns columns = new(cols);
                elements.Add(columns);
            }

            SheetData sheetData = new(data);
            elements.Add(sheetData);

            if (merges.Count != 0)
            {
                MergeCells mergeCells = new(merges)
                {
                    Count = new UInt32Value((uint)merges.Count)
                };
                elements.Add(mergeCells);
            }

            Worksheet worksheet = new(elements);
            worksheet.AddNamespaceDeclaration("x", mainNamespace);
            worksheet.AddNamespaceDeclaration("mc", markupCompatibilityNamespace);
            worksheet.AddNamespaceDeclaration("x14ac", spreadsheetmlacNamespace);
            worksheet.AddNamespaceDeclaration("xr", spreadsheetmlrevisionNamespace);
            worksheet.AddNamespaceDeclaration("xr2", spreadsheetmlrevision2Namespace);
            worksheet.AddNamespaceDeclaration("xr3", spreadsheetmlrevision3Namespace);

            worksheet.MCAttributes = new MarkupCompatibilityAttributes
            {
                Ignorable = new StringValue("x14ac xr xr2 xr3")
            };
            return worksheet;
        }

        internal static Style CreateStyle(ExcelReportTemplate template, List<ExcelFilter> filters, List<ExcelColumn> excelColumns, List<ExcelColumn> rowColumns, List<ExcelColumn> columnColumns, List<ExcelColumn> valueColums)
        {
            Style style = new()
            {
                NumberFormats = StyleHelper.CreateNumberingFormats(filters.Where(f => f.ExcelFormat is not null).Select(f => f.ExcelFormat).Union(excelColumns.Where(c => c.ExcelFormat is not null).Select(c => c.ExcelFormat)).ToList()),
                Fonts = StyleHelper.CreateFonts(template),
                Fills = StyleHelper.CreateFills(),
                Borders = StyleHelper.CreateBorders(),
                CellStyleFormats = StyleHelper.CreateCellStyleFormats()
            };
            style.CellFormats = StyleHelper.CreateCellFormats(template, filters, rowColumns, columnColumns, valueColums, style.CellStyleFormats.ToArray());
            style.CellStyles = StyleHelper.CreateCellStyles();
            style.DifferentialFormats = StyleHelper.CreateDifferentialFormats();
            style.TableStyles = StyleHelper.CreateTableStyles();

            return style;
        }

        internal static List<Row> CreateRows(ExcelReportTemplate template, List<ExcelFilter> filters, out int startRow)
        {
            List<Row> rows = [];
            startRow = 1;

            if (template.HasHeader)
            {
                // Title
                Cell headerTitleCell = new()
                {
                    CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(0)}{startRow}"),
                    StyleIndex = new UInt32Value(2U),
                    DataType = new EnumValue<CellValues>(CellValues.SharedString),
                    CellValue = new CellValue("0")
                };

                Row title = new(headerTitleCell)
                {
                    RowIndex = new UInt32Value((uint)startRow++),
                    DyDescent = new DoubleValue(0.3)
                };

                rows.Add(title);

                foreach (ExcelFilter filter in filters)
                {
                    Cell[] cells =
                    [
                    new Cell
                    {
                        CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(0)}{startRow}"),
                        StyleIndex = new UInt32Value((uint)filter.HeaderStyleId),
                        DataType = new EnumValue<CellValues>(CellValues.SharedString),
                        CellValue = new CellValue(filter.HeaderSharedTable.ToString())
                    },
                    new Cell
                    {
                        CellReference = new StringValue($"{ExcelHelper.GetColumnLetter(1)}{startRow}"),
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
                        RowIndex = new UInt32Value((uint)startRow++),
                        DyDescent = new DoubleValue(0.3)
                    };

                    rows.Add(filterRow);
                }

                // Dejar una línea en blanco
                startRow++;
            }
            return rows;
        }
    }
}
