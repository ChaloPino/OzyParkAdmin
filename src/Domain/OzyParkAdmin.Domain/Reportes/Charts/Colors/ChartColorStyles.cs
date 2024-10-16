using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OzyParkAdmin.Domain.Reportes.Charts.Colors;

/// <summary>
/// Los estilos de colores para los gráficos.
/// </summary>
public static partial class ChartColorStyles
{
    private static readonly Regex _alphaRegex = AlphaRegex();
    private static readonly Regex _alphaAddRegex = AlphaAddRegex();
    private static readonly Regex _alphaSubRegex = AlphaSubRegex();
    private static readonly Regex _hueRegex = HueRegex();
    private static readonly Regex _hueModRegex = HueModRegex();
    private static readonly Regex _hueOffRegex = HueOffRegex();
    private static readonly Regex _satRegex = SatRegex();
    private static readonly Regex _satModRegex = SatModRegex();
    private static readonly Regex _satOffRegex = SatOffRegex();
    private static readonly Regex _lumRegex = LumRegex();
    private static readonly Regex _lumModRegex = LumModRegex();
    private static readonly Regex _lumOffRegex = LumOffRegex();

    private static readonly Dictionary<Regex, Func<double, ChartColorStyleVariationToken>> VariationTokenCreators = new()
    {
        [_alphaRegex] = ChartColorStyleVariationToken.Transparency,
        [_alphaAddRegex] = ChartColorStyleVariationToken.IncrementTransparency,
        [_alphaSubRegex] = ChartColorStyleVariationToken.ReduceTransparency,
        [_hueRegex] = ChartColorStyleVariationToken.Hue,
        [_hueModRegex] = ChartColorStyleVariationToken.HueModulate,
        [_hueOffRegex] = ChartColorStyleVariationToken.HueOffset,
        [_satRegex] = ChartColorStyleVariationToken.Saturation,
        [_satModRegex] = ChartColorStyleVariationToken.SaturationModulate,
        [_satOffRegex] = ChartColorStyleVariationToken.SaturationOffset,
        [_lumRegex] = ChartColorStyleVariationToken.Luminance,
        [_lumModRegex] = ChartColorStyleVariationToken.LuminanceModulate,
        [_lumOffRegex] = ChartColorStyleVariationToken.LuminanceOffset,
    };

