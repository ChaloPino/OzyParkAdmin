using OzyParkAdmin.Domain.Reportes.DataSources;

namespace OzyParkAdmin.Domain.Reportes.Charts;

/// <summary>
/// Un gráfico o tabla para un dashboard.
/// </summary>
public sealed class Chart : SecureComponent<Chart>
{
    private readonly List<ChartDataSet> _dataSets = [];
    private readonly List<ChartTableColumn> _columns = [];

    private Chart()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="Chart"/>.
    /// </summary>
    /// <param name="report">El <see cref="ChartReport"/> al que pertenece este gráfico.</param>
    /// <param name="id">El identificador del chart.</param>
    public Chart(ChartReport report, int id)
    {
        ReportId = report.Id;
        Id = id;
    }

    /// <summary>
    /// El id del reporte.
    /// </summary>
    public Guid ReportId { get; private set; }

    /// <summary>
    /// El id del chart.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// El nombre del chart.
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    /// El tipo del chart.
    /// </summary>
    public ChartType? Type { get; private set; }

    /// <summary>
    /// La fuente de datos para las etiquetas.
    /// </summary>
    public DataSource? LabelDataSource { get; private set; }

    /// <summary>
    /// El índice de la fuente de datos original para las etiquetas.
    /// </summary>
    public int? LabelDataSourceIndex { get; set; }

    /// <summary>
    /// Las etiquetas.
    /// </summary>
    public string? Labels { get; private set; }

    /// <summary>
    /// El nombre de la columna para las etiquetas.
    /// </summary>
    public string? LabelColumnName { get; private set; }

    /// <summary>
    /// La fuente de datos para la tabla.
    /// </summary>
    public DataSource? TableDataSource { get; private set; }

    /// <summary>
    /// El índice de la fuente de datos original para la tabla.
    /// </summary>
    public int? TableDataSourceIndex { get; private set; }

    /// <summary>
    /// Los datasets.
    /// </summary>
    public IEnumerable<ChartDataSet> DataSets => _dataSets;

    /// <summary>
    /// Las columnas de la tabla.
    /// </summary>
    public IEnumerable<ChartTableColumn> Columns => _columns;

    /// <summary>
    /// El parseo.
    /// </summary>
    public string? Parsing { get; private set; }

    /// <summary>
    /// La animación.
    /// </summary>
    public string? Animations { get; private set; }

    /// <summary>
    /// La decimación.
    /// </summary>
    public string? Decimation { get; private set; }

    /// <summary>
    /// La interacción.
    /// </summary>
    public string? Interaction { get; private set; }

    /// <summary>
    /// El layout del gráfico.
    /// </summary>
    public string? Layout { get; private set; }

    /// <summary>
    /// La leyenda.
    /// </summary>
    public string? Legend { get; private set; }

    /// <summary>
    /// Si es responsibo.
    /// </summary>
    public bool? Responsive { get; private set; }

    /// <summary>
    /// Si se mantiene el ratio de aspecto.
    /// </summary>
    public bool? MaintainAspectRatio { get; private set; }

    /// <summary>
    /// El ratio de aspecto.
    /// </summary>
    public double? AspectRatio { get; private set; }

    /// <summary>
    /// La demora del redimensionamiento.
    /// </summary>
    public int? ResizeDelay { get; private set; }

    /// <summary>
    /// El ancho.
    /// </summary>
    public string? Width { get; private set; }

    /// <summary>
    /// El alto.
    /// </summary>
    public string? Height { get; private set; }

    /// <summary>
    /// El tooltip para el chart.
    /// </summary>
    public string? Tooltip { get; private set; }

    /// <summary>
    /// Las etiquetas de datos.
    /// </summary>
    public string? DataLabels { get; private set; }

    /// <summary>
    /// El título.
    /// </summary>
    public string? Title { get; private set; }

    /// <summary>
    /// El subtítulo.
    /// </summary>
    public string? Subtitle { get; private set; }

    /// <summary>
    /// Las escalas.
    /// </summary>
    public string? Scales { get; private set; }
}
