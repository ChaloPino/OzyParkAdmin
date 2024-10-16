using System.Diagnostics.CodeAnalysis;

namespace OzyParkAdmin.Domain.Reportes.Pivoted;

/// <summary>
/// Los nombres de las agregaciones para el reporte tipo pivote.
/// </summary>
public static class PivotAggregationNames
{
    private static readonly List<string> AggregatorFactoryHeaderFormats =
    [
        AverageAggregatorFactoryHeaderFormat,
        CountAggregatorFactoryHeaderFormat,
        MaxAggregatorFactoryHeaderFormat,
        MinAggregatorFactoryHeaderFormat,
        SumAggregatorFactoryHeaderFormat
    ];

    /// <summary>
    /// El formato para la cabecera del promedio.
    /// </summary>
    public static string AverageAggregatorFactoryHeaderFormat => PivotResources.AverageAggregatorFactoryHeaderFormat;

    /// <summary>
    /// El formato para la cabecera del conteo.
    /// </summary>
    public static string CountAggregatorFactoryHeaderFormat => PivotResources.CountAggregatorFactoryHeaderFormat;

    /// <summary>
    /// El formato para la cabecera del máximo.
    /// </summary>
    public static string MaxAggregatorFactoryHeaderFormat => PivotResources.MaxAggregatorFactoryHeaderFormat;

    /// <summary>
    /// El formato para la cabecera del máximo.
    /// </summary>
    public static string MinAggregatorFactoryHeaderFormat => PivotResources.MinAggregatorFactoryHeaderFormat;

    /// <summary>
    /// El formato para la cabecera de la suma.
    /// </summary>
    public static string SumAggregatorFactoryHeaderFormat => PivotResources.SumAggregatorFactoryHeaderFormat;

    /// <summary>
    /// Trata de conseguir el formato para la cabecera de una agregación.
    /// </summary>
    /// <param name="aggregatorName">El nombre de la agregación..</param>
    /// <param name="aggregatorFactoryHeaderFormat">El formato para la cabecera de la agregación.</param>
    /// <returns><c>true</c> si se encuentra el formato para la cabecera de la agregación; en caso contrario, <c>false</c>.</returns>
    public static bool TryGetAggregatorFactoryHeaderFormat(string aggregatorName, [NotNullWhen(true)] out string? aggregatorFactoryHeaderFormat)
    {
        aggregatorFactoryHeaderFormat = AggregatorFactoryHeaderFormats.Find(x => string.Equals(x, aggregatorName, StringComparison.OrdinalIgnoreCase));
        return aggregatorFactoryHeaderFormat is not null;
    }
}
