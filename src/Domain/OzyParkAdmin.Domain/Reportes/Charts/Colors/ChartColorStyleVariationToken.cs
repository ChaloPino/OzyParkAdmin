namespace OzyParkAdmin.Domain.Reportes.Charts.Colors;

/// <summary>
/// El token de variación para los estilos de colores.
/// </summary>
public readonly struct ChartColorStyleVariationToken
{
    private enum OperationType
    {
        Default,
        ModulateOrIncrement,
        OffsetOrReduce,
    }

    private enum TargetType
    {
        Saturation,
        Luminance,
        Hue,
        Alpha,
    }

    private ChartColorStyleVariationToken(double value, OperationType operation, TargetType target)
    {
        Value = value;
        Operation = operation;
        Target = target;
    }

    /// <summary>
    /// El valor que se va a aplicar.
    /// </summary>
    public double Value { get; }

    private OperationType Operation { get; }
    private TargetType Target { get; }

    internal IChartColor Apply(IChartColor chartColor)
    {
        return Target switch
        {
            TargetType.Saturation => ApplySaturation(chartColor),
            TargetType.Luminance => ApplyLuminance(chartColor),
            TargetType.Hue => ApplyHue(chartColor),
            TargetType.Alpha => ApplyAlpha(chartColor),
            _ => chartColor,
        };
    }

    private IChartColor ApplyAlpha(IChartColor chartColor)
    {
        RgbChartColor rgbChartColor = chartColor.ToRgb();

        return Operation switch
        {
            OperationType.Default => rgbChartColor.Transparency(Value),
            OperationType.OffsetOrReduce => rgbChartColor.SubtractTransparency(Value),
            OperationType.ModulateOrIncrement => rgbChartColor.AddTransparency(Value),
            _ => chartColor,
        };
    }

    private IChartColor ApplySaturation(IChartColor chartColor)
    {
        HslChartColor hslChartColor = chartColor.ToHsl();

        return Operation switch
        {
            OperationType.Default => new HslChartColor(hslChartColor.Hue, Value, hslChartColor.Light),
            OperationType.OffsetOrReduce => hslChartColor.SaturateOffset(Value),
            OperationType.ModulateOrIncrement => hslChartColor.SaturateModulate(Value),
            _ => chartColor,
        };
    }

    private IChartColor ApplyLuminance(IChartColor chartColor)
    {
        HslChartColor hslChartColor = chartColor.ToHsl();

        return Operation switch
        {
            OperationType.Default => new HslChartColor(hslChartColor.Hue, hslChartColor.Saturation, Value),
            OperationType.OffsetOrReduce => hslChartColor.LuminateOffset(Value),
            OperationType.ModulateOrIncrement => hslChartColor.LuminateModulate(Value),
            _ => chartColor,
        };
    }

    private IChartColor ApplyHue(IChartColor chartColor)
    {
        HslChartColor hslChartColor = chartColor.ToHsl();

        return Operation switch
        {
            OperationType.Default => new HslChartColor(Value, hslChartColor.Saturation, hslChartColor.Light),
            OperationType.OffsetOrReduce => hslChartColor.HueOffset(Value),
            OperationType.ModulateOrIncrement => hslChartColor.HueModulate(Value),
            _ => chartColor,
        };
    }

    /// <summary>
    /// Crea un token de variación para iluminar un color.
    /// </summary>
    /// <param name="value">El valor de iluminación.</param>
    /// <returns>El <see cref="ChartColorStyleVariationToken"/> para iluminar.</returns>
    public static ChartColorStyleVariationToken Luminance(double value)
    {
        return new ChartColorStyleVariationToken(value, OperationType.Default, TargetType.Luminance);
    }

    /// <summary>
    /// Crea un token de variación para dispersar la iluminación de un color.
    /// </summary>
    /// <param name="value">El valor de dispersión de la iluminación.</param>
    /// <returns>El <see cref="ChartColorStyleVariationToken"/> para dispersar la iluminación.</returns>
    public static ChartColorStyleVariationToken LuminanceOffset(double value)
    {
        return new ChartColorStyleVariationToken(value, OperationType.OffsetOrReduce, TargetType.Luminance);
    }

    /// <summary>
    /// Crea un token de variación para modular la iluminación de un color.
    /// </summary>
    /// <param name="value">El valor de modulación de la iluminación.</param>
    /// <returns>El <see cref="ChartColorStyleVariationToken"/> para modular la iluminación.</returns>
    public static ChartColorStyleVariationToken LuminanceModulate(double value)
    {
        return new ChartColorStyleVariationToken(value, OperationType.ModulateOrIncrement, TargetType.Luminance);
    }

    /// <summary>
    /// Crea un token de variación para saturar un color.
    /// </summary>
    /// <param name="value">El valor de saturación.</param>
    /// <returns>El <see cref="ChartColorStyleVariationToken"/> para saturar.</returns>
    public static ChartColorStyleVariationToken Saturation(double value)
    {
        return new ChartColorStyleVariationToken(value, OperationType.Default, TargetType.Saturation);
    }

    /// <summary>
    /// Crea un token de variación para dispersar la saturación de un color.
    /// </summary>
    /// <param name="value">El valor de dispersión de la saturación.</param>
    /// <returns>El <see cref="ChartColorStyleVariationToken"/> para dispersar la saturación.</returns>
    public static ChartColorStyleVariationToken SaturationOffset(double value)
    {
        return new ChartColorStyleVariationToken(value, OperationType.OffsetOrReduce, TargetType.Saturation);
    }

    /// <summary>
    /// Crea un token de variación para modular la saturación de un color.
    /// </summary>
    /// <param name="value">El valor de modulación de la saturación.</param>
    /// <returns>El <see cref="ChartColorStyleVariationToken"/> para modular la saturación.</returns>
    public static ChartColorStyleVariationToken SaturationModulate(double value)
    {
        return new ChartColorStyleVariationToken(value, OperationType.ModulateOrIncrement, TargetType.Saturation);
    }

    /// <summary>
    /// Crea un token de variación para establecer la transparencia de un color.
    /// </summary>
    /// <param name="value">El valor de transparencia.</param>
    /// <returns>El <see cref="ChartColorStyleVariationToken"/> para establecer la transparencia.</returns>
    public static ChartColorStyleVariationToken Transparency(double value)
    {
        return new ChartColorStyleVariationToken(value, OperationType.Default, TargetType.Alpha);
    }

    /// <summary>
    /// Crea un token de variación para incrementar transparencia de un color.
    /// </summary>
    /// <param name="value">El valor de transparencia.</param>
    /// <returns>El <see cref="ChartColorStyleVariationToken"/> para incrementar transparencia.</returns>
    public static ChartColorStyleVariationToken IncrementTransparency(double value)
    {
        return new ChartColorStyleVariationToken(value, OperationType.ModulateOrIncrement, TargetType.Alpha);
    }

    /// <summary>
    /// Crea un token de variación para reducir transparencia de un color.
    /// </summary>
    /// <param name="value">El valor de transparencia.</param>
    /// <returns>El <see cref="ChartColorStyleVariationToken"/> para reducir transparencia.</returns>
    public static ChartColorStyleVariationToken ReduceTransparency(double value)
    {
        return new ChartColorStyleVariationToken(value, OperationType.OffsetOrReduce, TargetType.Alpha);
    }

    /// <summary>
    /// Crea un token de variación para modular el hue de un color.
    /// </summary>
    /// <param name="value">El valor de modulación del hue.</param>
    /// <returns>El <see cref="ChartColorStyleVariationToken"/> para modular del hue.</returns>
    public static ChartColorStyleVariationToken HueModulate(double value)
    {
        return new ChartColorStyleVariationToken(value, OperationType.ModulateOrIncrement, TargetType.Hue);
    }

    /// <summary>
    /// Crea un token de variación para dispersar el hue de un color.
    /// </summary>
    /// <param name="value">El valor de dispersión del hue.</param>
    /// <returns>El <see cref="ChartColorStyleVariationToken"/> para dispersar el hue.</returns>
    public static ChartColorStyleVariationToken HueOffset(double value)
    {
        return new ChartColorStyleVariationToken(value, OperationType.OffsetOrReduce, TargetType.Hue);
    }

    /// <summary>
    /// Crea un token de variación para establecer el hue de un color.
    /// </summary>
    /// <param name="value">El valor del hue.</param>
    /// <returns>El <see cref="ChartColorStyleVariationToken"/> para establecer el hue.</returns>
    public static ChartColorStyleVariationToken Hue(double value)
    {
        return new ChartColorStyleVariationToken(value, OperationType.Default, TargetType.Hue);
    }
}
