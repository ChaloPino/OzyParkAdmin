using NReco.PivotData;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Pivoted;
using System.Data;
using System.Globalization;

namespace OzyParkAdmin.Application.Reportes;

/// <summary>
/// Utilitarios de agregación.
/// </summary>
public static class AggregationUtils
{
    private static readonly Dictionary<Type, string> _aggregatorFormatResources = new()
    {
        [typeof(SumAggregatorFactory)] = "SumAggregatorFactoryHeaderFormat",
        [typeof(CountAggregatorFactory)] = "CountAggregatorFactoryHeaderFormat",
        [typeof(MinAggregatorFactory)] = "MinAggregatorFactoryHeaderFormat",
        [typeof(MaxAggregatorFactory)] = "MaxAggregatorFactoryHeaderFormat",
        [typeof(AverageAggregatorFactory)] = "AverageAggregatorFactoryHeaderFormat",

    };

    /// <summary>
    /// Realiza la agregación de una lista de <see cref="DataInfo"/> dependiendo de la columna solicitada.
    /// </summary>
    /// <param name="source">La lista de <see cref="DataInfo"/> a la que se va a ejecutar la agregación.</param>
    /// <param name="column">La <see cref="ColumnInfo"/> para conseguir los datos de agregación.</param>
    /// <returns>El valor agregado.</returns>
    public static object? Aggregate(IEnumerable<DataInfo> source, ColumnInfo column)
    {
        if (column.AggregationType is null)
        {
            return null;
        }

        IEnumerable<object?> values = source.Select(x => x[column]);

        return column.AggregationType.Value switch
        {
            AggregationType.Sum => Sum(values, column),
            AggregationType.Average => Average(values, column),
            AggregationType.Count => Count(values),
            AggregationType.Min => Min(values),
            AggregationType.Max => Max(values),
            _ => null,
        };
    }

    /// <summary>
    /// Formatea la cabecera de la agregación.
    /// </summary>
    /// <param name="aggregator">La agregación a formatear.</param>
    /// <param name="pivotedMembers">La lista de miembros del pivote.</param>
    /// <param name="index">El índice del miemro del pivote a buscar.</param>
    /// <returns>La cabecera de la agregación formateada.</returns>
    public static string? FormatAggregator(this IAggregatorFactory aggregator, IList<PivotedMember> pivotedMembers, int index)
    {
        if (_aggregatorFormatResources.TryGetValue(aggregator.GetType(), out string? resourceName) && 
            PivotAggregationNames.TryGetAggregatorFactoryHeaderFormat(resourceName, out string? format))
        {
            return string.Format(CultureInfo.CurrentCulture, format, pivotedMembers[index].GetFullName());
        }

        return aggregator.ToString();
    }

    private static object? Sum(IEnumerable<object?> source, ColumnInfo column)
    {
        if (column.IsNumericType())
        {
            return SumInteger(source);
        }

        if (column.IsDecimalType())
        {
            return SumDecimal(source);
        }

        return null;
    }

    private static long SumInteger(IEnumerable<object?> source)
    {
        double value = SumDecimal(source);
        return Convert.ToInt64(value);
    }

    private static double SumDecimal(IEnumerable<object?> source)
    {
        return source.Where(o => o is not null).Sum(o => Convert.ToDouble(o, CultureInfo.InvariantCulture));
    }

    private static object? Average(IEnumerable<object?> source, ColumnInfo column)
    {
        if (column.IsNumericType() || column.IsDecimalType())
        {
            return Average(source);
        }

        return null;
    }

    private static double Average(IEnumerable<object?> source)
    {
        return source.Average(o => o is null ? 0.0 : Convert.ToDouble(o, CultureInfo.InvariantCulture));
    }

    private static object? Max(IEnumerable<object?> source) =>
        source.Max();

    private static object? Min(IEnumerable<object?> source) =>
        source.Min();

    private static int Count(IEnumerable<object?> source) =>
        source.Count();
}
