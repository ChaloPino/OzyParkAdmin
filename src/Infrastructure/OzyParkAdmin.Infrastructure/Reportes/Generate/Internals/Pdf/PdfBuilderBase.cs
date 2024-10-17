using iText.Html2pdf.Resolver.Font;
using iText.Html2pdf;
using iText.IO.Font;
using iText.Kernel.Pdf;
using iText.StyledXmlParser.Css.Media;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Filters;
using OzyParkAdmin.Domain.Reportes.Pdf;
using System.Data;
using System.Text;
using System.Xml;
using iText.Kernel.Geom;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.Fonts;
using System.Security.Claims;
namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf;

internal abstract class PdfBuilderBase<TReport>
    where TReport : Report
{
    public byte[]? Build(TReport report, ReportFilter reportFilter, PdfReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        string? html = CreateHtml(report, reportFilter, template, dataSet, user);

        return html is not null ? ConvertToPdf(html, template) : null;
    }

    public byte[]? Build(TReport report, ReportFilter reportFilter, PdfReportTemplate template, DataTable dataTable, ClaimsPrincipal user)
    {
        string? html = CreateHtml(report, reportFilter, template, dataTable, user);
        return html is not null ? ConvertToPdf(html, template) : null;
    }

    protected virtual string? CreateHtml(TReport report, ReportFilter reportFilter, PdfReportTemplate template, DataTable dataTable, ClaimsPrincipal user)
    {
        return null;
    }

    protected virtual string? CreateHtml(TReport report, ReportFilter reportFilter, PdfReportTemplate template, DataSet dataSet, ClaimsPrincipal user)
    {
        return null;
    }

    private static void WriteHtmlHeader(XmlWriter xmlWriter, TReport report, PdfReportTemplate template)
    {
        xmlWriter.WriteStartElement("html");
        xmlWriter.WriteStartElement("head");

        xmlWriter.WriteStartElement("title");
        xmlWriter.WriteString(template.HeaderTitle ?? report.Aka);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("meta");
        xmlWriter.WriteAttributeString("name", "description");

        if (template.HasHeader)
        {
            xmlWriter.WriteAttributeString("content", $"Reporte {template.HeaderTitle}");
        }

        xmlWriter.WriteEndElement();

        StringBuilder style = new();
        // Page
        _ = style.AppendLine("@page {");

        if (template.Margin.Left is not null)
        {
            _ = style.AppendLine($"margin-left:{template.Margin.Left}pt;");
        }

        if (template.Margin.Top is not null)
        {
            _ = style.AppendLine($"margin-top:{template.Margin.Top}pt;");
        }

        if (template.Margin.Right is not null)
        {
            _ = style.AppendLine($"margin-right:{template.Margin.Right}pt;");
        }

        if (template.Margin.Bottom is not null)
        {
            _ = style.AppendLine($"margin-botom:{template.Margin.Bottom}pt;");
        }
        _ = style.AppendLine("}");

        // Body
        _ = style.AppendLine("body {");
        _ = style.AppendLine("font-family:Calibri;");
        _ = style.AppendLine("font-size:11pt;");
        _ = style.AppendLine("}");

        // Title
        _ = style.AppendLine("div.title {");
        _ = style.AppendLine("width:100%;");
        _ = style.AppendLine("text-align:center;");
        if (template.TitleFontSize.HasValue)
        {
            _ = style.AppendLine($"font-size:{template.TitleFontSize}pt;");
        }

        _ = style.AppendLine("font-weight:bold;");
        _ = style.AppendLine("}");

        // .table-collapse
        _ = style.AppendLine("table.table-collapse {");
        _ = style.AppendLine("border-collapse:collapse;");
        _ = style.AppendLine("}");

        // .table-striped
        _ = style.AppendLine(".table-striped tbody tr:nth-of-type(odd) {");
        _ = style.AppendLine("background-color:rgba(217, 225, 242, 0.5);");
        _ = style.AppendLine("}");

        // .table thead th .table tbody th
        _ = style.AppendLine(".table thead th, .table tbody th {");
        _ = style.AppendLine("background-color:rgba(68, 114, 196, 0.5);");

        if (template.HeaderFontSize.HasValue)
        {
            _ = style.AppendLine($"font-size:{template.HeaderFontSize}pt;");
        }

        _ = style.AppendLine("font-weight:bold;");
        _ = style.AppendLine("color:white;");
        _ = style.AppendLine("}");

        // .table thead th
        _ = style.AppendLine(".table thead th {");
        _ = style.AppendLine("vertical-align:bottom;");
        _ = style.AppendLine("text-align:center;");
        _ = style.AppendLine("}");

        // .table tbody th
        _ = style.AppendLine(".table tbody th {");
        _ = style.AppendLine("vertical-align:top;");
        _ = style.AppendLine("}");

        // .table tbody td
        if (template.RowFontSize.HasValue)
        {
            _ = style.AppendLine(".table tbody td {");
            _ = style.AppendLine($"font-size:{template.RowFontSize}pt;");
            _ = style.AppendLine("}");
        }

        // .table tfoot th
        _ = style.AppendLine(".table tfoot th {");

        _ = style.AppendLine("vertical-align:bottom;");

        if (template.FooterFontSize.HasValue)
        {
            _ = style.AppendLine($"font-size:{template.FooterFontSize}pt;");
        }
        _ = style.AppendLine("font-weight:bold;");
        _ = style.AppendLine("}");

        // .table-tab th
        _ = style.AppendLine(".table-tab th {");
        _ = style.AppendLine("text-align:left;");
        _ = style.AppendLine("background-color:rgba(68, 114, 196, 0.5);");

        if (template.FilterHeaderFontSize.HasValue)
        {
            _ = style.AppendLine($"font-size:{template.FilterHeaderFontSize}pt;");
        }

        _ = style.AppendLine("font-weight:bold;");
        _ = style.AppendLine("color:white;");
        _ = style.AppendLine("}");

        // .table-tab td
        if (template.FilterFontSize.HasValue)
        {
            _ = style.AppendLine(".table-tab td {");
            _ = style.AppendLine($"font-size:{template.FilterFontSize}pt;");
            _ = style.AppendLine("}");
        }

        // .table-bordered thead th, .table-bordered thead td, .table-bordered tbody th, .table-bordered tbody td
        _ = style.AppendLine(".table-bordered thead th, .table-bordered thead td, .table-bordered tbody th, .table-bordered tbody td {");
        _ = style.AppendLine("border-style:solid;");
        _ = style.AppendLine("border-width:1px;");
        _ = style.AppendLine("border-color:rgba(142, 169, 219, 0.5);");
        _ = style.AppendLine("}");

        // .table-bordered tfoot th, .table-bordered tfoot td
        _ = style.AppendLine(".table-bordered tfoot th, .table-bordered tfoot td {");
        _ = style.AppendLine("border-top-style:double;");
        _ = style.AppendLine("border-bottom-style:solid;");
        _ = style.AppendLine("border-bottom-width:1px;");
        _ = style.AppendLine("border-color:rgba(142, 169, 219, 0.5);");
        _ = style.AppendLine("}");

        //.totalValue
        _ = style.AppendLine(".totalValue, .subtotalValue {");
        _ = style.AppendLine("font-weight:bold;");
        _ = style.AppendLine("}");

        //.dt-body-right
        _ = style.AppendLine(".dt-body-right {");
        _ = style.AppendLine("text-align:right;");
        _ = style.AppendLine("}");

        // .text-white
        _ = style.AppendLine(".text-white {");
        _ = style.AppendLine("color:#fff!important;");
        _ = style.AppendLine("}");

        // .text-dark
        _ = style.AppendLine(".text-dark {");
        _ = style.AppendLine("color:#343a40!important;");
        _ = style.AppendLine("}");

        // .bg-success
        _ = style.AppendLine(".bg-success {");
        _ = style.AppendLine("background-color:#28a745!important;");
        _ = style.AppendLine("}");

        // .bg-danger
        _ = style.AppendLine(".bg-danger {");
        _ = style.AppendLine("background-color:#dc3545!important;");
        _ = style.AppendLine("}");

        // .bg-warning
        _ = style.AppendLine(".bg-warning {");
        _ = style.AppendLine("background-color:#ffc107!important;");
        _ = style.AppendLine("}");

        // .bg-info
        _ = style.AppendLine(".bg-info {");
        _ = style.AppendLine("background-color:#17a2b8!important;");
        _ = style.AppendLine("}");

        // .bg-light
        _ = style.AppendLine(".bg-light {");
        _ = style.AppendLine("background-color:#f8f9fa!important;");
        _ = style.AppendLine("}");

        // .bg-dark
        _ = style.AppendLine(".bg-dark {");
        _ = style.AppendLine("background-color:#343a40!important;");
        _ = style.AppendLine("}");

        // .text-right
        _ = style.Append(".text-right {");
        _ = style.AppendLine("text-align:right!important;");
        _ = style.AppendLine("}");

        xmlWriter.WriteStartElement("style");
        xmlWriter.WriteString(style.ToString());
        xmlWriter.WriteEndElement();

        xmlWriter.WriteEndElement();
    }

    protected static void WriteHtmlTitle(XmlWriter xmlWriter, TReport report, PdfReportTemplate template)
    {
        PdfBuilderBase<TReport>.WriteHtmlHeader(xmlWriter, report, template);
        xmlWriter.WriteStartElement("body");

        if (template.HasHeader)
        {
            xmlWriter.WriteStartElement("div");
            xmlWriter.WriteAttributeString("class", "title");
            xmlWriter.WriteString(template.HeaderTitle);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("br");
            xmlWriter.WriteEndElement();
        }
    }

    protected static void WriteHtmlTitleAndFilter(XmlWriter xmlWriter, TReport report, List<Filter> filters, ReportFilter reportFilter, PdfReportTemplate template)
    {
        PdfBuilderBase<TReport>.WriteHtmlHeader(xmlWriter, report, template);
        xmlWriter.WriteStartElement("body");

        if (template.HasHeader)
        {
            xmlWriter.WriteStartElement("div");
            xmlWriter.WriteAttributeString("class", "title");
            xmlWriter.WriteString(template.HeaderTitle);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("br");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("table");
            xmlWriter.WriteAttributeString("class", "table-tab table-collapse table-bordered");

            filters.ForEach(filter =>
            {
                xmlWriter.WriteStartElement("tr");

                xmlWriter.WriteStartElement("th");
                xmlWriter.WriteString(filter.Label);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("td");
                xmlWriter.WriteString(filter.GetFormattedText(reportFilter.GetFilter(filter.Id)));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            });

            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("br");
            xmlWriter.WriteEndElement();
        }
    }

    protected static void WriteEndHtml(XmlWriter xmlWriter)
    {
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
    }

    private static byte[] ConvertToPdf(string html, PdfReportTemplate template)
    {
        using MemoryStream ms = new();

        PdfWriter writer = new(ms);
        PdfDocument pdf = new(writer);
        ConverterProperties properties = new();
        MediaDeviceDescription mediaDeviceDescription = new(MediaType.SCREEN);

        if (template.Orientation == PdfOrientation.Landscape)
        {
            pdf.SetDefaultPageSize(PageSize.A3.Rotate());
        }
        else
        {
            pdf.SetDefaultPageSize(PageSize.LETTER);
        }

        _ = properties.SetMediaDeviceDescription(mediaDeviceDescription);
        DefaultFontProvider fontProvider = new(true, true, false);
        _ = fontProvider.AddFont(CalibriFonts.Calibri, PdfEncodings.WINANSI);
        _ = fontProvider.AddFont(CalibriFonts.CalibriBold, PdfEncodings.WINANSI);
        _ = properties.SetFontProvider(fontProvider);

        HtmlConverter.ConvertToPdf(html, pdf, properties);
        return ms.ToArray();
    }
}