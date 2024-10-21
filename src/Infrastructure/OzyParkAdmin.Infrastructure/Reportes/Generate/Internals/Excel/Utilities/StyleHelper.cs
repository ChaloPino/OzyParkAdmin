using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Domain.Reportes.MasterDetails;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Utilities;
internal static class StyleHelper
{
    internal static Style CreateStyle(ExcelReportTemplate template, IList<ExcelFilter> filters, IList<ExcelColumn> columns)
    {
        Style style = new()
        {
            NumberFormats = CreateNumberingFormats(columns.Select(c => c.ExcelFormat).Union(filters.Select(f => f.ExcelFormat)).ToList()),
            Fonts = CreateFonts(template),
            Fills = CreateFills(),
            Borders = CreateBorders(),
            CellStyleFormats = CreateCellStyleFormats()
        };

        style.CellFormats = CreateCellFormats(template, columns, filters, style.CellStyleFormats.ToArray());
        style.CellStyles = CreateCellStyles();
        style.DifferentialFormats = CreateDifferentialFormats();
        style.TableStyles = CreateTableStyles();

        return style;
    }

    public static NumberingFormat[] CreateNumberingFormats(IList<ExcelFormat?> formats)
    {
        var filteredFormats = formats.Where(e => e is not null).Select(e => e!);

        return filteredFormats
            .Where(e => !e.IsBuiltIn)
            .OrderBy(e => e.Id)
            .Distinct()
            .Select(e => new NumberingFormat
            {
                NumberFormatId = new UInt32Value((uint)e.Id),
                FormatCode = new StringValue(e.Format)
            })
            .ToArray();
    }

    public static Font[] CreateFonts(ExcelReportTemplate template, MasterDetailReport report, IEnumerable<ReportDetail> details)
    {
        List<Font> fonts = [.. CreateFonts(template)];

        if (!string.IsNullOrEmpty(report.TitleInReport) || details.Any(d => !string.IsNullOrEmpty(d.Title)))
        {
            // 9 - Subtitle
            fonts.Add(new Font
            {
                Bold = new Bold(),
                FontSize = new FontSize { Val = new DoubleValue(12.0) },
                Color = new Color { Theme = new UInt32Value(1U) },
                FontName = new FontName { Val = new StringValue("Calibri") },
                FontFamilyNumbering = new FontFamilyNumbering { Val = new Int32Value(2) },
                FontScheme = new FontScheme { Val = new EnumValue<FontSchemeValues>(FontSchemeValues.Minor) }
            });
        }
        return [.. fonts];
    }

    public static Font[] CreateFonts(ExcelReportTemplate template)
    {
        List<Font> fonts =
        [
            // 0 - Cuerpo
            new Font
            {
                FontSize = new FontSize { Val = new DoubleValue(11.0) },
                Color = new Color { Theme = new UInt32Value(1U) },
                FontName = new FontName { Val = new StringValue("Calibri") },
                FontFamilyNumbering = new FontFamilyNumbering { Val = new Int32Value(2) },
                FontScheme = new FontScheme { Val = new EnumValue<FontSchemeValues>(FontSchemeValues.Minor) }
            },
            // 1 - Header
            new Font
            {
                Bold = new Bold(),
                FontSize = new FontSize { Val = new DoubleValue(11.0) },
                Color = new Color { Theme = new UInt32Value(0U) },
                FontName = new FontName { Val = new StringValue("Calibri") },
                FontFamilyNumbering = new FontFamilyNumbering { Val = new Int32Value(2) },
                FontScheme = new FontScheme { Val = new EnumValue<FontSchemeValues>(FontSchemeValues.Minor) }
            },
            // 2 - Footer
            new Font
            {
                Bold = new Bold(),
                FontSize = new FontSize { Val = new DoubleValue(11.0) },
                Color = new Color { Theme = new UInt32Value(1U) },
                FontName = new FontName { Val = new StringValue("Calibri") },
                FontFamilyNumbering = new FontFamilyNumbering { Val = new Int32Value(2) },
                FontScheme = new FontScheme { Val = new EnumValue<FontSchemeValues>(FontSchemeValues.Minor) }
            },
            // 3 - Success
            new Font
            {
                FontSize = new FontSize { Val = new DoubleValue(11.0) },
                Color = new Color { Rgb = new HexBinaryValue("FF006100") },
                FontName = new FontName { Val = new StringValue("Calibri") },
                FontFamilyNumbering = new FontFamilyNumbering { Val = new Int32Value(2) },
                FontScheme = new FontScheme { Val = new EnumValue<FontSchemeValues>(FontSchemeValues.Minor) }
            },
            // 4 - Error
            new Font
            {
                FontSize = new FontSize { Val = new DoubleValue(11.0) },
                Color = new Color { Rgb = new HexBinaryValue("FF9C0006") },
                FontName = new FontName { Val = new StringValue("Calibri") },
                FontFamilyNumbering = new FontFamilyNumbering { Val = new Int32Value(2) },
                FontScheme = new FontScheme { Val = new EnumValue<FontSchemeValues>(FontSchemeValues.Minor) }
            },
            // 5 - Warning
            new Font
            {
                FontSize = new FontSize { Val = new DoubleValue(11.0) },
                Color = new Color { Rgb = new HexBinaryValue("FF9C5700") },
                FontName = new FontName { Val = new StringValue("Calibri") },
                FontFamilyNumbering = new FontFamilyNumbering { Val = new Int32Value(2) },
                FontScheme = new FontScheme { Val = new EnumValue<FontSchemeValues>(FontSchemeValues.Minor) }
            },
            // 6 - Info y Dark
            new Font
            {
                FontSize = new FontSize { Val = new DoubleValue(11.0) },
                Color = new Color { Theme = new UInt32Value(0U) },
                FontName = new FontName { Val = new StringValue("Calibri") },
                FontFamilyNumbering = new FontFamilyNumbering { Val = new Int32Value(2) },
                FontScheme = new FontScheme { Val = new EnumValue<FontSchemeValues>(FontSchemeValues.Minor) }
            },
            // 7 - Light
            new Font
            {
                FontSize = new FontSize { Val = new DoubleValue(11.0) },
                Color = new Color { Theme = new UInt32Value(1U) },
                FontName = new FontName { Val = new StringValue("Calibri") },
                FontFamilyNumbering = new FontFamilyNumbering { Val = new Int32Value(2) },
                FontScheme = new FontScheme { Val = new EnumValue<FontSchemeValues>(FontSchemeValues.Minor) }
            },
        ];
        if (template.HasHeader)
        {
            // 8 - Title
            fonts.Add(new Font
            {
                Bold = new Bold(),
                FontSize = new FontSize { Val = new DoubleValue(14.0) },
                Color = new Color { Theme = new UInt32Value(1U) },
                FontName = new FontName { Val = new StringValue("Calibri") },
                FontFamilyNumbering = new FontFamilyNumbering { Val = new Int32Value(2) },
                FontScheme = new FontScheme { Val = new EnumValue<FontSchemeValues>(FontSchemeValues.Minor) }
            });
        }

        return [.. fonts];
    }

