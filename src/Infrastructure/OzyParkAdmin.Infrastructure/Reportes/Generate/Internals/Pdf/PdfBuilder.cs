using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Filters;
using OzyParkAdmin.Domain.Reportes.Pdf;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Xml;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf;
internal sealed class PdfBuilder<TReport> : PdfBuilderBase<TReport>
    where TReport : Report
{
    protected override string CreateHtml(TReport report, ReportFilter reportFilter, PdfReportTemplate template, DataTable dataTable, ClaimsPrincipal user)
    {
        List<ColumnBase> columns = [.. report.Columns.Cast<ColumnBase>().Where(column => column.IsAccessibleByUser(user)).OrderBy(c => c.Order)];
        List<DataRow> rows = [.. dataTable.AsEnumerable()];
        List<Filter> filters = [.. report.Filters.OrderBy(f => f.Order)];
        IDictionary<string, object?>? totals = rows.Aggregate(columns);
        return CreateHtml(report, reportFilter, template, columns, rows, filters, totals);
    }

    private static string CreateHtml(TReport report, ReportFilter reportFilter, PdfReportTemplate template, List<ColumnBase> columns, List<DataRow> rows, List<Filter> filters, IDictionary<string, object?>? totals)
    {
        using MemoryStream ms = new();

        XmlWriter xmlWriter = XmlWriter.Create(ms, new XmlWriterSettings
        {
            Indent = false,
            OmitXmlDeclaration = true,
            CloseOutput = false,
            CheckCharacters = false
        });

        WriteHtmlTitleAndFilter(xmlWriter, report, filters, reportFilter, template);

        xmlWriter.WriteStartElement("table");
        xmlWriter.WriteAttributeString("class", "table table-bordered table-striped table-collapse");

        if (template.RepeatHeaderInEachPage)
        {
            xmlWriter.WriteStartElement("thead");
        }
        xmlWriter.WriteStartElement("tr");

        foreach (ColumnBase column in columns)
        {
            xmlWriter.WriteStartElement("th");
            xmlWriter.WriteString(column.Header);
            xmlWriter.WriteEndElement();
        }

        xmlWriter.WriteEndElement();

        if (template.RepeatHeaderInEachPage)
        {
            xmlWriter.WriteEndElement();
        }

        xmlWriter.WriteStartElement("tbody");

        int index = 0;

        rows.ForEach(row =>
        {
            xmlWriter.WriteStartElement("tr");

            columns.ForEach(column =>
            {
                StringBuilder itemStyle = new();

                if (column.IsNumericType() || column.IsDateType() || column.Type == DbType.Time)
                {
                    _ = itemStyle.Append("text-align:right;");
                }

                xmlWriter.WriteStartElement("td");

                if (column.HasConditionalStyle)
                {
                    if (column.SuccessStyle is not null && column.EvaluateSuccessCondition(column.Type, row[column.Name]))
                    {
                        if (column.TryGenerateSuccessCssStyle(out string? successClass))
                        {
                            xmlWriter.WriteAttributeString("class", successClass);
                        }
                    }
                    else if (column.WarningStyle is not null && column.EvaluateWarningCondition(column.Type, row[column.Name]))
                    {
                        if (column.TryGenerateWarningCssStyle(out string? warningClass))
                        {
                            xmlWriter.WriteAttributeString("class", warningClass);
                        }
                    }
                    else
                    {
                        if (column.TryGenerateErrorCssStyle(out string? errorClass))
                        {
                            xmlWriter.WriteAttributeString("class", errorClass);
                        }
                    }
                }

                if (itemStyle.Length > 0)
                {
                    xmlWriter.WriteAttributeString("style", itemStyle.ToString());
                }

                xmlWriter.WriteString(FormatValue(row[column.Name], column));
                xmlWriter.WriteEndElement();
            });

            xmlWriter.WriteEndElement();
            index++;
        });

        xmlWriter.WriteEndElement();

        if (totals?.Any() == true)
        {
            if (template.RepeatFooterInEachPage)
            {
                xmlWriter.WriteStartElement("tfoot");
            }

            xmlWriter.WriteStartElement("tr");

            columns.ForEach(column =>
            {
                StringBuilder footerStyle = new();

                if (column.IsNumericType() || column.IsDateType() || column.Type == DbType.Time)
                {
                    _ = footerStyle.Append("text-align:right;");
                }

                xmlWriter.WriteStartElement("th");
                xmlWriter.WriteAttributeString("style", footerStyle.ToString());

                if (totals.TryGetValue(column.Name, out object? value))
                {
                    xmlWriter.WriteString(FormatValue(value, column));
                }

                xmlWriter.WriteEndElement();
            });

            xmlWriter.WriteEndElement();

            if (template.RepeatFooterInEachPage)
            {
                xmlWriter.WriteEndElement();
            }
        }

        xmlWriter.WriteEndElement();

        WriteEndHtml(xmlWriter);

        xmlWriter.Flush();

        return Encoding.UTF8.GetString(ms.ToArray());
    }

    private static string FormatValue(object? value, ColumnBase column)
    {
        return value switch
        {
            null => string.Empty,
            _ => !string.IsNullOrEmpty(column.Format) ? string.Format($"{{0:{column.Format}}}", value) : value.ToString() ?? string.Empty,
        };
    }
}
