using System.Globalization;

namespace OzyParkAdmin.Domain.Reportes.Charts.Colors;

/// <summary>
/// Un color hsl.
/// </summary>
public readonly struct HslChartColor : IChartColor
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="HslChartColor"/>.
    /// </summary>
    /// <param name="hue">El hue del color.</param>
    /// <param name="saturation">La saturación del color.</param>
    /// <param name="light">La luminosidad del color.</param>
    public HslChartColor(double hue, double saturation, double light)
    {
        if (hue < 0 || hue > 360.0)
        {
            throw new ArgumentOutOfRangeException(nameof(hue));
        }

        if (saturation < 0.0 || saturation > 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(saturation));
        }

        if (light < 0.0 || light > 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(light));
        }

        Hue = hue;
        Saturation = saturation;
        Light = light;
    }

    internal double Hue { get; }
    internal double Saturation { get; }
    internal double Light { get; }

    /// <summary>
    /// Modula el hue del color hsl.
    /// </summary>
    /// <param name="value">El valor a modular.</param>
    /// <returns>El color <see cref="HslChartColor"/> modulado.</returns>
    public HslChartColor HueModulate(double value)
    {
        return new HslChartColor(Hue * value, Saturation, Light);
    }

    /// <summary>
    /// Dispersa el hue del color hsl.
    /// </summary>
    /// <param name="value">El valor a dispersar.</param>
    /// <returns>El color <see cref="HslChartColor"/> dispersado.</returns>
    public HslChartColor HueOffset(double value)
    {
        return new HslChartColor(Hue + value, Saturation, Light);
    }

    /// <summary>
    /// Modula la iluminación del color hsl.
    /// </summary>
    /// <param name="value">El valor a modular.</param>
    /// <returns>El color <see cref="HslChartColor"/> modulado.</returns>
    public HslChartColor LuminateModulate(double value)
    {
        return new HslChartColor(Hue, Saturation, Light * value);
    }

    /// <summary>
    /// Dispersa la iluminación del color hsl.
    /// </summary>
    /// <param name="value">El valor a dispersar.</param>
    /// <returns>El color <see cref="HslChartColor"/> dispersado.</returns>
    public HslChartColor LuminateOffset(double value)
    {
        return new HslChartColor(Hue, Saturation, Light + value);
    }

    /// <summary>
    /// Modula la saturación del color hsl.
    /// </summary>
    /// <param name="value">El valor a modular.</param>
    /// <returns>El color <see cref="HslChartColor"/> modular.</returns>
    public HslChartColor SaturateModulate(double value)
    {
        return new HslChartColor(Hue, Saturation * value, Light);
    }

    /// <summary>
    /// Dispersa la saturación del color hsl.
    /// </summary>
    /// <param name="value">El valor a dispersar.</param>
    /// <returns>El color <see cref="HslChartColor"/> dispersado.</returns>
    public HslChartColor SaturateOffset(double value)
    {
        return new HslChartColor(Hue, Saturation + value, Light);
    }

    /// <inheritdoc/>
    public HexChartColor ToHex()
    {
        return ToRgb().ToHex();
    }

    /// <inheritdoc/>
    public HslChartColor ToHsl() => this;

    /// <inheritdoc/>
    public RgbChartColor ToRgb()
    {
        double c = (1 - Math.Abs((2 * Light) - 1)) * Saturation;
        double x = c * (1 - Math.Abs(Hue / 60 % 2) - 1);
        double m = Light - (c / 2);

        double rp;
        double gp;
        double bp;

        if (0 <= Hue && Hue < 60)
        {
            rp = c;
            gp = x;
            bp = 0;
        }
        else if (60 <= Hue && Hue < 120)
        {
            rp = x;
            gp = c;
            bp = 0;
        }
        else if (120 <= Hue && Hue < 180)
        {
            rp = 0;
            gp = c;
            bp = x;
        }
        else if (180 <= Hue && Hue < 240)
        {
            rp = 0;
            gp = x;
            bp = c;
        }
        else if (240 <= Hue && Hue < 300)
        {
            rp = x;
            gp = 0;
            bp = c;
        }
        else
        {
            rp = c;
            gp = 0;
            bp = x;
        }

        int red = (int)Math.Round((rp + m) * 255, 0);
        int green = (int)Math.Round((gp + m) * 255, 0);
        int blue = (int)Math.Round((bp + m) * 255, 0);

        return new RgbChartColor(red, green, blue, null);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        FormattableString formattableString = $"hsl({Hue}, {Saturation:P0}, {Light:P0})";
        return formattableString.ToString(CultureInfo.InvariantCulture);
    }
}