    public static Fill[] CreateFills()
    {
        return
        [
            // 0 - Default Fill (Event Body and Footer)
            new Fill
            {
                PatternFill = new PatternFill{ PatternType = new EnumValue<PatternValues>(PatternValues.None) }
            },
            // 1 - Default Gray Fill
            new Fill
            {
                PatternFill = new PatternFill{ PatternType = new EnumValue<PatternValues>(PatternValues.Gray125) }
            },
            // 2 - Header Fill
            new Fill
            {
                PatternFill = new PatternFill
                {
                    PatternType = new EnumValue<PatternValues>(PatternValues.Solid),
                    ForegroundColor = new ForegroundColor { Theme = new UInt32Value(4U) },
                    BackgroundColor = new BackgroundColor{ Theme = new UInt32Value(4U) }
                }
            },
            // 3 - Body Fill (Odd)
            new Fill
            {
                PatternFill = new PatternFill
                {
                    PatternType = new EnumValue<PatternValues>(PatternValues.Solid),
                    ForegroundColor = new ForegroundColor
                    {
                        Theme = new UInt32Value(4U),
                        Tint = new DoubleValue(0.79998168889431442)
                    },
                    BackgroundColor = new BackgroundColor
                    {
                        Theme = new UInt32Value(4U),
                        Tint = new DoubleValue(0.79998168889431442)
                    }
                }
            },
            // 4 - Success Fill
            new Fill
            {
                PatternFill = new PatternFill
                {
                    PatternType = new EnumValue<PatternValues>(PatternValues.Solid),
                    ForegroundColor = new ForegroundColor
                    {
                        Rgb = new HexBinaryValue("FFC6EFCE")
                    }
                }
            },
            // 5 - Error Fill
            new Fill
            {
                PatternFill = new PatternFill
                {
                    PatternType = new EnumValue<PatternValues>(PatternValues.Solid),
                    ForegroundColor = new ForegroundColor
                    {
                        Rgb = new HexBinaryValue("FFFFC7CE")
                    }
                }
            },
            // 6 - Warning Fill
            new Fill
            {
                PatternFill = new PatternFill
                {
                    PatternType = new EnumValue<PatternValues>(PatternValues.Solid),
                    ForegroundColor = new ForegroundColor
                    {
                        Rgb = new HexBinaryValue("FFFFEB9C")
                    }
                }
            },
            // 7 - Info Fill
            new Fill
            {
                PatternFill = new PatternFill
                {
                    PatternType = new EnumValue<PatternValues>(PatternValues.Solid),
                    ForegroundColor = new ForegroundColor
                    {
                        Theme = new UInt32Value(4U)
                    }
                }
            },
            // 8 - Light Fill
            new Fill
            {
                PatternFill = new PatternFill
                {
                    PatternType = new EnumValue<PatternValues>(PatternValues.Solid),
                    ForegroundColor = new ForegroundColor
                    {
                        Theme = new UInt32Value(6U),
                        Tint = new DoubleValue(0.79998168889431442)
                    },
                    BackgroundColor = new BackgroundColor
                    {
                        Indexed = new UInt32Value(65U)
                    }
                }
            },
            // 9 - Dark Fill
            new Fill
            {
                PatternFill = new PatternFill
                {
                    PatternType = new EnumValue<PatternValues>(PatternValues.Solid),
                    ForegroundColor = new ForegroundColor
                    {
                        Theme = new UInt32Value(6U)
                    }
                }
            },
        ];
    }

