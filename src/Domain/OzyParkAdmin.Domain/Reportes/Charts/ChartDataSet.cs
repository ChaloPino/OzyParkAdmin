using OzyParkAdmin.Domain.Reportes.DataSources;

namespace OzyParkAdmin.Domain.Reportes.Charts;

/// <summary>
/// El dataset de un chart.
/// </summary>
public sealed class ChartDataSet
{
    private ChartDataSet()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ChartDataSet"/>.
    /// </summary>
    /// <param name="chart">El <see cref="Chart"/> al que pertenece este dataset.</param>
    /// <param name="id">El identificador del dataset.</param>
    public ChartDataSet(Chart chart, int id)
    {
        ReportId = chart.ReportId;
        ChartId = chart.Id;
        Id = id;
    }

    /// <summary>
    /// El id del reporte.
    /// </summary>
    public Guid ReportId { get; private set; }

    /// <summary>
    /// El id del chart.
    /// </summary>
    public int ChartId { get; private set; }

    /// <summary>
    /// El id del dataset.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// El tipo del chart.
    /// </summary>
    public ChartType? Type { get; private set; }

    /// <summary>
    /// El color de fondo.
    /// </summary>
    public string? BackgroundColor { get; private set; }

    /// <summary>
    /// La base del datset.
    /// </summary>
    public int? Base { get; private set; }

    /// <summary>
    /// El porcentaje de la barra.
    /// </summary>
    public double? BarPercentage { get; private set; }

    /// <summary>
    /// El grosor de la barra.
    /// </summary>
    public string? BarThickness { get; private set; }

    /// <summary>
    /// El alineamiento del borde.
    /// </summary>
    public BorderAlign? BorderAlign { get; private set; }

    /// <summary>
    /// El estilo de límite del borde.
    /// </summary>
    public LineCap? BorderCapStyle { get; private set; }

    /// <summary>
    /// El color del borde.
    /// </summary>
    public string? BorderColor { get; private set; }

    /// <summary>
    /// El guión del borde.
    /// </summary>
    public string? BorderDash { get; private set; }

    /// <summary>
    /// El desplazamiento de guiones del borde.
    /// </summary>
    public double? BorderDashOffset { get; private set; }

    /// <summary>
    /// El estilo de unión del borde.
    /// </summary>
    public BorderJoinStyle? BorderJoinStyle { get; private set; }

    /// <summary>
    /// El saltado del borde.
    /// </summary>
    public BorderSkipped? BorderSkipped { get; private set; }

    /// <summary>
    /// El radio del borde.
    /// </summary>
    public string? BorderRadius { get; private set; }

    /// <summary>
    /// El ancho del borde.
    /// </summary>
    public string? BorderWidth { get; private set; }

    /// <summary>
    /// El porcentaje de la categoría.
    /// </summary>
    public double? CategoryPercentage { get; private set; }

    /// <summary>
    /// La circunferencia.
    /// </summary>
    public int? Circumference { get; private set; }

    /// <summary>
    /// El clip.
    /// </summary>
    public string? Clip { get; private set; }

    /// <summary>
    /// El modo de interpolación cúbica.
    /// </summary>
    public CubicInterpolationMode? CubicInterpolationMode { get; private set; }

    /// <summary>
    /// Si se dibujarán los elementos encima.
    /// </summary>
    public bool? DrawActiveElementsOnTop { get; private set; }

    /// <summary>
    /// El llenado.
    /// </summary>
    public string? Fill { get; private set; }

    /// <summary>
    /// Si estará agrupado.
    /// </summary>
    public bool? Grouped { get; private set; }

    /// <summary>
    /// El radio de impacto.
    /// </summary>
    public int? HitRadius { get; private set; }

    /// <summary>
    /// El color de fondo al pasar el cursor por encima.
    /// </summary>
    public string? HoverBackgroundColor { get; private set; }

    /// <summary>
    /// El estilo del límite del borde al pasar el cursor por encima.
    /// </summary>
    public LineCap? HoverBorderCapStyle { get; private set; }

    /// <summary>
    /// El color del borde al pasar el cursor por encima.
    /// </summary>
    public string? HoverBorderColor { get; private set; }

    /// <summary>
    /// Los guiones del borde al pasar el cursor por encima.
    /// </summary>
    public string? HoverBorderDash { get; private set; }

    /// <summary>
    /// El desplazamiento de los guiones del borde al pasar el cursor por encima.
    /// </summary>
    public double? HoverBorderDashOffset { get; private set; }

    /// <summary>
    /// El estilo de unión del borde al pasar el cursor por encima.
    /// </summary>
    public BorderJoinStyle? HoverBorderJoinStyle { get; private set; }

    /// <summary>
    /// El radio del borde al pasar el cursor por encima.
    /// </summary>
    public int? HoverBorderRadius { get; private set; }

    /// <summary>
    /// El ancho del borde al pasar el cursor por encima.
    /// </summary>
    public int? HoverBorderWidth { get; private set; }

    /// <summary>
    /// El desplazamiento al pasar el cursor por encima.
    /// </summary>
    public int? HoverOffset { get; private set; }

