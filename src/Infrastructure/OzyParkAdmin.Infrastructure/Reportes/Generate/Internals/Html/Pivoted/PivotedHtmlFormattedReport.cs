using NReco.PivotData;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Pivoted;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html.Pivoted;
internal sealed class PivotedHtmlFormattedReport : HtmlFormattedReport
{
    public override ReportType Type => ReportType.Pivoted;
    public IPivotTable PivotTable { get; set; } = default!;
    public bool TotalsColum { get; set; }
    public bool TotalsRow { get; set; }
    public bool GrandTotal { get; set; }
    public bool SubtotalColumns { get; set; }
    public bool SubtotalRows { get; set; }
    public string[] SubtotalDimensions { get; set; } = [];

    internal static PivotedHtmlFormattedReport Create(IPivotTable pivotTable, PivotedReport report)
    {
        return new()
        {
            PivotTable = pivotTable,
            TotalsColum = report.IncludeTotalsColumn,
            TotalsRow = report.IncludeTotalsRow,
            GrandTotal = report.IncludeGrandTotal,
            SubtotalColumns = report.PivotedMembers.Any(pc => pc.PivotType == PivotType.Column && pc.ShowTotal == true),
            SubtotalRows = report.PivotedMembers.Any(pc => pc.PivotType == PivotType.Row && pc.ShowTotal == true),
            SubtotalDimensions = [.. report.PivotedMembers.Where(pm => pm.ShowTotal == true).Select(pm => pm.GetFullName())],
        };
    }

    public override ReportGenerated Generate() =>
        new()
        {
            Type = Type,
            Format = Format,
            PivotTable = PivotTable,
            TotalsColum = TotalsColum,
            TotalsRow = TotalsRow,
            GrandTotal = GrandTotal,
            SubtotalColumns = SubtotalColumns,
            SubtotalRows = SubtotalRows,
            SubtotalDimensions = SubtotalDimensions,
        };
}
