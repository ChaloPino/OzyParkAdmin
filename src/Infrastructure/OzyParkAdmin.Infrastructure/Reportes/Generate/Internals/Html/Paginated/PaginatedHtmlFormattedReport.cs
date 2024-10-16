using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using System.Data;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.Paginated;
internal sealed class PaginatedHtmlFormattedReport : HtmlFormattedReport
{
    public override ReportType Type => ReportType.Paginated;

    /// <summary>
    /// La información de la columna.
    /// </summary>
    public IEnumerable<ColumnInfo> Columns { get; set; } = [];

    /// <summary>
    /// Los datos.
    /// </summary>
    public IEnumerable<DataInfo> Data { get; set; } = [];

    public long TotalRecords { get; set; }

    internal static PaginatedHtmlFormattedReport Create(IEnumerable<DataRow> dataRows, IEnumerable<ColumnBase> columns, long totalRecords)
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

        return new() { Columns = columnsInfo, Data = data, TotalRecords = totalRecords };
    }

    public override ReportGenerated Generate() =>
        new()
        {
            Type = Type,
            Format = Format,
            Columns = Columns,
            Data = Data,
            TotalRecords = TotalRecords,
        };
}