    /// <summary>
    /// El radio al pasar el cursor por encima.
    /// </summary>
    public int? HoverRadius { get; private set; }

    /// <summary>
    /// El íncide del eje.
    /// </summary>
    public string? IndexAxis { get; private set; }

    /// <summary>
    /// La cantidad de inflación.
    /// </summary>
    public string? InflateAmount { get; private set; }

    /// <summary>
    /// La etiqueta.
    /// </summary>
    public string? Label { get; private set; }

    /// <summary>
    /// El máximo grosor de la barra.
    /// </summary>
    public int? MaxBarThickness { get; private set; }

    /// <summary>
    /// El mínimo largo de la barra.
    /// </summary>
    public int? MinBarLength { get; private set; }

    /// <summary>
    /// El desplazamiento.
    /// </summary>
    public int? Offset { get; private set; }

    /// <summary>
    /// El orden de despliegue del dataset.
    /// </summary>
    public int? Order { get; private set; }

    /// <summary>
    /// El color de fondo del punto.
    /// </summary>
    public string? PointBackgroundColor { get; private set; }

    /// <summary>
    /// El color del borde del punto.
    /// </summary>
    public string? PointBorderColor { get; private set; }

    /// <summary>
    /// El ancho del borde del punto.
    /// </summary>
    public int? PointBorderWidth { get; private set; }

    /// <summary>
    /// El radio de impacto del punto.
    /// </summary>
    public int? PointHitRadius { get; private set; }

    /// <summary>
    /// El color de fondo del punto al pasar el cursor por encima.
    /// </summary>
    public string? PointHoverBackgroundColor { get; private set; }

    /// <summary>
    /// El color del borde del punto al pasar el cursor por encima.
    /// </summary>
    public string? PointHoverBorderColor { get; private set; }

    /// <summary>
    /// El ancho del borde del punto al pasar el cursor por encima.
    /// </summary>
    public int? PointHoverBorderWidth { get; private set; }

    /// <summary>
    /// El radio del punto al pasar el cursor por encima.
    /// </summary>
    public int? PointHoverRadius { get; private set; }

    /// <summary>
    /// El radio del punto.
    /// </summary>
    public int? PointRadius { get; private set; }

    /// <summary>
    /// La rotación del punto.
    /// </summary>
    public int? PointRotation { get; private set; }

    /// <summary>
    /// El estilo del punto.
    /// </summary>
    public PointStyle? PointStyle { get; private set; }

    /// <summary>
    /// El radio.
    /// </summary>
    public int? Radius { get; private set; }

    /// <summary>
    /// La rotación.
    /// </summary>
    public int? Rotation { get; private set; }

    /// <summary>
    /// Si se mostrarán líneas.
    /// </summary>
    public bool? ShowLine { get; private set; }

    /// <summary>
    /// Si se saltarán los valores nulos.
    /// </summary>
    public bool? SkipNull { get; private set; }

    /// <summary>
    /// El espaciado.
    /// </summary>
    public int? Spacing { get; private set; }

    /// <summary>
    /// Los huecos de tramos.
    /// </summary>
    public string? SpanGaps { get; private set; }

    /// <summary>
    /// La apilación.
    /// </summary>
    public string? Stack { get; private set; }

    /// <summary>
    /// Los pasos.
    /// </summary>
    public string? Stepped { get; private set; }

    /// <summary>
    /// La tensión.
    /// </summary>
    public double? Tension { get; private set; }

    /// <summary>
    /// El peso.
    /// </summary>
    public int? Weight { get; set; }

    /// <summary>
    /// El id del eje X.
    /// </summary>
    public string? XAxisID { get; private set; }

    /// <summary>
    /// El id del eje Y.
    /// </summary>
    public string? YAxisID { get; private set; }

    /// <summary>
    /// El parseo.
    /// </summary>
    public string? Parsing { get; private set; }

    /// <summary>
    /// Si estará oculto.
    /// </summary>
    public bool? Hidden { get; private set; }

    /// <summary>
    /// La fuente de datos del dataset.
    /// </summary>
    public DataSource? DataSource { get; private set; }

    /// <summary>
    /// El índice de la fuente de datos si es que la fuente de datos devuelve varios record sets.
    /// </summary>
    public int? DataSourceIndex { get; private set; }

    /// <summary>
    /// El nombre de la columna.
    /// </summary>
    public string? ColumnName { get; private set; }

    /// <summary>
    /// El nombre de la columna en el eje X.
    /// </summary>
    public string? XColumnName { get; private set; }

    /// <summary>
    /// El nombre de la columna en el eje Y.
    /// </summary>
    public string? YColumnName { get; private set; }

    /// <summary>
    /// El nombre de la columna de filtro.
    /// </summary>
    public string? FilterColumnName { get; private set; }

    /// <summary>
    /// El valor de filtro.
    /// </summary>
    public string? FilterValue { get; private set; }

    /// <summary>
    /// El tipo del valor del chart.
    /// </summary>
    public ChartDataType DataType { get; private set; }
}
