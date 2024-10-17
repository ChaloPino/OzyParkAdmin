using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Globalization;
using X15 = DocumentFormat.OpenXml.Office2013.Excel;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel;
internal class Style
{
    private const string Spreadsheetml = "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac";

    public IEnumerable<NumberingFormat>? NumberFormats { get; set; }
    public IEnumerable<Font>? Fonts { get; set; }
    public IEnumerable<Fill>? Fills { get; set; }
    public IEnumerable<Border>? Borders { get; set; }
    public IEnumerable<CellFormat>? CellStyleFormats { get; set; }
    public IEnumerable<CellFormat>? CellFormats { get; set; }
    public IEnumerable<CellStyle>? CellStyles { get; set; }
    public IEnumerable<DifferentialFormat>? DifferentialFormats { get; set; }
    public TableStyles? TableStyles { get; set; }

    internal void Write(WorkbookPart workbookPart)
    {
        WorkbookStylesPart stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();

        using OpenXmlWriter writer = OpenXmlWriter.Create(stylesPart);
        Stylesheet stylesheet = new()
        {
            MCAttributes = new MarkupCompatibilityAttributes { Ignorable = "x14ac x16r2" }
        };

        Dictionary<string, string> ns = new()
        {
            { "mc", "http://schemas.openxmlformats.org/markup-compatibility/2006" },
            { "x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac" },
            { "x16r2", "http://schemas.microsoft.com/office/spreadsheetml/2015/02/main" }
        };

        writer.WriteStartElement(stylesheet, null!, ns);

        WriteNumberFormats(writer);

        WriteFonts(writer);

        WriteFills(writer);

        WriteBorders(writer);

        WriteCellStyleFormats(writer);

        WriteCellFormats(writer);

        WriteCellStyles(writer);

        WriteDifferentialFormats(writer);

        WriteTableStyles(writer);

        WriteExtensionList(writer);

        writer.WriteEndElement();
    }

    private static void WriteExtensionList(OpenXmlWriter writer)
    {
        writer.WriteStartElement(new StylesheetExtensionList());

        WriteStylesheetExtension(writer, ("{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}", "x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main", new X14.SlicerStyles(), "defaultSlicerStyle", "SlicerStyleLight1"));
        WriteStylesheetExtension(writer, ("{9260A510-F301-46a8-8635-F512D64BE5F5}", "x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main", new X15.TimelineStyles(), "defaultTimelineStyle", "TimeSlicerStyleLight1"));

        writer.WriteEndElement();
    }

    private static void WriteStylesheetExtension(OpenXmlWriter writer, (string Id, string Prefix, string Uri, OpenXmlCompositeElement Style, string StyleName, string StyleValue) stylesheetExtension)
    {
        List<OpenXmlAttribute> oxa =
        [
            new OpenXmlAttribute("uri", null!, stylesheetExtension.Id)
        ];

        StylesheetExtension ext = new();
        ext.AddNamespaceDeclaration(stylesheetExtension.Prefix, stylesheetExtension.Uri);
        writer.WriteStartElement(ext, oxa);

        WriteExtension(writer, stylesheetExtension.Style, stylesheetExtension.StyleName, stylesheetExtension.StyleValue);

        writer.WriteEndElement();
    }

    private static void WriteExtension(OpenXmlWriter writer, OpenXmlCompositeElement style, string defaultStyleName, string defaultStyleValue)
    {
        List<OpenXmlAttribute> oxa = [new OpenXmlAttribute(defaultStyleName, null!, defaultStyleValue)];
        writer.WriteStartElement(style, oxa);
        writer.WriteEndElement();
    }

    private void WriteTableStyles(OpenXmlWriter writer)
    {
        if (TableStyles is not null)
        {
            writer.WriteElement(TableStyles);
        }
    }

