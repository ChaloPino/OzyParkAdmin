namespace OzyParkAdmin.Domain.Reportes.Charts;

/// <summary>
/// Un reporte que contiente varios gráficos y tablas.
/// </summary>
/// <remarks>
/// Usados por ejemplo para construir dashboards.
/// </remarks>
public sealed class ChartReport : Report
{
    private readonly List<Chart> _charts = [];
    private ChartReport()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ChartReport"/>.
    /// </summary>
    /// <param name="aka">El aka del reporte.</param>
    public ChartReport(string aka)
        : base(ReportType.Chart, aka)
    {
    }

    /// <summary>
    /// El layout del dashboard.
    /// </summary>
    public string? Layout { get; private set; }

    /// <summary>
    /// Los gráficos por fila.
    /// </summary>
    public int? ChartsPerRow { get; private set; }

    /// <summary>
    /// Los gráficos.
    /// </summary>
    public IEnumerable<Chart> Charts => _charts;
}
