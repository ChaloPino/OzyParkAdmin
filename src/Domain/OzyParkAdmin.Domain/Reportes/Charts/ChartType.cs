namespace OzyParkAdmin.Domain.Reportes.Charts;

/// <summary>
/// El tipo de un gráfico.
/// </summary>
public enum ChartType
{
    /// <summary>
    /// Es un gráfico de líneas.
    /// </summary>
    Line = 1,

    /// <summary>
    /// Es un gráfico de barras.
    /// </summary>
    Bar = 2,

    /// <summary>
    /// Es un gráfico de burbujas.
    /// </summary>
    Bubble = 3,

    /// <summary>
    /// Es un gráfico de dona.
    /// </summary>
    Doughnut = 4,

    /// <summary>
    /// Es un gráfico de pie.
    /// </summary>
    Pie = 5,

    /// <summary>
    /// Es un gráfico de área polar.
    /// </summary>
    PolarArea = 6,

    /// <summary>
    /// Es un gráfico de tipo radar.
    /// </summary>
    Radar = 7,

    /// <summary>
    /// Es un gráfico esparcido.
    /// </summary>
    Scatter = 8,

    /// <summary>
    /// Es una tabla.
    /// </summary>
    Table = 9,
}
