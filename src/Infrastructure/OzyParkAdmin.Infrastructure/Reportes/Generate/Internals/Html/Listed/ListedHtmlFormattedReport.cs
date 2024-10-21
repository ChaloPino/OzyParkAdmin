using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using System.Data;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.Listed;
internal class ListedHtmlFormattedReport : HtmlFormattedReport
{
    public override ReportType Type => ReportType.Listed;
    public IEnumerable<ColumnInfo> Columns { get; set; } = [];

    public IEnumerable<DataInfo> Data { get; set; } = [];

    internal static ListedHtmlFormattedReport Create(IEnumerable<DataRow> dataRows, IEnumerable<ColumnBase> columns)
    {
        List<ColumnInfo> columnsInfo = [];
        List<DataInfo> data = [];

        foreach (ColumnBase column in columns)
        {
            columnsInfo.Add(ColumnInfo.FromColumn(column));
        }

        foreach (var row in dataRows)
        {
            DataInfo dataInfo = new();

            foreach (ColumnInfo column in columnsInfo)
            {
                dataInfo[column] = row[column.Name];
            }

            data.Add(dataInfo);
        }

        return new() { Columns = columnsInfo,  Data = data };
    }

    public override ReportGenerated Generate() =>
        new()
        {
            Type = Type,
            Format = Format,
            Columns = Columns,
            Data = Data
        };
}