    public static Border[] CreateBorders()
    {
        return
        [
            // 0 - Default Border
            new Border
            {
                LeftBorder = new LeftBorder(),
                RightBorder = new RightBorder(),
                TopBorder = new TopBorder(),
                BottomBorder = new BottomBorder(),
                DiagonalBorder = new DiagonalBorder()
            },
            // 1 - Header and Body Border
            new Border
            {
                LeftBorder = new LeftBorder
                {
                    Style = new EnumValue<BorderStyleValues>(BorderStyleValues.Thin),
                    Color = new Color { Theme = new UInt32Value(4U), Tint = new DoubleValue(0.39994506668294322) }
                },
                RightBorder = new RightBorder
                {
                    Style = new EnumValue<BorderStyleValues>(BorderStyleValues.Thin),
                    Color = new Color { Theme = new UInt32Value(4U), Tint = new DoubleValue(0.39994506668294322) }
                },
                TopBorder = new TopBorder
                {
                    Style = new EnumValue<BorderStyleValues>(BorderStyleValues.Thin),
                    Color = new Color { Theme = new UInt32Value(4U), Tint = new DoubleValue(0.39997558519241921) }
                },
                BottomBorder = new BottomBorder
                {
                    Style = new EnumValue<BorderStyleValues>(BorderStyleValues.Thin),
                    Color = new Color { Theme = new UInt32Value(4U), Tint = new DoubleValue(0.39997558519241921) }
                }
            },
            // 2 - Footer Border
            new Border
            {
                LeftBorder = new LeftBorder(),
                RightBorder = new RightBorder(),
                TopBorder = new TopBorder
                {
                    Style = new EnumValue<BorderStyleValues>(BorderStyleValues.Double),
                    Color = new Color { Theme = new UInt32Value(4U) }
                },
                BottomBorder = new BottomBorder
                {
                    Style = new EnumValue<BorderStyleValues>(BorderStyleValues.Thin),
                    Color = new Color { Theme = new UInt32Value(4U), Tint = new DoubleValue(0.39997558519241921) }
                }
            }
        ];
    }
    public static CellFormat[] CreateCellStyleFormats()
    {
        return
        [
            // 0 - Default CellStyleFormat
            new CellFormat
            {
                NumberFormatId = new UInt32Value(0U),
                FontId = new UInt32Value(0U),
                BorderId = new UInt32Value(0U)
            },
            // 1 - Success CellStyleFormat
            new CellFormat
            {
                NumberFormatId = new UInt32Value(0U),
                FontId = new UInt32Value(3U),
                FillId = new UInt32Value(4U),
                BorderId = new UInt32Value(0U),
                ApplyNumberFormat = new BooleanValue(false),
                ApplyBorder = new BooleanValue(false),
                ApplyAlignment = new BooleanValue(false),
                ApplyProtection = new BooleanValue(false)
            },
            // 2 - Error CellStyleFormat
            new CellFormat
            {
                NumberFormatId = new UInt32Value(0U),
                FontId = new UInt32Value(4U),
                FillId = new UInt32Value(5U),
                BorderId = new UInt32Value(0U),
                ApplyNumberFormat = new BooleanValue(false),
                ApplyBorder = new BooleanValue(false),
                ApplyAlignment = new BooleanValue(false),
                ApplyProtection = new BooleanValue(false)
            },
            // 3 - Warning CellStyleFormat
            new CellFormat
            {
                NumberFormatId = new UInt32Value(0U),
                FontId = new UInt32Value(5U),
                FillId = new UInt32Value(6U),
                BorderId = new UInt32Value(0U),
                ApplyNumberFormat = new BooleanValue(false),
                ApplyBorder = new BooleanValue(false),
                ApplyAlignment = new BooleanValue(false),
                ApplyProtection = new BooleanValue(false)
            },
            // 4 - Info CellStyleFormat
            new CellFormat
            {
                NumberFormatId = new UInt32Value(0U),
                FontId = new UInt32Value(6U),
                FillId = new UInt32Value(7U),
                BorderId = new UInt32Value(0U),
                ApplyNumberFormat = new BooleanValue(false),
                ApplyBorder = new BooleanValue(false),
                ApplyAlignment = new BooleanValue(false),
                ApplyProtection = new BooleanValue(false)
            },
            // 5 - Dark CellStyleFormat
            new CellFormat
            {
                NumberFormatId = new UInt32Value(0U),
                FontId = new UInt32Value(6U),
                FillId = new UInt32Value(9U),
                BorderId = new UInt32Value(0U),
                ApplyNumberFormat = new BooleanValue(false),
                ApplyBorder = new BooleanValue(false),
                ApplyAlignment = new BooleanValue(false),
                ApplyProtection = new BooleanValue(false)
            },
            // 6 - Light CellStyleFormat
            new CellFormat
            {
                NumberFormatId = new UInt32Value(0U),
                FontId = new UInt32Value(7U),
                FillId = new UInt32Value(8U),
                BorderId = new UInt32Value(0U),
                ApplyNumberFormat = new BooleanValue(false),
                ApplyBorder = new BooleanValue(false),
                ApplyAlignment = new BooleanValue(false),
                ApplyProtection = new BooleanValue(false)
            }

        ];
    }

