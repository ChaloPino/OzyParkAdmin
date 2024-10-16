using System.Globalization;

namespace OzyParkAdmin.Domain.Reportes.Charts.Colors;

/// <summary>
/// Un color rgb.
/// </summary>
public readonly struct RgbChartColor : IChartColor
{
    private const double epsilon = 0.0001;

    /// <summary>
    /// Crea una nueva instancia de <see cref="RgbChartColor"/>.
    /// </summary>
    /// <param name="red">El color rojo.</param>
    /// <param name="green">El color verde.</param>
    /// <param name="blue">El color azul.</param>
    /// <param name="alpha">El alpah.</param>
    public RgbChartColor(int red, int green, int blue, double? alpha)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    internal int Red { get; }
    internal int Green { get; }
    internal int Blue { get; }
    internal double? Alpha { get; }

    /// <inheritdoc/>
    public HexChartColor ToHex()
    {
        string redHex = Red.ToString("X2", CultureInfo.InvariantCulture);
        string greenHex = Green.ToString("X2", CultureInfo.InvariantCulture);
        string blueHex = Blue.ToString("X2", CultureInfo.InvariantCulture);
        return new HexChartColor($"{redHex}{greenHex}{blueHex}");
    }

    /// <inheritdoc/>
    public RgbChartColor ToRgb() => this;

    /// <inheritdoc/>
    public HslChartColor ToHsl()
    {
        double rp = Red / 255.0;
        double gp = Green / 255.0;
        double bp = Blue / 255.0;

        double cmax = Math.Max(Math.Max(rp, gp), bp);
        double cmin = Math.Min(Math.Min(rp, gp), bp);
        double delta = cmax - cmin;

        double h = 0;

        if (delta >= epsilon)
        {
            if ((cmax - rp) < epsilon)
            {
                h = 60 * ((gp - bp) / delta % 6);
            }
            else if ((cmax - gp) < epsilon)
            {
                h = 60 * (((bp - rp) / delta) + 2);
            }
            else if ((cmax - bp) < epsilon)
            {
                h = 60 * (((rp - gp) / delta) + 4);
            }
        }

        double l = (cmax + cmin) / 2;

        double s = delta < epsilon
            ? 0
            : delta / (1 - Math.Abs((2 * l) - 1));

        return new HslChartColor(Math.Round(h, 0), s, l);
    }

    /// <summary>
    /// Le asigna transparencia al color.
    /// </summary>
    /// <param name="alpha">El porcentaje de transparencia.</param>
    /// <returns>El color con transparencia.</returns>
    public RgbChartColor Transparency(double alpha)
    {
        return new RgbChartColor(Red, Green, Blue, alpha);
    }

    /// <summary>
    /// Le añade más transparencia al color.
    /// </summary>
    /// <param name="alpha">El porcentaje de transparencia.</param>
    /// <returns>El color con transparencia.</returns>
    public RgbChartColor AddTransparency(double alpha)
    {
        return Alpha.HasValue
            ? new RgbChartColor(Red, Green, Blue, Alpha.Value + alpha)
            : new RgbChartColor(Red, Green, Blue, alpha);
    }

    /// <summary>
    /// Le quita transparencia al color.
    /// </summary>
    /// <param name="alpha">El porcentaje de transparencia.</param>
    /// <returns>El color con transparencia.</returns>
    public RgbChartColor SubtractTransparency(double alpha)
    {
        return AddTransparency(-alpha);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (Alpha == null)
        {
            return $"rgb({Red}, {Green}, {Blue})";
        }

        FormattableString value = $"rgba({Red}, {Green}, {Blue}, {Alpha:R})";
        return value.ToString(CultureInfo.InvariantCulture);
    }
}