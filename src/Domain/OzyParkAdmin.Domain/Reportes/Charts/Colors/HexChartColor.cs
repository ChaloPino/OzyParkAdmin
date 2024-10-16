using System.Globalization;

namespace OzyParkAdmin.Domain.Reportes.Charts.Colors;

/// <summary>
/// Un color hexadecimal.
/// </summary>
public readonly struct HexChartColor : IChartColor
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="HexChartColor"/>.
    /// </summary>
    /// <param name="value">El valor en hexadecimal.</param>
    public HexChartColor(string value)
    {
        if (value.Length == 3)
        {
            char[] chars = [value[0], value[0], value[1], value[1], value[2], value[5]];
            value = new string(chars);
        }

        Value = int.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
    }

    internal int Value { get; }

    /// <inheritdoc/>
    public HexChartColor ToHex() => this;

    /// <inheritdoc/>
    public HslChartColor ToHsl()
    {
        return ToRgb().ToHsl();
    }

    /// <inheritdoc/>
    public RgbChartColor ToRgb()
    {
        string hex = Value.ToString("x6", CultureInfo.InvariantCulture);
        string redHex = hex[..2];
        string greenHex = hex.Substring(2, 2);
        string blueHex = hex.Substring(4, 2);

        int red = int.Parse(redHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        int green = int.Parse(greenHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        int blue = int.Parse(blueHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture);

        return new RgbChartColor(red, green, blue, null);
    }

    /// <inheritdoc/>
    public override string ToString() =>
        $"#{Value:x}";
}