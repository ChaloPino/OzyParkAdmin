using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.MasterDetails;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Xml;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf.MasterDetail;

internal sealed class MasterDetailHtmlWriter(TextWriter writer)
{
    private readonly TextWriter _writer = writer;

    public Action<XmlWriter>? OnPreWrite { get; set; }

    public void Write(MasterDetailReport report, DataSet dataSet, ClaimsPrincipal user)
    {
        XmlWriter xmlWriter = XmlWriter.Create(_writer, new XmlWriterSettings
        {
            Indent = false,
            OmitXmlDeclaration = true,
            CloseOutput = false,
            CheckCharacters = false
        });

        using (xmlWriter)
        {
            OnPreWrite?.Invoke(xmlWriter);

            DataTable? dataTable = ExtractDataTable(dataSet, report.MasterResultIndex, report.DataSource.Name);

            if (dataTable is not null)
            {
                xmlWriter.WriteStartElement("div");
                WriteTable(xmlWriter, report.IsTabular, report.TitleInReport, dataTable, user, report.Columns);

                if (report.HasDetail)
                {
                    if (report.HasDynamicDetails && report.Details is not null)
                    {
                        WriteDynamic(xmlWriter, dataSet, report.Details, user);
                    }
                    else if (report.Details is not null)
                    {
                        foreach (ReportDetail detail in report.Details)
                        {
                            xmlWriter.WriteStartElement("br");
                            xmlWriter.WriteEndElement();
                            dataTable = ExtractDataTable(dataSet, detail.DetailResultSetIndex, detail.DetailDataSource?.Name);

                            if (dataTable is not null)
                            {
                                WriteTable(xmlWriter, detail.IsTabular, detail.Title, dataTable, user, detail.DetailColumns);
                            }
                        }
                    }
                }

                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
        }
    }

    private static void WriteDynamic(XmlWriter writer, DataSet dataSet, IEnumerable<ReportDetail> details, ClaimsPrincipal user)
    {
        List<ReportDetail> orderedDetails = [.. details.OrderBy(d => d.DetailId)];

        for (int i = 1; i < dataSet.Tables.Count; i++)
        {
            int index = i - 1;
            ReportDetail reportDetail = orderedDetails[index % orderedDetails.Count];
            writer.WriteStartElement("br");
            writer.WriteEndElement();
            WriteTable(writer, reportDetail.IsTabular, reportDetail.Title, dataSet.Tables[i], user, reportDetail.DetailColumns);
        }
    }

    private static void WriteTable(XmlWriter writer, bool isTabular, string? title, DataTable dataTable, ClaimsPrincipal user, IEnumerable<ColumnBase> columns)
    {
        if (!string.IsNullOrEmpty(title))
        {
            writer.WriteStartElement("h3");
            writer.WriteString(title);
            writer.WriteEndElement();
        }

        writer.WriteStartElement("div");
        writer.WriteAttributeString("class", "table-responsive");
        writer.WriteStartElement("table");

        if (isTabular)
        {
            writer.WriteAttributeString("class", "table-striped table-bordered table-hover table-collapse table-sm");
        }
        else
        {
            writer.WriteAttributeString("class", "table table-bordered table-hover table-striped table-collapse table-sm");
        }

        if (isTabular)
        {
            WriteTabular(writer, dataTable, user, columns);
        }
        else
        {
            WriteDetail(writer, dataTable, user, columns);
        }

        writer.WriteEndElement();
        writer.WriteEndElement();
    }

    private static void WriteTabular(XmlWriter writer, DataTable dataTable, ClaimsPrincipal user, IEnumerable<ColumnBase> columns)
    {
        List<ColumnBase> orderedColumns = [.. columns.Where(c => c.IsAccessibleByUser(user)).OrderBy(c => c.Order)];

        orderedColumns.ForEach(column =>
        {
            writer.WriteStartElement("tr");

            writer.WriteStartElement("th");
            writer.WriteString(column.Header);
            writer.WriteEndElement();

            writer.WriteStartElement("td");
            object value = dataTable.Rows[0][column.Name];
            string style = GetStylePerColumn(column, value);
            if (!string.IsNullOrEmpty(style))
            {
                writer.WriteAttributeString("class", style);
            }
            writer.WriteString(FormatValue(value, column));
            writer.WriteEndElement();

            writer.WriteEndElement();
        });
    }

    private static void WriteDetail(XmlWriter writer, DataTable dataTable, ClaimsPrincipal user, IEnumerable<ColumnBase> columns)
    {
        List<ColumnBase> orderedColumns = [.. columns.Where(c => c.IsAccessibleByUser(user)).OrderBy(c => c.Order)];

        writer.WriteStartElement("thead");
        writer.WriteStartElement("tr");

        orderedColumns.ForEach(column =>
        {
            writer.WriteStartElement("th");
            writer.WriteString(column.Header);
            writer.WriteEndElement();
        });

        writer.WriteEndElement();
        writer.WriteEndElement();

        writer.WriteStartElement("tbody");
        dataTable.AsEnumerable().ToList().ForEach(row =>
        {
            writer.WriteStartElement("tr");

            orderedColumns.ForEach(column =>
            {
                writer.WriteStartElement("td");
                object value = row[column.Name];
                string style = GetStylePerColumn(column, value);
                if (!string.IsNullOrEmpty(style))
                {
                    writer.WriteAttributeString("class", style);
                }
                writer.WriteString(FormatValue(value, column));
                writer.WriteEndElement();
            });

            writer.WriteEndElement();
        });

        writer.WriteEndElement();
    }

    private static string GetStylePerColumn(ColumnBase column, object value)
    {
        StringBuilder sb = new();

        if (column.HasConditionalStyle)
        {
            if (column.EvaluateSuccessCondition(column.Type, value))
            {
                if (column.TryGenerateSuccessCssStyle(out string? successClass))
                {
                    _ = sb.Append(successClass);
                }
            }
            else if (column.WarningStyle.HasValue && column.EvaluateWarningCondition(column.Type, value))
            {
                if (column.TryGenerateWarningCssStyle(out string? warningClass))
                {
                    _ = sb.Append(warningClass);
                }
            }
            else
            {
                if (column.TryGenerateErrorCssStyle(out string? errorClass))
                {
                    _ = sb.Append(errorClass);
                }
            }
        }

        if (column.IsNumericType() || column.IsDateType() || column.Type == DbType.Time)
        {
            if (sb.Length > 0)
            {
                _ = sb.Append(' ');
            }

            _ = sb.Append("text-right");
        }

        return sb.ToString();
    }

    private static string FormatValue(object value, ColumnBase column)
    {
        return value switch
        {
            null => string.Empty,
            _ => !string.IsNullOrEmpty(column.Format) ? string.Format(string.Concat("{0:", column.Format + "}"), value) : value.ToString() ?? string.Empty,
        };
    }

    private static DataTable? ExtractDataTable(DataSet dataSet, int? index, string? name) =>
        index switch
        {
            null => !string.IsNullOrEmpty(name) ? dataSet.Tables[name] : null,
            _ => dataSet.Tables[index.Value],
        };
}