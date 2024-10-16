using System.Globalization;
using System.Text.RegularExpressions;

namespace OzyParkAdmin.Domain.Reportes.Charts.Colors;

/// <summary>
/// Contiene los métodos para poder parsear colores.
/// </summary>
public static partial class ChartColorsParser
{
    private static readonly Regex _styleRegex = StyleRegex();
    private static readonly Regex _patternRegex = PatternRegex();
    private static readonly Regex _hexadecimalRegex = HexRegex();
    private static readonly Regex _reducedHexadecimalRegex = ReducedHexRegex();
    private static readonly Regex _hslRegex = HslRegex();
    private static readonly Regex _rgbRegex = RgbRegex();
    private static readonly Regex _themeRegex = ThemeRegex();

    private static readonly Dictionary<Regex, Action<Match, List<IChartColor>>> Parsers = new()
    {
        [_styleRegex] = ParseColorStyles,
        [_patternRegex] = ParsePatternColors,
        [_hexadecimalRegex] = ParseHexColors,
        [_reducedHexadecimalRegex] = ParseHexColors,
        [_hslRegex] = ParseHslColors,
        [_rgbRegex] = ParseRgbColors,
        [_themeRegex] = ParseThemeColors,
    };

    /// <summary>
    /// Parsea el <paramref name="value"/> para conseguir los colores.
    /// </summary>
    /// <param name="value">El valor a parsear.</param>
    /// <returns>La lista de colores.</returns>
    public static IList<IChartColor>? Parse(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        string[] values = value.Split(['|'], StringSplitOptions.RemoveEmptyEntries);

        List<IChartColor> allColors = [];

        foreach (string val in values)
        {
            List<IChartColor> colors = [];

            foreach (var parser in Parsers)
            {
                Match match = parser.Key.Match(val);

                if (match.Success)
                {
                    parser.Value(match, colors);

                    if (colors.Count > 0)
                    {
                        allColors.AddRange(colors);
                        break;
                    }
                }
            }
        }

        return allColors;
    }

    private static void ParseColorStyles(Match match, List<IChartColor> colors)
    {
        string styleKey = match.Groups["Style"].Value;
        string variation = match.Groups["Variation"].Value;

        if (ChartColorStyles.TryGetColors(styleKey, variation, out IEnumerable<IChartColor>? generatedColors))
        {
            colors.AddRange(generatedColors);
        }
    }

    private static void ParsePatternColors(Match match, List<IChartColor> colors)
    {
        string name = match.Groups["Name"].Value;
        string value = match.Groups["Value"].Value;
        colors.Add(new PatternChartColor(name, ParseOne(value)));
    }

    private static void ParseHexColors(Match match, List<IChartColor> colors)
    {
        string value = match.Groups["Value"].Value;
        colors.Add(new HexChartColor(value));
    }

    private static void ParseHslColors(Match match, List<IChartColor> colors)
    {
        double hue = double.Parse(match.Groups["Hue"].Value, CultureInfo.InvariantCulture);
        double saturation = double.Parse(match.Groups["Saturation"].Value, CultureInfo.InvariantCulture);
        double light = int.Parse(match.Groups["Light"].Value, CultureInfo.InvariantCulture);
        colors.Add(new HslChartColor(hue, saturation, light));
    }

    private static void ParseRgbColors(Match match, List<IChartColor> colors)
    {
        int red = int.Parse(match.Groups["Red"].Value, CultureInfo.InvariantCulture);
        int green = int.Parse(match.Groups["Green"].Value, CultureInfo.InvariantCulture);
        int blue = int.Parse(match.Groups["Blue"].Value, CultureInfo.InvariantCulture);
        double? alpha = null;

        string strAlpha = match.Groups["Alpha"].Value;

        if (!string.IsNullOrEmpty(strAlpha))
        {
            alpha = double.Parse(strAlpha, CultureInfo.InvariantCulture);
        }

        colors.Add(new RgbChartColor(red, green, blue, alpha));
    }

    private static void ParseThemeColors(Match match, List<IChartColor> colors)
    {
        string key = match.Groups["Key"].Value;
        string variation = match.Groups["Variation"].Value;

        if (ChartColors.TryGetColor(key, variation, out IChartColor? color))
        {
            colors.Add(color);
        }
    }

    /// <summary>
    /// Parsea el <paramref name="value"/> para conseguir un solo color.
    /// </summary>
    /// <param name="value">El valor a parsear.</param>
    /// <returns>El color.</returns>
    public static IChartColor ParseOne(string? value)
    {
        IEnumerable<IChartColor>? colors = Parse(value);
        return colors?.Any() != true ? ChartColors.White : colors.First();
    }

    [GeneratedRegex(@"^style-(?<Style>\w+)(-(?<Variation>alpha-\d+(\.\d+)?|hue(mod|off)?-\d+(\.\d+)?|sat(mod|off)?-\d+(\.d+)?|lum(mod|off)?-\d+(\.\d+)?))?$", RegexOptions.Compiled)]
    private static partial Regex StyleRegex();

    [GeneratedRegex(@"pattern\.draw\('(?<Name>\w+)',\s*'(?<Value>.+)'\)", RegexOptions.Compiled)]
    private static partial Regex PatternRegex();

    [GeneratedRegex("#(?<Value>[a-fA-F0-9]{6})", RegexOptions.Compiled)]
    private static partial Regex HexRegex();

    [GeneratedRegex("#(?<Value>[a-fA-F0-9]{3})", RegexOptions.Compiled)]
    private static partial Regex ReducedHexRegex();

    [GeneratedRegex(@"hsl?\((?<Hue>\d+)\s*,\s*(?<Saturation>d+%)\s*,\s*(?<Light>d+%)\)", RegexOptions.Compiled)]
    private static partial Regex HslRegex();

    [GeneratedRegex(@"rgb(a)?\((?<Red>\d+)\s*,\s*(?<Green>\d+)\s*,\s*(?<Blue>\d+)(\s*,\s*(?<Alpha>\d+(\.\d+)?))?\)", RegexOptions.Compiled)]
    private static partial Regex RgbRegex();

    [GeneratedRegex(@"(?<Key>accent(1|2|3|4|5|6)|red|orange|yellow|green|blue|purple|grey)(-(?<Variation>alpha-\d+(\.\d+)?|hue(mod|off)?-\d+(\.\d+)?|sat(mod|off)?-\d+(\.d+)?|lum(mod|off)?-\d+(\.\d+)?))?")]
    private static partial Regex ThemeRegex();
}