    public static void CreateHeaderCellFormats(IList<CellFormat> cellFormats)
    {
        // 0 - Default CellFormat
        cellFormats.Add(new CellFormat
        {
            NumberFormatId = new UInt32Value(0U),
            FontId = new UInt32Value(0U),
            FillId = new UInt32Value(0U),
            BorderId = new UInt32Value(0U),
            FormatId = new UInt32Value(0U)
        });
        // 1 - Header CellFormat 
        cellFormats.Add(new CellFormat
        {
            NumberFormatId = new UInt32Value(0U),
            FontId = new UInt32Value(1U),
            FillId = new UInt32Value(2U),
            BorderId = new UInt32Value(1U),
            FormatId = new UInt32Value(0U),
            ApplyFont = new BooleanValue(true),
            ApplyFill = new BooleanValue(true),
            ApplyBorder = new BooleanValue(true),
            ApplyAlignment = new BooleanValue(true),
            Alignment = new Alignment
            {
                Horizontal = new EnumValue<HorizontalAlignmentValues>(HorizontalAlignmentValues.Center)
            }
        });
    }

    public static void CreateHeaderCellFormats(IList<ExcelColumn> columns, IList<CellFormat> cellFormats)
    {
        CreateHeaderCellFormats(cellFormats);

        for (int i = 0; i < columns.Count; i++)
        {
            ExcelColumn column = columns[i];
            column.HeaderStyleId = 1;
        }
    }

    internal static void CreateTitleFormats(ExcelReportTemplate template, string? title, IEnumerable<ReportDetail> details, List<CellFormat> cellFormats)
    {
        if (template.HasHeader)
        {
            // 2 - Formato para el Título
            cellFormats.Add(new CellFormat
            {
                NumberFormatId = new UInt32Value(0U),
                FontId = new UInt32Value(8U),
                FillId = new UInt32Value(0U),
                BorderId = new UInt32Value(0U),
                FormatId = new UInt32Value(0U),
                ApplyFont = new BooleanValue(true),
                ApplyAlignment = new BooleanValue(true),
                Alignment = new Alignment
                {
                    Horizontal = new EnumValue<HorizontalAlignmentValues>(HorizontalAlignmentValues.Center)
                }
            });
        }
        if (!string.IsNullOrEmpty(title) || details.Any(d => !string.IsNullOrEmpty(d.Title)))
        {
            // 3 - Formato para los subtítulos
            cellFormats.Add(new CellFormat
            {
                NumberFormatId = new UInt32Value(0U),
                FontId = new UInt32Value(9U),
                FillId = new UInt32Value(0U),
                BorderId = new UInt32Value(0U),
                FormatId = new UInt32Value(0U),
                ApplyFont = new BooleanValue(true),
                ApplyAlignment = new BooleanValue(true),
                Alignment = new Alignment
                {
                    Horizontal = new EnumValue<HorizontalAlignmentValues>(HorizontalAlignmentValues.Center)
                }
            });
        }
    }

    public static void CreateFilterAndTitleFormats(ExcelReportTemplate template, IList<ExcelFilter> filters, IList<CellFormat> cellFormats)
    {
        if (template.HasHeader)
        {
            // Formato para el Título
            cellFormats.Add(new CellFormat
            {
                NumberFormatId = new UInt32Value(0U),
                FontId = new UInt32Value(8U),
                FillId = new UInt32Value(0U),
                BorderId = new UInt32Value(0U),
                FormatId = new UInt32Value(0U),
                ApplyFont = new BooleanValue(true),
                ApplyAlignment = new BooleanValue(true),
                Alignment = new Alignment
                {
                    Horizontal = new EnumValue<HorizontalAlignmentValues>(HorizontalAlignmentValues.Center)
                }
            });

            CellFormat headerCellFormat = new()
            {
                NumberFormatId = new UInt32Value(0U),
                FontId = new UInt32Value(1U),
                FillId = new UInt32Value(2U),
                BorderId = new UInt32Value(1U),
                FormatId = new UInt32Value(0U),
                ApplyFont = new BooleanValue(true),
                ApplyFill = new BooleanValue(true),
                ApplyBorder = new BooleanValue(true)
            };

            cellFormats.Add(headerCellFormat);

            Dictionary<int, CellFormat> filterCellFormats = [];

            foreach (ExcelFilter filter in filters)
            {
                CellFormat cellFormat;
                filter.HeaderStyleId = cellFormats.IndexOf(headerCellFormat);

                if (filter.ExcelFormat is not null)
                {
                    int formatId = filter.ExcelFormat.Id;

                    if (!filterCellFormats.TryGetValue(formatId, out CellFormat? value))
                    {
                        cellFormat = new CellFormat
                        {
                            NumberFormatId = new UInt32Value((uint)formatId),
                            FontId = new UInt32Value(0U),
                            FillId = new UInt32Value(0U),
                            BorderId = new UInt32Value(1U),
                            FormatId = new UInt32Value(0U),
                            ApplyFont = new BooleanValue(true),
                            ApplyFill = new BooleanValue(true),
                            ApplyBorder = new BooleanValue(true)
                        };

                        if (!filter.HasTextValue())
                        {
                            cellFormat.ApplyNumberFormat = new BooleanValue(true);
                        }

                        cellFormats.Add(cellFormat);
                    }
                    else
                    {
                        cellFormat = value;
                    }

                    filter.StyleId = cellFormats.IndexOf(cellFormat);
                }
            }
        }
    }

