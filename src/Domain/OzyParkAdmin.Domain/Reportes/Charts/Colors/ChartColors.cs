using System.Diagnostics.CodeAnalysis;

namespace OzyParkAdmin.Domain.Reportes.Charts.Colors;

/// <summary>
/// Los colores predeterminados para los gráficos.
/// </summary>
public static class ChartColors
{
    /// <summary>
    /// Azul acentuado 1.
    /// </summary>
    public static readonly RgbChartColor BlueAccent1 = new(68, 114, 196, null);

    /// <summary>
    /// Anaranjado acentuado 2.
    /// </summary>
    public static readonly RgbChartColor OrangeAccent2 = new(237, 125, 49, null);

    /// <summary>
    /// Gris acentuado 3.
    /// </summary>
    public static readonly RgbChartColor GrayAccent3 = new(165, 165, 165, null);

    /// <summary>
    /// Oro acentuado 4.
    /// </summary>
    public static readonly RgbChartColor GoldAccent4 = new(255, 192, 0, null);

    /// <summary>
    /// Azul acentuado 5.
    /// </summary>
    public static readonly RgbChartColor BlueAccent5 = new(91, 155, 213, null);

    /// <summary>
    /// Verde acentuado 6.
    /// </summary>
    public static readonly RgbChartColor GreenAccent6 = new(112, 173, 71, null);

    /// <summary>
    /// Rojo
    /// </summary>
    public static readonly RgbChartColor Red = new(255, 99, 132, null);

    /// <summary>
    /// Anaranjado.
    /// </summary>
    public static readonly RgbChartColor Orange = new(255, 159, 86, null);

    /// <summary>
    /// Amarillo.
    /// </summary>
    public static readonly RgbChartColor Yellow = new(255, 205, 86, null);

    /// <summary>
    /// Verde.
    /// </summary>
    public static readonly RgbChartColor Green = new(75, 192, 192, null);

    /// <summary>
    /// Azul.
    /// </summary>
    public static readonly RgbChartColor Blue = new(54, 162, 235, null);

    /// <summary>
    /// Morado / Púrpura.
    /// </summary>
    public static readonly RgbChartColor Purple = new(153, 102, 255, null);

    /// <summary>
    /// Gris.
    /// </summary>
    public static readonly RgbChartColor Grey = new(201, 203, 207, null);

    /// <summary>
    /// Blanco.
    /// </summary>
    public static readonly RgbChartColor White = new(255, 255, 255, null);

    /// <summary>
    /// Negro.
    /// </summary>
    public static readonly RgbChartColor Black = new(0, 0, 0, null);

    private static readonly Dictionary<string, IChartColor> ThemeColors = new(StringComparer.OrdinalIgnoreCase)
    {
        ["accent1"] = BlueAccent1,
        ["accent2"] = OrangeAccent2,
        ["accent3"] = GrayAccent3,
        ["accent4"] = GoldAccent4,
        ["accent5"] = BlueAccent5,
        ["accent6"] = GreenAccent6,
        ["red"] = Red,
        ["orange"] = Orange,
        ["yellow"] = Yellow,
        ["green"] = Green,
        ["blue"] = Blue,
        ["purple"] = Purple,
        ["grey"] = Grey,
        ["white"] = White,
        ["black"] = Black,
    };

    /// <summary>
    /// Trata de conseguir el color de tema aplicando una variación.
    /// </summary>
    /// <param name="name">El nombre del color del tema.</param>
    /// <param name="variation">La variación que se le quiere aplicar.</param>
    /// <param name="color">El color del tema.</param>
    /// <returns><c>true</c> si existe el nombre; en caso contrario, <c>false</c>.</returns>
    public static bool TryGetColor(string name, string variation, [NotNullWhen(true)] out IChartColor? color)
    {
        if (ThemeColors.TryGetValue(name, out color))
        {
            ChartColorStyleVariationToken? token = ChartColorStyles.ExtractVariation(variation);

            if (token.HasValue)
            {
                color = token.Value.Apply(color);
            }

            return true;
        }

        return false;
    }
}
