using NReco.PivotData;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;
internal abstract class PivotTableWriterBase
{
    protected PivotTableWriterBase()
    {
        TotalsRow = true;
        TotalsColumn = true;
        GrandTotal = true;
        TotalsColumnHeaderText = "Total";
        TotalsRowHeaderText = "Total";
    }

    public bool GrandTotal { get; set; }
    public bool TotalsColumn { get; set; }
    public string TotalsColumnHeaderText { get; set; }
    public PivotTableTotalsPosition TotalsColumnPosition { get; set; }
    public bool TotalsRow { get; set; }
    public string TotalsRowHeaderText { get; set; }
    public PivotTableTotalsPosition TotalsRowPosition { get; set; }

    internal protected static int GetAggregatorsCount(IPivotTable pivotTable)
    {
        return pivotTable.PivotData.AggregatorFactory is not CompositeAggregatorFactory aggregator
            ? 1
            : aggregator.Factories.Length;
    }
}