    internal static void CreateValueCellFormats(List<ExcelColumn> columns, List<CellFormat> cellFormats)
    {
        Dictionary<int, CellFormat[]> totalFormats = [];
        columns.ForEach(column =>
        {
            CellFormat evenTotalCellFormat;
            CellFormat oddTotalCellFormat;
            CellFormat evenCellFormat;
            CellFormat oddCellFormat;
            CellFormat headerCellFormat;

            if (column.ExcelFormat is not null)
            {
                int formatId = column.ExcelFormat.Id;

                if (!totalFormats.TryGetValue(formatId, out CellFormat[]? value))
                {
                    headerCellFormat = new CellFormat
                    {
                        NumberFormatId = new UInt32Value((uint)formatId),
                        FontId = new UInt32Value(1U),
                        FillId = new UInt32Value(2U),
                        BorderId = new UInt32Value(1U),
                        FormatId = new UInt32Value(0U),
                        ApplyFont = new BooleanValue(true),
                        ApplyFill = new BooleanValue(true),
                        ApplyBorder = new BooleanValue(true),
                        ApplyAlignment = new BooleanValue(true),
                        Alignment = new Alignment
                        {
                            Horizontal = new EnumValue<HorizontalAlignmentValues>(HorizontalAlignmentValues.Center),
                        }
                    };

                    evenTotalCellFormat = new CellFormat
                    {
                        NumberFormatId = new UInt32Value((uint)formatId),
                        FontId = new UInt32Value(2U),
                        FillId = new UInt32Value(0U),
                        BorderId = new UInt32Value(1U),
                        FormatId = new UInt32Value(0U),
                        ApplyFont = new BooleanValue(true),
                        ApplyFill = new BooleanValue(true),
                        ApplyBorder = new BooleanValue(true)
                    };

                    oddTotalCellFormat = new CellFormat
                    {
                        NumberFormatId = new UInt32Value((uint)formatId),
                        FontId = new UInt32Value(2U),
                        FillId = new UInt32Value(3U),
                        BorderId = new UInt32Value(1U),
                        FormatId = new UInt32Value(0U),
                        ApplyFont = new BooleanValue(true),
                        ApplyFill = new BooleanValue(true),
                        ApplyBorder = new BooleanValue(true)
                    };

                    evenCellFormat = new CellFormat
                    {
                        NumberFormatId = new UInt32Value((uint)formatId),
                        FontId = new UInt32Value(0U),
                        FillId = new UInt32Value(0U),
                        BorderId = new UInt32Value(1U),
                        FormatId = new UInt32Value(0U),
                        ApplyFont = new BooleanValue(true),
                        ApplyFill = new BooleanValue(true),
                        ApplyBorder = new BooleanValue(true)
                    };

                    oddCellFormat = new CellFormat
                    {
                        NumberFormatId = new UInt32Value((uint)formatId),
                        FontId = new UInt32Value(0U),
                        FillId = new UInt32Value(3U),
                        BorderId = new UInt32Value(1U),
                        FormatId = new UInt32Value(0U),
                        ApplyFont = new BooleanValue(true),
                        ApplyFill = new BooleanValue(true),
                        ApplyBorder = new BooleanValue(true)
                    };

                    if (!column.IsText())
                    {
                        evenTotalCellFormat.ApplyNumberFormat = new BooleanValue(true);
                        oddTotalCellFormat.ApplyNumberFormat = new BooleanValue(true);
                        evenCellFormat.ApplyNumberFormat = new BooleanValue(true);
                        oddCellFormat.ApplyNumberFormat = new BooleanValue(true);
                    }

                    CellFormat[] arrayFormats = [headerCellFormat, evenTotalCellFormat, oddTotalCellFormat, evenCellFormat, oddCellFormat];
                    totalFormats.Add(formatId, arrayFormats);
                    cellFormats.AddRange(arrayFormats);
                }
                else
                {
                    headerCellFormat = value[0];
                    evenTotalCellFormat = value[1];
                    oddTotalCellFormat = value[2];
                    evenCellFormat = value[3];
                    oddCellFormat = value[4];
                }
                column.HeaderStyleId = cellFormats.IndexOf(headerCellFormat);
                column.StyleId = cellFormats.IndexOf(evenCellFormat);
                column.AlternateStyleId = cellFormats.IndexOf(oddCellFormat);
                column.TotalStyleId = cellFormats.IndexOf(evenTotalCellFormat);
                column.AlternateTotalStyleId = cellFormats.IndexOf(oddTotalCellFormat);
            }
        });
    }