    /// <summary>
    /// Estilo multicolor 1.
    /// </summary>
    public static readonly ChartColorStyle MultiColor1
        = new ChartColorStyle("multicolor1")
        .AddColors(
            ChartColors.BlueAccent1,
            ChartColors.OrangeAccent2,
            ChartColors.GrayAccent3,
            ChartColors.GoldAccent4,
            ChartColors.BlueAccent5,
            ChartColors.GreenAccent6)
        .AddVariations(
            ChartColorStyleVariation.Empty,
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.6)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.8),
                ChartColorStyleVariationToken.LuminanceOffset(0.2)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.8)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.6),
                ChartColorStyleVariationToken.LuminanceOffset(0.2)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.5)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.7),
                ChartColorStyleVariationToken.LuminanceOffset(0.3)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.7)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.5),
                ChartColorStyleVariationToken.LuminanceOffset(0.5)));

    /// <summary>
    /// Estilo multicolor 2.
    /// </summary>
    public static readonly ChartColorStyle MultiColor2
        = new ChartColorStyle("multicolor2")
        .AddColors(
            ChartColors.BlueAccent1,
            ChartColors.GrayAccent3,
            ChartColors.BlueAccent5)
        .AddVariations(
            ChartColorStyleVariation.Empty,
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.6)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.8),
                ChartColorStyleVariationToken.LuminanceOffset(0.2)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.8)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.6),
                ChartColorStyleVariationToken.LuminanceOffset(0.2)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.5)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.7),
                ChartColorStyleVariationToken.LuminanceOffset(0.3)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.7)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.5),
                ChartColorStyleVariationToken.LuminanceOffset(0.5)));

    /// <summary>
    /// Estilo multicolor 3.
    /// </summary>
    public static readonly ChartColorStyle MultiColor3
        = new ChartColorStyle("multicolor3")
        .AddColors(
            ChartColors.OrangeAccent2,
            ChartColors.GoldAccent4,
            ChartColors.GreenAccent6)
        .AddVariations(
            ChartColorStyleVariation.Empty,
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.6)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.8),
                ChartColorStyleVariationToken.LuminanceOffset(0.2)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.8)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.6),
                ChartColorStyleVariationToken.LuminanceOffset(0.2)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.5)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.7),
                ChartColorStyleVariationToken.LuminanceOffset(0.3)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.7)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.5),
                ChartColorStyleVariationToken.LuminanceOffset(0.5)));

    /// <summary>
    /// Estilo multicolor 4.
    /// </summary>
    public static readonly ChartColorStyle MultiColor4
        = new ChartColorStyle("multicolor4")
        .AddColors(
            ChartColors.GreenAccent6,
            ChartColors.BlueAccent5,
            ChartColors.GoldAccent4)
        .AddVariations(
            ChartColorStyleVariation.Empty,
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.6)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.8),
                ChartColorStyleVariationToken.LuminanceOffset(0.2)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.8)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.6),
                ChartColorStyleVariationToken.LuminanceOffset(0.2)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.5)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.7),
                ChartColorStyleVariationToken.LuminanceOffset(0.3)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.7)),
            new ChartColorStyleVariation(
                ChartColorStyleVariationToken.LuminanceModulate(0.5),
                ChartColorStyleVariationToken.LuminanceOffset(0.5)));

    private static readonly Dictionary<string, ChartColorStyle> _colorStyles
        = new(StringComparer.OrdinalIgnoreCase)
        {
            [MultiColor1.Key] = MultiColor1,
            [MultiColor2.Key] = MultiColor2,
            [MultiColor3.Key] = MultiColor3,
            [MultiColor4.Key] = MultiColor4,
        };

    /// <summary>
    /// Trata de conseguir los colores de un estilo dado el <paramref name="styleKey"/> y la <paramref name="variation"/>.
    /// </summary>
    /// <param name="styleKey">La clave del estilo a buscar.</param>
    /// <param name="variation">El nombre de la variación a buscar.</param>
    /// <param name="colors">La lista de colores que se encontraron.</param>
    /// <returns><c>true</c> si existe el estilo <paramref name="styleKey"/>; en caso contrario, <c>false</c>.</returns>
    public static bool TryGetColors(string styleKey, string variation, [NotNullWhen(true)] out IEnumerable<IChartColor>? colors)
    {
        if (_colorStyles.TryGetValue(styleKey, out ChartColorStyle? style))
        {
            ChartColorStyleVariationToken? token = ExtractVariation(variation);
            colors = style.GenerateColors(token);
            return true;
        }

        colors = null;
        return false;
    }

    internal static ChartColorStyleVariationToken? ExtractVariation(string variation)
    {
        if (string.IsNullOrEmpty(variation))
        {
            return null;
        }

        foreach (KeyValuePair<Regex, Func<double, ChartColorStyleVariationToken>> pair in VariationTokenCreators)
        {
            Regex regex = pair.Key;
            Func<double, ChartColorStyleVariationToken> creator = pair.Value;
            Match match = regex.Match(variation);

            if (match.Success)
            {
                double value = double.Parse(match.Groups["Value"].Value, CultureInfo.InvariantCulture);

                return creator(value);
            }
        }

        throw new InvalidOperationException($"'{variation}' no es una variación soportada.");
    }

    [GeneratedRegex(@"alpha-(?<Value>\d+(\.\d+)?)", RegexOptions.Compiled)]
    private static partial Regex AlphaRegex();

    [GeneratedRegex(@"alphaadd-(?<Value>\d+(\.\d+)?)", RegexOptions.Compiled)]
    private static partial Regex AlphaAddRegex();

    [GeneratedRegex(@"alphasub-(?<Value>\d+(\.\d+)?)", RegexOptions.Compiled)]
    private static partial Regex AlphaSubRegex();

    [GeneratedRegex(@"hue-(?<Value>\d+(\.\d+)?)", RegexOptions.Compiled)]
    private static partial Regex HueRegex();

    [GeneratedRegex(@"huemod-(?<Value>\d+(\.\d+)?)", RegexOptions.Compiled)]
    private static partial Regex HueModRegex();

    [GeneratedRegex(@"hueoff-(?<Value>\d+(\.\d+)?)", RegexOptions.Compiled)]
    private static partial Regex HueOffRegex();
    [GeneratedRegex(@"sat-(?<Value>\d+(\.\d+)?)", RegexOptions.Compiled)]
    private static partial Regex SatRegex();

    [GeneratedRegex(@"satmod-(?<Value>\d+(\.\d+)?)", RegexOptions.Compiled)]
    private static partial Regex SatModRegex();

    [GeneratedRegex(@"satoff-(?<Value>\d+(\.\d+)?)", RegexOptions.Compiled)]
    private static partial Regex SatOffRegex();

    [GeneratedRegex(@"lum-(?<Value>\d+(\.\d+)?)", RegexOptions.Compiled)]
    private static partial Regex LumRegex();

    [GeneratedRegex(@"lummod-(?<Value>\d+(\.\d+)?)", RegexOptions.Compiled)]
    private static partial Regex LumModRegex();

    [GeneratedRegex(@"lumoff-(?<Value>\d+(\.\d+)?)", RegexOptions.Compiled)]
    private static partial Regex LumOffRegex();
}
