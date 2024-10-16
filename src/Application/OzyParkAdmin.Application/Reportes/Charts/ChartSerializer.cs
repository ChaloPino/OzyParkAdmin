using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OzyParkAdmin.Application.Reportes.Charts.Enums;
using OzyParkAdmin.Application.Reportes.Charts.Records;
using OzyParkAdmin.Domain.Reportes.Charts;
using OzyParkAdmin.Domain.Reportes.Charts.Colors;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OzyParkAdmin.Application.Reportes.Charts;
internal static partial class ChartSerializer
{
    private static readonly JsonSerializerSettings _settings = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy(),
        },
    };

    static ChartSerializer()
    {
        _settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
    }

    internal static void AddParsingTo(this string? value, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (bool.TryParse(value, out bool parsedBoolean))
        {
            container["parsing"] = parsedBoolean;
            return;
        }

        JObject? obj = (JObject?)JsonConvert.DeserializeObject(value, _settings);

        if (obj is not null)
        {
            Dictionary<string, object?> parsing = [];

            obj.AddTo<string>("id", parsing);
            obj.AddTo<string>("xAxisKey", parsing);
            obj.AddTo<string>("yAxisKey", parsing);
            obj.AddTo<string>("key", parsing);

            container["parsing"] = parsing;
        }
    }

    internal static void AddInteractionTo(this string? value, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        JObject? obj = (JObject?)JsonConvert.DeserializeObject(value, _settings);

        if (obj is not null)
        {
            Dictionary<string, object?> interaction = [];

            obj.AddTo<string>("mode", interaction);
            obj.AddTo<bool>("intersect", interaction);
            obj.AddTo<string>("axis", interaction);
            obj.AddTo<bool>("includeInvisible", interaction);

            container["interaction"] = interaction;
        }
    }

    internal static void AddLayoutTo(this string? value, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        Dictionary<string, object?> layout = [];

        JObject? obj = (JObject?)JsonConvert.DeserializeObject(value, _settings);

        if (obj is not null)
        {
            obj.AddTo<bool>("autoPadding", layout);
            obj.AddPaddingTo(layout);

            if (layout.Count > 0)
            {
                container["layout"] = layout;
            }
        }
    }

    internal static void AddPaddingTo(this JObject obj, IDictionary<string, object?> container)
    {
        if (obj.TryGetValue("padding", out JToken? token))
        {
            if (token is JValue value)
            {
                container["padding"] = value.Value<int>();
                return;
            }

            Dictionary<string, object?> padding = [];

            if (token is JObject jObj)
            {
                jObj.AddTo<int>("left", padding);
                jObj.AddTo<int>("top", padding);
                jObj.AddTo<int>("right", padding);
                jObj.AddTo<int>("bottom", padding);
                jObj.AddTo<int>("x", padding);
                jObj.AddTo<int>("y", padding);
            }

            if (padding.Count > 0)
            {
                container["padding"] = padding;
            }
        }
    }

    internal static void AddDecimationTo(this string? value, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        JObject? obj = (JObject?)JsonConvert.DeserializeObject(value, _settings);

        if (obj is not null)
        {
            Dictionary<string, object?> decimation = [];

            obj.AddTo<bool>("enabled", decimation);
            obj.AddTo<string>("algorithm", decimation);
            obj.AddTo<double>("samples", decimation);
            obj.AddTo<double>("threshold", decimation);

            container["decimation"] = decimation;
        }
    }

    internal static void AddLegendTo(this string? value, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        JObject? obj = (JObject?)JsonConvert.DeserializeObject(value, _settings);

        if (obj is not null)
        {
            Dictionary<string, object?> legend = [];
            obj.AddTo<bool>("display", legend);
            obj.AddEnumTo<Position>("position", legend);
            obj.AddEnumTo<Align>("align", legend);
            obj.AddTo<int>("maxHeight", legend);
            obj.AddTo<int>("maxWidth", legend);
            obj.AddTo<bool>("reverse", legend);
            obj.AddLegendLabelsTo(legend);
            obj.AddTo<bool>("rtl", legend);
            obj.AddTo<string>("textDirection", legend);
            obj.AddLegendTitleTo(legend);
            container["legend"] = legend;
        }
    }

    private static void AddLegendLabelsTo(this JObject obj, Dictionary<string, object?> container)
    {
        if (obj.TryGetValue("labels", out JToken? token))
        {
            Dictionary<string, object?> labels = [];
            JObject legendObj = (JObject)token;
            legendObj.AddTo<int>("boxWidth", labels);
            legendObj.AddTo<int>("boxHeight", labels);
            legendObj.AddArrayOrScalarTo<string, string>("color", labels, (color) => ChartColorsParser.ParseOne(color).ToString());
            legendObj.AddFontTo("font", labels);
            legendObj.AddPaddingTo(labels);
            legendObj.AddEnumTo<PointStyle>("pointStyle", labels);
            legendObj.AddEnumTo<TextAlign>("textAlign", labels);
            legendObj.AddTo<bool>("usePointStyle", labels);
            container["labels"] = labels;
        }
    }

    private static void AddLegendTitleTo(this JObject obj, Dictionary<string, object?> container)
    {
        if (obj.TryGetValue("title", out JToken? token))
        {
            Dictionary<string, object?> title = [];
            JObject titleObj = (JObject)token;
            titleObj.AddArrayOrScalarTo<string, string>("color", title, (color) => ChartColorsParser.ParseOne(color).ToString());
            titleObj.AddTo<bool>("display", title);
            titleObj.AddFontTo("font", title);
            titleObj.AddPaddingTo(title);
            titleObj.AddTo<string>("text", title);
            container["title"] = title;
        }
    }

    private static void AddFontTo(this JObject obj, string propertyName, Dictionary<string, object?> container)
    {
        if (obj.TryGetValue(propertyName, out JToken? token))
        {
            Dictionary<string, object?> font = [];

            JObject fontObject = (JObject)token;
            fontObject.AddTo<string>("family", font);
            fontObject.AddTo<double>("size", font);
            fontObject.AddTo<string>("style", font);
            fontObject.AddTo<string, int>("weight", font);
            fontObject.AddTo<double, string>("lineHeight", font);

            container[propertyName] = font;
        }
    }

    internal static void AddTitleTo(this string? value, string propertyName, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        Dictionary<string, object?> title = [];

        JObject? obj = (JObject?)JsonConvert.DeserializeObject(value, _settings);

        if (obj is not null)
        {
            obj.AddEnumTo<Align>("align", title);
            obj.AddArrayOrScalarTo<string, string>("color", title, color => ChartColorsParser.ParseOne(color).ToString());
            obj.AddTo<bool>("display", title);
            obj.AddTo<bool>("fullSize", title);
            obj.AddEnumTo<Position>("position", title);
            obj.AddFontTo("font", title);
            obj.AddPaddingTo(title);
            obj.AddArrayOrScalarTo<string>("text", title);
            container[propertyName] = title;
        }
    }

    internal static void AddTooltipTo(this string? value, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        JObject? obj = (JObject?)JsonConvert.DeserializeObject(value, _settings);

        if (obj is not null)
        {
            Dictionary<string, object?> tooltip = [];
            obj.AddTo<bool>("enabled", tooltip);
            obj.AddTo<string>("external", tooltip);
            obj.AddTo<string>("mode", tooltip);
            obj.AddTo<bool>("intersect", tooltip);
            obj.AddTo<string>("position", tooltip);
            obj.AddTo("callbacks", tooltip);
            obj.AddTo<string>("itemSort", tooltip);
            obj.AddTo<string>("filter", tooltip);
            obj.AddColorTo("backgroundColor", tooltip);
            obj.AddColorTo("titleColor", tooltip);
            obj.AddFontTo("titleFont", tooltip);
            obj.AddTo<string>("titleAlign", tooltip);
            obj.AddTo<int>("titleSpacing", tooltip);
            obj.AddTo<int>("titleMarginBottom", tooltip);
            obj.AddColorTo("bodyColor", tooltip);
            obj.AddFontTo("bodyFont", tooltip);
            obj.AddTo<string>("bodyAlign", tooltip);
            obj.AddTo<int>("bodySpacing", tooltip);
            obj.AddColorTo("footerColor", tooltip);
            obj.AddFontTo("footerFont", tooltip);
            obj.AddTo<string>("footerAlign", tooltip);
            obj.AddTo<int>("footerSpacing", tooltip);
            obj.AddTo<int>("footerMarginTop", tooltip);
            obj.AddPaddingTo(tooltip);
            obj.AddTo<int>("caretPadding", tooltip);
            obj.AddTo<int>("carentSize", tooltip);
            obj.AddTo("cornerRadius", tooltip);
            obj.AddColorTo("multikeyBackground", tooltip);
            obj.AddTo<bool>("displayColors", tooltip);
            obj.AddTo<int>("boxWidth", tooltip);
            obj.AddTo<int>("boxHeight", tooltip);
            obj.AddTo<int>("boxPadding", tooltip);
            obj.AddTo<bool>("usePointStyle", tooltip);
            obj.AddColorTo("borderColor", tooltip);
            obj.AddTo<int>("borderWidth", tooltip);
            obj.AddTo<bool>("rtl", tooltip);
            obj.AddTo<string>("textDirection", tooltip);
            obj.AddTo<string>("xAlign", tooltip);
            obj.AddTo<string>("yAlign", tooltip);

            container["tooltip"] = tooltip;
        }
    }

    internal static void AddDataLabels(this string? value, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (bool.TryParse(value, out _))
        {
            return;
        }

        JObject? obj = (JObject?)JsonConvert.DeserializeObject(value, _settings);

        if (obj is not null)
        {
            Dictionary<string, object?> datalabels = [];
            obj.AddTo<int, string>("align", datalabels);
            obj.AddTo<string>("anchor", datalabels);
            obj.AddColorTo("backgroundColor", datalabels);
            obj.AddColorTo("borderColor", datalabels);
            obj.AddTo<int>("borderRadius", datalabels);
            obj.AddTo<int>("borderWidth", datalabels);
            obj.AddTo<bool>("clamp", datalabels);
            obj.AddTo<bool>("clip", datalabels);
            obj.AddColorTo("color", datalabels);
            obj.AddTo<bool, string>("display", datalabels);
            obj.AddFontTo("font", datalabels);
            obj.AddTo("formatter", datalabels);
            obj.AddTo("format", datalabels);
            obj.AddTo("labels", datalabels);
            obj.AddTo("listeners", datalabels);
            obj.AddTo<int>("offset", datalabels);
            obj.AddTo<int>("opacity", datalabels);
            obj.AddPaddingTo(datalabels);
            obj.AddTo<int>("rotation", datalabels);
            obj.AddTo<string>("textAlign", datalabels);
            obj.AddColorTo("textStrokeColor", datalabels);
            obj.AddTo<int>("textStrokeWidth", datalabels);
            obj.AddTo<int>("textShadowBlur", datalabels);
            obj.AddColorTo("textShadowColor", datalabels);

            container["datalabels"] = datalabels;
        }
    }

    private static readonly List<string> thickness = ["flex"];

    internal static void AddThicknessTo(this string? value, string propertyName, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
        {
            container[propertyName] = result;
        }

        value = value.Trim().ToLowerInvariant();

        if (thickness.IndexOf(value) >= 0)
        {
            container[propertyName] = value;
        }
    }

    internal static void AddBorderSkippedTo(this BorderSkipped? borderSkipped, string propertyName, IDictionary<string, object?> container)
    {
        if (!borderSkipped.HasValue)
        {
            return;
        }

        if (borderSkipped.Value == BorderSkipped.False)
        {
            container[propertyName] = false;
            return;
        }

        borderSkipped.AddEnumTo(propertyName, container);
    }

    internal static void AddWidthTo(this string? value, string propertyName, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int width))
        {
            container[propertyName] = width;
            return;
        }

        JObject? obj = (JObject?)JsonConvert.DeserializeObject(value, _settings);

        if (obj is not null)
        {
            Dictionary<string, object?> chartWidth = [];

            obj.AddTo<int>("left", chartWidth);
            obj.AddTo<int>("top", chartWidth);
            obj.AddTo<int>("right", chartWidth);
            obj.AddTo<int>("bottom", chartWidth);

            container[propertyName] = chartWidth;
        }
    }

    internal static void AddRadiusTo(this string? value, string propertyName, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int radius))
        {
            container[propertyName] = radius;
            return;
        }

        JObject? obj = (JObject?)JsonConvert.DeserializeObject(value, _settings);

        if (obj is not null)
        {
            Dictionary<string, object?> chartRadius = [];

            obj.AddTo<int>("topLeft", chartRadius);
            obj.AddTo<int>("topRight", chartRadius);
            obj.AddTo<int>("bottomLeft", chartRadius);
            obj.AddTo<int>("bottomRight", chartRadius);

            container[propertyName] = chartRadius;
        }
    }

    internal static void AddClipTo(this string? value, string propertyName, IDictionary<string, object?> container)
    {
        if (!string.IsNullOrEmpty(value))
        {
            if (int.TryParse(value, out int clip))
            {
                container[propertyName] = clip;
                return;
            }

            JObject? obj = (JObject?)JsonConvert.DeserializeObject(value, _settings);

            if (obj is not null)
            {
                Dictionary<string, object?> result = [];

                obj.AddTo<int, bool>("left", result);
                obj.AddTo<int, bool>("top", result);
                obj.AddTo<int, bool>("right", result);
                obj.AddTo<int, bool>("bottom", result);

                container[propertyName] = result;
            }
        }
    }

    private static readonly Regex absoluteFillRegex = AbsoluteFillRegex();
    private static readonly Regex relativeFillRegex = RelativeFillRegex();
    private static readonly Regex boundaryFillRegex = BoundaryFillRegex();
    private static readonly Regex disabledFillRegex = DisabledFillRegex();
    private static readonly Regex stackedValueBelowFillRegex = StackedValueBelowFillRegex();
    private static readonly Regex axisValueFillRegex = AxisValueFillRegex();
    private static readonly Regex shapeFillRegex = ShapeFillRegex();

    internal static void AddFillTo(this string? str, string propertyName, IDictionary<string, object?> container)
    {
        if (!string.IsNullOrEmpty(str))
        {
            if (TryParseSimpleFill(str, out object? value))
            {
                container[propertyName] = value;
                return;
            }

            if (TryParseComplexFill(str, out value))
            {
                container[propertyName] = value;
            }
        }
    }

    private static bool TryParseSimpleFill(string str, [NotNullWhen(true)] out object? value)
    {
        if (absoluteFillRegex.Match(str).Success)
        {
            value = int.Parse(str, CultureInfo.InvariantCulture);
            return true;
        }

        if (disabledFillRegex.Match(str).Success)
        {
            value = bool.Parse(str);
            return true;
        }

        if (relativeFillRegex.Match(str).Success
            || boundaryFillRegex.Match(str).Success
            || stackedValueBelowFillRegex.Match(str).Success
            || shapeFillRegex.Match(str).Success)
        {
            value = str;
            return true;
        }

        Match axisValueMatch = axisValueFillRegex.Match(str);

        if (axisValueMatch.Success)
        {
            value = new ValueChartFill(int.Parse(axisValueMatch.Groups["Value"].Value, CultureInfo.InvariantCulture));
            return true;
        }

        value = null;
        return false;
    }

    private static bool TryParseComplexFill(string str, [NotNullWhen(true)] out object? value)
    {
        TmpFill fill = JsonConvert.DeserializeObject<TmpFill>(str, _settings)!;

        if (fill.Target is int intTarget)
        {
            value = new ComplexChartFill<int>(intTarget, fill.Above, fill.Below);
            return true;
        }

        if (fill.Target is bool boolTarget)
        {
            value = new ComplexChartFill<bool>(boolTarget, fill.Above, fill.Below);
            return true;
        }

        if (fill.Target is string stringTarget)
        {
            if (relativeFillRegex.Match(stringTarget).Success
                || boundaryFillRegex.Match(stringTarget).Success
                || stackedValueBelowFillRegex.Match(stringTarget).Success
                || shapeFillRegex.Match(stringTarget).Success)
            {
                value = new ComplexChartFill<string>(stringTarget, fill.Above, fill.Below);
                return true;
            }

            Match axisValueMatch = axisValueFillRegex.Match(str);

            if (axisValueMatch.Success)
            {
                ValueChartFill target = new(int.Parse(axisValueMatch.Groups["Value"].Value, CultureInfo.InvariantCulture));
                value = new ComplexChartFill<ValueChartFill>(target, fill.Above, fill.Below);
                return true;
            }
        }

        value = null;
        return false;
    }

    private sealed record TmpFill(object Target, string Above, string Below);

    private static readonly List<string> inflateAmounts = ["auto"];
    internal static void AddInflateAmountTo(this string? value, string propertyName, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int inflateAmount))
        {
            container[propertyName] = inflateAmount;
        }

        value = value.Trim().ToLowerInvariant();

        if (inflateAmounts.IndexOf(value) >= 0)
        {
            container[propertyName] = value;
        }
    }

    internal static void AddSpanGapsTo(this string? value, string propertyName, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int spanGap))
        {
            container[propertyName] = spanGap;
            return;
        }

        if (bool.TryParse(value, out bool result))
        {
            container[propertyName] = result;
        }
    }

    private static readonly List<string> steppeds = ["before", "after", "middle"];

    public static void AddSteppedTo(this string? value, string propertyName, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (bool.TryParse(value, out bool result))
        {
            container[propertyName] = result;
            return;
        }

        value = value.Trim().ToLowerInvariant();

        if (steppeds.IndexOf(value) >= 0)
        {
            container[propertyName] = value;
        }
    }

    internal static void AddDictionaryTo(this string? value, string propertyName, IDictionary<string, object?> container)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        Dictionary<string, object?>? dictionary = JsonConvert.DeserializeObject<Dictionary<string, object?>>(value);

        if (dictionary is not null)
        {
            Dictionary<string, object?> newValue = [];

            foreach (KeyValuePair<string, object?> pair in dictionary)
            {
                if (pair.Value is JValue jValue)
                {
                    newValue[pair.Key] = jValue.Value;
                    continue;
                }

                if (pair.Value is JArray array)
                {
                    array.DynamicAddTo(pair.Key, newValue);
                    continue;
                }

                if (pair.Value is JObject obj)
                {
                    obj.DynamicAddTo(pair.Key, newValue);
                    continue;
                }

                newValue[pair.Key] = pair.Value;
            }

            container[propertyName] = newValue;
        }
    }

    internal static void AddColorTo(this JObject obj, string propertyName, IDictionary<string, object?> container)
    {
        if (obj.TryGetValue(propertyName, out JToken? token))
        {
            if (token is JValue value)
            {
                object? colorValue = value.Value;

                if (colorValue is not null)
                {
                    container[propertyName] = ChartColorsParser.ParseOne((string)colorValue).ToString();
                }

                return;
            }

            if (token is JArray array)
            {
                List<string> colors = [];

                foreach (JToken itemToken in array)
                {
                    if (itemToken is JValue item)
                    {
                        object? colorValue = item.Value;

                        if (colorValue is not null)
                        {
                            colors.Add((string)colorValue);
                        }
                    }
                }

                string allColors = string.Join("|", colors);

                allColors.AddColorsTo(propertyName, container);
            }
        }
    }

    internal static void AddColorsTo(this string? value, string propertyName, IDictionary<string, object?> container)
    {
        IList<IChartColor>? colors = ChartColorsParser.Parse(value);

        if (colors?.Any() == true)
        {
            if (colors.Count == 1)
            {
                container[propertyName] = colors[0].ToString();
                return;
            }

            container[propertyName] = colors.Select(x => x.ToString()).ToList();
        }
    }

    internal static void AddInt32ArrayTo(this string? value, string propertyName, IDictionary<string, object?> container)
    {
        int[]? ints = value.ToInt32Array();

        if (ints is not null)
        {
            container[propertyName] = ints;
        }
    }

    internal static int[]? ToInt32Array(this string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        string[] parts = value.Split([','], StringSplitOptions.RemoveEmptyEntries);
        int[] ints = new int[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            ints[i] = int.Parse(parts[i].Trim(), CultureInfo.InvariantCulture);
        }

        return ints;
    }

    internal static void AddTo<T>(this T value, string propertyName, IDictionary<string, object?> container)
        where T : struct =>
        container[propertyName] = value;

    internal static void AddTo<T>(this T? value, string propertyName, IDictionary<string, object?> container, bool allowNullable = false)
        where T : struct
    {
        if (value.HasValue)
        {
            container[propertyName] = value.Value;
            return;
        }

        if (allowNullable)
        {
            container[propertyName] = null;
        }
    }

    internal static void AddTo(this string? value, string propertyName, IDictionary<string, object?> container)
    {
        if (!string.IsNullOrEmpty(value))
        {
            container[propertyName] = value;
        }
    }

    internal static void AddTo<T>(this JObject obj, string propertyName, IDictionary<string, object?> container)
    {
        if (obj.TryGetValue(propertyName, out JToken? token) && token is JValue value && value.Value is T tValue)
        {
            container[propertyName] = tValue;
        }
    }

    internal static void AddTo<T1, T2>(this JObject obj, string propertyName, IDictionary<string, object?> container)
    {
        obj.AddTo<T1>(propertyName, container);

        if (!container.ContainsKey(propertyName))
        {
            obj.AddTo<T2>(propertyName, container);
        }
    }

    internal static void AddTo(this JObject obj, string propertyName, IDictionary<string, object?> container)
    {
        if (obj.TryGetValue(propertyName, out JToken? token))
        {
            if (token is JValue value)
            {
                container[propertyName] = value.Value;
                return;
            }

            if (token is JObject jObj)
            {
                jObj.DynamicAddTo(propertyName, container);
                return;
            }

            if (token is JArray array)
            {
                array.DynamicAddTo(propertyName, container);
            }
        }
    }

    internal static void DynamicAddTo(this JObject obj, string propertyName, IDictionary<string, object?> container)
    {
        Dictionary<string, object?> dictionary = [];

        foreach (JProperty property in obj.Properties())
        {
            if (property.Value is JValue value)
            {
                dictionary[property.Name] = value.Value;
                continue;
            }

            if (property.Value is JArray array)
            {
                array.DynamicAddTo(property.Name, dictionary);
                continue;
            }

            if (property.Value is JObject objValue)
            {
                objValue.DynamicAddTo(property.Name, dictionary);
            }
        }

        container[propertyName] = dictionary;
    }

    internal static void DynamicAddTo(this JArray array, string propertyName, IDictionary<string, object?> container)
    {
        List<object?> list = [];

        array.DynamicAddTo(list);

        container[propertyName] = list;
    }

    private static void DynamicAddTo(this JArray array, List<object?> list)
    {
        foreach (JToken token in array)
        {
            if (token is JValue value)
            {
                list.Add(value.Value);
                continue;
            }

            if (token is JArray innerArray)
            {
                List<object?> innerList = [];
                innerArray.DynamicAddTo(innerList);
                list.Add(innerList);
                continue;
            }

            if (token is JObject obj)
            {
                Dictionary<string, object?> dictionary = [];

                foreach (JProperty property in obj.Properties())
                {
                    if (property.Value is JValue jValue)
                    {
                        dictionary[property.Name] = jValue.Value;
                        continue;
                    }

                    if (property.Value is JArray jArray)
                    {
                        jArray.DynamicAddTo(property.Name, dictionary);
                        continue;
                    }

                    if (property.Value is JObject jObj)
                    {
                        jObj.DynamicAddTo(property.Name, dictionary);
                    }
                }

                list.Add(dictionary);
                continue;
            }
        }
    }

    internal static void AddEnumTo<TEnum>(this TEnum? @enum, string propertyName, IDictionary<string, object?> container)
        where TEnum : struct
    {
        string? value = @enum.ToLowerString();

        if (value is not null)
        {
            container[propertyName] = value;
        }
    }

    internal static void AddEnumTo<T>(this JObject obj, string propertyName, IDictionary<string, object?> container)
        where T : struct
    {
        if (obj.TryGetValue(propertyName, out JToken? token) && token is JValue value && value.Value is string strValue && Enum.TryParse<T>(strValue, true, out _))
        {
            container[propertyName] = strValue;
        }
    }

    internal static void AddArrayOrScalarTo<T, TResult>(this JObject obj, string propertyName, IDictionary<string, object?> container, Func<T?, TResult?> converter)
    {
        if (obj.TryGetValue(propertyName, out JToken? token))
        {
            if (token is JArray array)
            {
                List<TResult?> results = [];

                foreach (T? item in array.Values<T>())
                {
                    results.Add(converter(item));
                }

                container[propertyName] = results;
                return;
            }

            T? value = token.Value<T>();
            container[propertyName] = converter(value);
        }
    }

    internal static void AddArrayOrScalarTo<T>(this JObject obj, string propertyName, IDictionary<string, object?> container) =>
        obj.AddArrayOrScalarTo<T, T>(propertyName, container, value => value);

    internal static string ToLowerString<TEnum>(this TEnum @enum)
        where TEnum : struct
    {
        return @enum.ToString()!.ToLowerInvariant();
    }

    internal static string? ToLowerString<TEnum>(this TEnum? @enum)
        where TEnum : struct
    {
        return @enum.HasValue ? @enum.ToString()!.ToCamelCase() : null;
    }

    internal static string ToCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        char[] chars = value.ToCharArray();
        chars[0] = char.ToLowerInvariant(chars[0]);
        return new string(chars);
    }

    [GeneratedRegex(@"\d+", RegexOptions.Compiled)]
    private static partial Regex AbsoluteFillRegex();

    [GeneratedRegex(@"[\+-]\d+", RegexOptions.Compiled)]
    private static partial Regex RelativeFillRegex();

    [GeneratedRegex(@"start|end|origin", RegexOptions.Compiled)]
    private static partial Regex BoundaryFillRegex();

    [GeneratedRegex(@"false|true", RegexOptions.Compiled)]
    private static partial Regex DisabledFillRegex();

    [GeneratedRegex(@"\{\s*""?value""?\s*:\s*(?<Value>\d+)\}", RegexOptions.Compiled)]
    private static partial Regex AxisValueFillRegex();

    [GeneratedRegex(@"shape", RegexOptions.Compiled)]
    private static partial Regex ShapeFillRegex();

    [GeneratedRegex(@"stack", RegexOptions.Compiled)]
    private static partial Regex StackedValueBelowFillRegex();
}
