namespace OzyParkAdmin.Domain.Reportes.Charts;

/// <summary>
/// La columna de una tabla que se presenta en un dashboard.
/// </summary>
public sealed class ChartTableColumn : ColumnBase<ChartTableColumn>
{
    private ChartTableColumn()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ChartTableColumn"/>.
    /// </summary>
    /// <param name="chart">El <see cref="Chart"/> al que pertenece la columa.</param>
    /// <param name="id">El identificador de la columna.</param>
    public ChartTableColumn(Chart chart, int id)
        : base(chart.ReportId, id)
    {
        ChartId = chart.Id;

    }

    /// <summary>
    /// El id del chart.
    /// </summary>
    public int ChartId { get; private set; }
}