    internal static void CreateRowCellFormats(List<ExcelColumn> columns, IList<CellFormat> cellFormats)
    {
        Dictionary<int, CellFormat[]> totalFormats = [];
        columns.ForEach(column =>
        {
            CellFormat totalCellFormat;
            CellFormat cellFormat;

            if (column.ExcelFormat is not null)
            {
                int formatId = column.ExcelFormat.Id;

                if (!totalFormats.TryGetValue(formatId, out CellFormat[]? value))
                {
                    totalCellFormat = new CellFormat
                    {
                        NumberFormatId = new UInt32Value((uint)formatId),
                        FontId = new UInt32Value(1U),
                        FillId = new UInt32Value(2U),
                        BorderId = new UInt32Value(1U),
                        FormatId = new UInt32Value(0U),
                        ApplyFont = new BooleanValue(true),
                        ApplyFill = new BooleanValue(true),
                        ApplyBorder = new BooleanValue(true),
                        ApplyAlignment = new BooleanValue(true),
                        Alignment = new Alignment
                        {
                            Vertical = new EnumValue<VerticalAlignmentValues>(VerticalAlignmentValues.Top)
                        }
                    };

                    cellFormat = new CellFormat
                    {
                        NumberFormatId = new UInt32Value((uint)formatId),
                        FontId = new UInt32Value(1U),
                        FillId = new UInt32Value(2U),
                        BorderId = new UInt32Value(1U),
                        FormatId = new UInt32Value(0U),
                        ApplyFont = new BooleanValue(true),
                        ApplyFill = new BooleanValue(true),
                        ApplyBorder = new BooleanValue(true),
                        ApplyAlignment = new BooleanValue(true),
                        Alignment = new Alignment
                        {
                            Vertical = new EnumValue<VerticalAlignmentValues>(VerticalAlignmentValues.Top)
                        }
                    };

                    if (!column.IsText())
                    {
                        totalCellFormat.ApplyNumberFormat = new BooleanValue(true);
                        cellFormat.ApplyNumberFormat = new BooleanValue(true);
                    }

                    totalFormats.Add(formatId, [totalCellFormat, cellFormat]);
                    cellFormats.Add(totalCellFormat);
                    cellFormats.Add(cellFormat);
                }
                else
                {
                    totalCellFormat = value[0];
                    cellFormat = value[1];
                }

                column.TotalHeaderStyleId = cellFormats.IndexOf(totalCellFormat);
                column.HeaderStyleId = cellFormats.IndexOf(cellFormat);
            }
        });
    }

    internal static void CreateColumnCellFormats(List<ExcelColumn> columns, List<CellFormat> cellFormats)
    {
        Dictionary<int, CellFormat[]> totalFormats = [];
        columns.ForEach(column =>
        {
            CellFormat totalCellFormat;
            CellFormat cellFormat;

            if (column.ExcelFormat is not null)
            {
                int formatId = column.ExcelFormat.Id;

                if (!totalFormats.TryGetValue(formatId, out CellFormat[]? value))
                {
                    totalCellFormat = new CellFormat
                    {
                        NumberFormatId = new UInt32Value((uint)formatId),
                        FontId = new UInt32Value(1U),
                        FillId = new UInt32Value(2U),
                        BorderId = new UInt32Value(1U),
                        FormatId = new UInt32Value(0U),
                        ApplyFont = new BooleanValue(true),
                        ApplyFill = new BooleanValue(true),
                        ApplyBorder = new BooleanValue(true),
                        ApplyAlignment = new BooleanValue(true),
                        Alignment = new Alignment
                        {
                            Horizontal = new EnumValue<HorizontalAlignmentValues>(HorizontalAlignmentValues.Center),
                            WrapText = new BooleanValue(true)
                        }
                    };

                    cellFormat = new CellFormat
                    {
                        NumberFormatId = new UInt32Value((uint)formatId),
                        FontId = new UInt32Value(1U),
                        FillId = new UInt32Value(2U),
                        BorderId = new UInt32Value(1U),
                        FormatId = new UInt32Value(0U),
                        ApplyFont = new BooleanValue(true),
                        ApplyFill = new BooleanValue(true),
                        ApplyBorder = new BooleanValue(true),
                        ApplyAlignment = new BooleanValue(true),
                        Alignment = new Alignment
                        {
                            Horizontal = new EnumValue<HorizontalAlignmentValues>(HorizontalAlignmentValues.Center),
                        }
                    };

                    if (!column.IsText())
                    {
                        totalCellFormat.ApplyNumberFormat = new BooleanValue(true);
                        cellFormat.ApplyNumberFormat = new BooleanValue(true);
                    }
                    totalFormats.Add(formatId, [totalCellFormat, cellFormat]);
                    cellFormats.Add(totalCellFormat);
                    cellFormats.Add(cellFormat);
                }
                else
                {
                    totalCellFormat = value[0];
                    cellFormat = value[1];
                }

                column.TotalHeaderStyleId = cellFormats.IndexOf(totalCellFormat);
                column.HeaderStyleId = cellFormats.IndexOf(cellFormat);
            }
        });
    }

    public static void CreateBodyCellFormats(IList<ExcelColumn> columns, List<CellFormat> cellFormats)
    {
        Dictionary<int, CellFormat[]> bodyFormats = [];

        for (int i = 0; i < columns.Count; i++)
        {
            ExcelColumn column = columns[i];
            CellFormat evenCellFormat;
            CellFormat oddCellFormat;

            if (column.ExcelFormat is not null)
            {
                int formatId = column.ExcelFormat.Id;

                if (!bodyFormats.TryGetValue(formatId, out CellFormat[]? value))
                {
                    evenCellFormat = new CellFormat
                    {
                        NumberFormatId = new UInt32Value((uint)formatId),
                        FontId = new UInt32Value(0U),
                        FillId = new UInt32Value(0U),
                        BorderId = new UInt32Value(1U),
                        FormatId = new UInt32Value(0U),
                        ApplyFont = new BooleanValue(true),
                        ApplyFill = new BooleanValue(true),
                        ApplyBorder = new BooleanValue(true)
                    };

                    oddCellFormat = new CellFormat
                    {
                        NumberFormatId = new UInt32Value((uint)formatId),
                        FontId = new UInt32Value(0U),
                        FillId = new UInt32Value(3U),
                        BorderId = new UInt32Value(1U),
                        FormatId = new UInt32Value(0U),
                        ApplyFont = new BooleanValue(true),
                        ApplyFill = new BooleanValue(true),
                        ApplyBorder = new BooleanValue(true)
                    };

                    if (!column.IsText())
                    {
                        evenCellFormat.ApplyNumberFormat = new BooleanValue(true);
                        oddCellFormat.ApplyNumberFormat = new BooleanValue(true);
                    }

                    CellFormat[] arrayFormats = [evenCellFormat, oddCellFormat];
                    bodyFormats.Add(formatId, arrayFormats);
                    cellFormats.AddRange(arrayFormats);
                }
                else
                {
                    evenCellFormat = value[0];
                    oddCellFormat = value[1];
                }

                column.StyleId = cellFormats.IndexOf(evenCellFormat);
                column.AlternateStyleId = cellFormats.IndexOf(oddCellFormat);
            }
        }
    }