    private void WriteDifferentialFormats(OpenXmlWriter writer)
    {
        if (DifferentialFormats is not null)
        {
            List<OpenXmlAttribute> oxa =
            [
                new OpenXmlAttribute("count", null!, DifferentialFormats.Count().ToString(CultureInfo.InvariantCulture))
            ];

            writer.WriteStartElement(new DifferentialFormats(), oxa);

            foreach (var differentialFormat in DifferentialFormats)
            {
                writer.WriteElement(differentialFormat);
            }

            writer.WriteEndElement();
        }
    }

    private void WriteCellStyles(OpenXmlWriter writer)
    {
        if (CellStyles is not null)
        {
            List<OpenXmlAttribute> oxa =
            [
                new OpenXmlAttribute("count", null!, CellStyles.Count().ToString(CultureInfo.InvariantCulture))
            ];

            writer.WriteStartElement(new CellStyles(), oxa);

            foreach (var cellStyle in CellStyles)
            {
                writer.WriteElement(cellStyle);
            }

            writer.WriteEndElement();
        }
    }

    private void WriteCellFormats(OpenXmlWriter writer)
    {
        if (CellFormats is not null)
        {
            List<OpenXmlAttribute> oxa =
            [
                new OpenXmlAttribute("count", null!, CellFormats.Count().ToString(CultureInfo.InvariantCulture))
            ];

            writer.WriteStartElement(new CellFormats(), oxa);

            foreach (var cellFormat in CellFormats)
            {
                writer.WriteElement(cellFormat);
            }

            writer.WriteEndElement();
        }
    }

    private void WriteCellStyleFormats(OpenXmlWriter writer)
    {
        if (CellStyleFormats is not null)
        {
            List<OpenXmlAttribute> oxa =
            [
                new OpenXmlAttribute("count", null!, CellStyleFormats.Count().ToString(CultureInfo.InvariantCulture))
            ];

            writer.WriteStartElement(new CellStyleFormats(), oxa);

            foreach (var cellFormat in CellStyleFormats)
            {
                writer.WriteElement(cellFormat);
            }

            writer.WriteEndElement();
        }
    }

    private void WriteBorders(OpenXmlWriter writer)
    {
        if (Borders is not null)
        {
            List<OpenXmlAttribute> oxa =
            [
                new OpenXmlAttribute("count", null!, Borders.Count().ToString(CultureInfo.InvariantCulture))
            ];

            writer.WriteStartElement(new Borders(), oxa);

            foreach (var border in Borders)
            {
                writer.WriteElement(border);
            }

            writer.WriteEndElement();
        }
    }

    private void WriteFills(OpenXmlWriter writer)
    {
        if (Fills is not null)
        {
            List<OpenXmlAttribute> oxa =
            [
                new OpenXmlAttribute("count", null!, Fills.Count().ToString(CultureInfo.InvariantCulture))
            ];

            writer.WriteStartElement(new Fills(), oxa);

            foreach (Fill fill in Fills)
            {
                writer.WriteElement(fill);
            }

            writer.WriteEndElement();
        }
    }

    private void WriteFonts(OpenXmlWriter writer)
    {
        if (Fonts is not null)
        {
            List<OpenXmlAttribute> oxa =
            [
                new OpenXmlAttribute("count", null!, Fonts.Count().ToString(CultureInfo.InvariantCulture)),
                new OpenXmlAttribute("x14ac", "knownfonts", Spreadsheetml, "1")
            ];

            writer.WriteStartElement(new Fonts(), oxa);

            foreach (Font font in Fonts)
            {
                writer.WriteElement(font);
            }

            writer.WriteEndElement();
        }
    }

    private void WriteNumberFormats(OpenXmlWriter writer)
    {
        if (NumberFormats is not null)
        {
            List<OpenXmlAttribute> oxa =
            [
                new OpenXmlAttribute("count", null!, NumberFormats.Count().ToString(CultureInfo.InvariantCulture))
            ];

            writer.WriteStartElement(new NumberingFormats(), oxa);

            foreach (NumberingFormat numFormat in NumberFormats)
            {
                writer.WriteElement(numFormat);
            }

            writer.WriteEndElement();
        }
    }
}
