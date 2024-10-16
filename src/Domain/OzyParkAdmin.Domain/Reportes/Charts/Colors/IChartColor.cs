namespace OzyParkAdmin.Domain.Reportes.Charts.Colors;

/// <summary>
/// El color para un gráfico.
/// </summary>
public interface IChartColor
{
    /// <summary>
    /// Convierte el <see cref="IChartColor"/> en un <see cref="RgbChartColor"/>.
    /// </summary>
    /// <returns>El <see cref="RgbChartColor"/></returns>
    RgbChartColor ToRgb();

    /// <summary>
    /// Convierte el <see cref="IChartColor"/> en un <see cref="HexChartColor"/>.
    /// </summary>
    /// <returns>El <see cref="HexChartColor"/></returns>
    HexChartColor ToHex();

    /// <summary>
    /// Convierte el <see cref="IChartColor"/> en un <see cref="HslChartColor"/>.
    /// </summary>
    /// <returns>El <see cref="HslChartColor"/>.</returns>
    HslChartColor ToHsl();

    /// <summary>
    /// La representación de texto del color.
    /// </summary>
    /// <returns>La representación de texto del color.</returns>
    string ToString();
}