    public static void CreateConditionalCellFormats(IList<ExcelColumn> columns, List<CellFormat> cellFormats, CellFormat[] baseCellFormats)
    {
        Dictionary<int, CellFormat?[]> bodyFormats = [];

        foreach (ExcelColumn column in columns.Where(c => c.HasConditionalStyle))
        {
            CellFormat? successCellFormat = null;
            CellFormat? warningCellFormat = null;
            CellFormat? errorCellFormat = null;
            int successIndex = 0;
            int warningIndex = 0;
            int errorIndex = 0;

            if (column.ExcelFormat is not null)
            {
                int formatId = column.ExcelFormat.Id;

                if (!bodyFormats.TryGetValue(formatId, out CellFormat?[]? value))
                {
                    if (column.SuccessStyle.HasValue)
                    {
                        successIndex = ExcelColumn.GetExcelIndexStyle(column.SuccessStyle.Value);

                        if (successIndex > 0)
                        {
                            successCellFormat = (CellFormat)baseCellFormats[successIndex].Clone();
                            successCellFormat.NumberFormatId = new UInt32Value((uint)formatId);
                            successCellFormat.BorderId = new UInt32Value(1U);
                            successCellFormat.FormatId = new UInt32Value((uint)successIndex);
                            successCellFormat.ApplyFont = new BooleanValue(true);
                            successCellFormat.ApplyFill = new BooleanValue(true);
                            successCellFormat.ApplyBorder = new BooleanValue(true);
                        }
                    }

                    if (column.WarningStyle.HasValue)
                    {
                        warningIndex = ExcelColumn.GetExcelIndexStyle(column.WarningStyle.Value);

                        if (warningIndex > 0)
                        {
                            warningCellFormat = (CellFormat)baseCellFormats[warningIndex].Clone();
                            warningCellFormat.NumberFormatId = new UInt32Value((uint)formatId);
                            warningCellFormat.BorderId = new UInt32Value(1U);
                            warningCellFormat.FormatId = new UInt32Value((uint)warningIndex);
                            warningCellFormat.ApplyFont = new BooleanValue(true);
                            warningCellFormat.ApplyFill = new BooleanValue(true);
                            warningCellFormat.ApplyBorder = new BooleanValue(true);
                        }
                    }

                    if (column.ErrorStyle.HasValue)
                    {
                        errorIndex = ExcelColumn.GetExcelIndexStyle(column.ErrorStyle.Value);

                        if (errorIndex > 0)
                        {
                            errorCellFormat = (CellFormat)baseCellFormats[errorIndex].Clone();
                            errorCellFormat.NumberFormatId = new UInt32Value((uint)formatId);
                            errorCellFormat.BorderId = new UInt32Value(1U);
                            errorCellFormat.FormatId = new UInt32Value((uint)errorIndex);
                            errorCellFormat.ApplyFont = new BooleanValue(true);
                            errorCellFormat.ApplyFill = new BooleanValue(true);
                            errorCellFormat.ApplyBorder = new BooleanValue(true);
                        }
                    }
                    if (!column.IsText())
                    {
                        if (successCellFormat != null)
                        {
                            successCellFormat.ApplyNumberFormat = new BooleanValue(true);
                        }

                        if (warningCellFormat != null)
                        {
                            warningCellFormat.ApplyNumberFormat = new BooleanValue(true);
                        }

                        if (errorCellFormat != null)
                        {
                            errorCellFormat.ApplyNumberFormat = new BooleanValue(true);
                        }
                    }

                    CellFormat?[] arrayFormats = [successCellFormat, warningCellFormat, errorCellFormat];
                    bodyFormats.Add(formatId, arrayFormats);
                    cellFormats.AddRange(arrayFormats.Where(f => f is not null).ToArray()!);
                }
                else
                {
                    successCellFormat = value[0];
                    warningCellFormat = value[1];
                    errorCellFormat = value[2];
                }

                if (successCellFormat is not null && column.SuccessStyle.HasValue && column.SuccessStyle != ConditionalStyle.Default)
                {
                    column.SuccessStyleId = cellFormats.IndexOf(successCellFormat!);
                }

                if (errorCellFormat is not null && column.ErrorStyle.HasValue && column.ErrorStyle != ConditionalStyle.Default)
                {
                    column.ErrorStyleId = cellFormats.IndexOf(errorCellFormat!);
                }

                if (warningCellFormat is not null && column.WarningStyle.HasValue && warningCellFormat != null && column.WarningStyle != ConditionalStyle.Default)
                {
                    column.WarningStyleId = cellFormats.IndexOf(warningCellFormat!);
                }
            }
        }
    }

    public static void CreateFooterCellFormats(IList<ExcelColumn> columns, List<CellFormat> cellFormats)
    {
        if (columns.Any(c => c.AggregationType.HasValue))
        {
            Dictionary<int, CellFormat> footerFotmats = [];

            for (int i = 0; i < columns.Count; i++)
            {
                ExcelColumn column = columns[i];

                if (column.ExcelFormat is not null)
                {
                    int formatId = column.ExcelFormat.Id;
                    CellFormat cellFormat;

                    if (!footerFotmats.TryGetValue(formatId, out CellFormat? value))
                    {
                        if (column.AggregationType.HasValue)
                        {
                            cellFormat = new CellFormat
                            {
                                NumberFormatId = new UInt32Value((uint)formatId),
                                FontId = new UInt32Value(2U),
                                FillId = new UInt32Value(0U),
                                BorderId = new UInt32Value(2U),
                                FormatId = new UInt32Value(0U),
                                ApplyFont = new BooleanValue(true),
                                ApplyFill = new BooleanValue(true),
                                ApplyBorder = new BooleanValue(true)
                            };

                            if (!column.IsText())
                            {
                                cellFormat.ApplyNumberFormat = new BooleanValue(true);
                            }
                        }
                        else
                        {
                            cellFormat = new CellFormat
                            {
                                NumberFormatId = new UInt32Value((uint)0),
                                FontId = new UInt32Value(2U),
                                FillId = new UInt32Value(0U),
                                BorderId = new UInt32Value(2U),
                                FormatId = new UInt32Value(0U),
                                ApplyFont = new BooleanValue(true),
                                ApplyFill = new BooleanValue(true),
                                ApplyBorder = new BooleanValue(true)
                            };
                        }

                        footerFotmats.Add(formatId, cellFormat);
                        cellFormats.Add(cellFormat);
                    }
                    else
                    {
                        cellFormat = value;
                    }

                    column.FooterStyleId = cellFormats.IndexOf(cellFormat);
                }
            }
        }
    }

    public static CellStyle[] CreateCellStyles()
    {
        return
        [
            new CellStyle
            {
                Name = new StringValue("20% Énfasis3"),
                FormatId = new UInt32Value(6U),
                BuiltinId = new UInt32Value(38U)
            },
            new CellStyle
            {
                Name = new StringValue("Bueno"),
                FormatId = new UInt32Value(1U),
                BuiltinId = new UInt32Value(26U)
            },
            new CellStyle
            {
                Name = new StringValue("Énfasis1"),
                FormatId = new UInt32Value(4U),
                BuiltinId = new UInt32Value(29U)
            },
            new CellStyle
            {
                Name = new StringValue("Énfasis3"),
                FormatId = new UInt32Value(5U),
                BuiltinId = new UInt32Value(37U)
            },
            new CellStyle
            {
                Name = new StringValue("Incorrecto"),
                FormatId = new UInt32Value(2U),
                BuiltinId = new UInt32Value(27U)
            },
            new CellStyle
            {
                Name = new StringValue("Neutral"),
                FormatId = new UInt32Value(3U),
                BuiltinId = new UInt32Value(28U)
            },
            new CellStyle
            {
                Name = new StringValue("Normal"),
                FormatId = new UInt32Value(0U),
                BuiltinId = new UInt32Value(0U)
            }
        ];
    }

    public static TableStyles CreateTableStyles()
    {
        return new TableStyles
        {
            Count = new UInt32Value(0U),
            DefaultTableStyle = new StringValue("TableStyleMedium2"),
            DefaultPivotStyle = new StringValue("PivotStyleLight16")
        };
    }

    public static DifferentialFormat[] CreateDifferentialFormats() => [];

    internal static CellFormat[] CreateCellFormats(ExcelReportTemplate template, IList<ExcelColumn> columns, IList<ExcelFilter> filters, CellFormat[] baseCellFormats)
    {
        List<CellFormat> cellFormats = [];
        CreateHeaderCellFormats(columns, cellFormats);
        CreateFilterAndTitleFormats(template, filters, cellFormats);
        CreateBodyCellFormats(columns, cellFormats);
        CreateConditionalCellFormats(columns, cellFormats, baseCellFormats);
        CreateFooterCellFormats(columns, cellFormats);
        return [.. cellFormats];
    }

    internal static CellFormat[] CreateCellFormats(ExcelReportTemplate template, IList<ExcelFilter> filters, List<ExcelColumn> rowColumns, List<ExcelColumn> columnColumns, List<ExcelColumn> valueColums, CellFormat[] baseCellFormats)
    {
        List<CellFormat> cellFormats = new();
        CreateHeaderCellFormats(cellFormats);
        CreateFilterAndTitleFormats(template, filters, cellFormats);
        CreateColumnCellFormats(columnColumns, cellFormats);
        CreateRowCellFormats(rowColumns, cellFormats);
        CreateValueCellFormats(valueColums, cellFormats);
        CreateConditionalCellFormats(valueColums, cellFormats, baseCellFormats);
        return cellFormats.ToArray();
    }
}
