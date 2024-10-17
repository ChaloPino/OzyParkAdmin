using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Shared;
using System.Data;
using System.Globalization;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals;
internal static class DataTableExtensions
{
    private static readonly Dictionary<AggregationType, Func<IList<DataRow>, ColumnBase, object?>> SumarizeFunctions = new()
    {
        { AggregationType.Sum, Sum },
        { AggregationType.Average, Average },
        { AggregationType.Count, Count },
        { AggregationType.Min, Min },
        { AggregationType.Max, Max }
    };

    private static readonly Dictionary<DbType, Func<IEnumerable<DataRow>, ColumnBase, object>> NumericSums = new()
    {
        { DbType.Byte, (rows, column) => rows.Sum(r => Convert.ToByte(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Int16, (rows, column) => rows.Sum(r => Convert.ToInt16(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Int32, (rows, column) => rows.Sum(r => Convert.ToInt32(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Int64, (rows, column) => rows.Sum(r => Convert.ToInt64(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Decimal, (rows, column) => rows.Sum(r => Convert.ToDecimal(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Double, (rows, column) => rows.Sum(r => Convert.ToDouble(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Single, (rows, column) => rows.Sum(r => Convert.ToSingle(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.SByte, (rows, column) => rows.Sum(r => Convert.ToSByte(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.UInt16, (rows, column) => rows.Sum(r => Convert.ToUInt16(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.UInt32, (rows, column) => rows.Sum(r => Convert.ToUInt32(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.UInt64, (rows, column) => rows.Sum(r => Convert.ToInt64(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Currency, (rows, column) => rows.Sum(r => Convert.ToDecimal(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.VarNumeric, (rows, column) => rows.Sum(r => Convert.ToDecimal(r[column.Name], CultureInfo.InvariantCulture)) }
    };

    private static readonly Dictionary<DbType, Func<IEnumerable<DataRow>, ColumnBase, object>> NumericAverages = new()
    {
        { DbType.Byte, (rows, column) => rows.Average(r => Convert.ToByte(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Int16, (rows, column) => rows.Average(r => Convert.ToInt16(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Int32, (rows, column) => rows.Average(r => Convert.ToInt32(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Int64, (rows, column) => rows.Average(r => Convert.ToInt64(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Decimal, (rows, column) => rows.Average(r => Convert.ToDecimal(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Double, (rows, column) => rows.Average(r => Convert.ToDouble(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Single, (rows, column) => rows.Average(r => Convert.ToSingle(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.SByte, (rows, column) => rows.Average(r => Convert.ToSByte(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.UInt16, (rows, column) => rows.Average(r => Convert.ToUInt16(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.UInt32, (rows, column) => rows.Average(r => Convert.ToUInt32(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.UInt64, (rows, column) => rows.Average(r => Convert.ToInt64(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.Currency, (rows, column) => rows.Average(r => Convert.ToDecimal(r[column.Name], CultureInfo.InvariantCulture)) },
        { DbType.VarNumeric, (rows, column) => rows.Average(r => Convert.ToDecimal(r[column.Name], CultureInfo.InvariantCulture)) }
    };

    public static List<DataRow> Sort(this DataTable dataTable, bool canSort, SortExpressionCollection<DataRow> sortExpressionCollection)
    {
        List<DataRow> rows = [.. dataTable.AsEnumerable()];
        return rows.Sort(canSort, sortExpressionCollection);
    }

    public static List<DataRow> Sort(this List<DataRow> rows, bool canSort, SortExpressionCollection<DataRow> sortExpressionCollection)
    {
        if (canSort)
        {
            rows = sortExpressionCollection.Sort(rows).ToList();
        }

        return rows;
    }

    public static IDictionary<string, object?>? Aggregate<TColumn>(this IList<DataRow> rows, IList<TColumn> columns, bool formatData = true)
        where TColumn : ColumnBase
    {
        if (columns.Any(c => c.AggregationType.HasValue))
        {
            Dictionary<string, object?> totals = [];

            foreach (TColumn column in columns.Where(c => c.AggregationType.HasValue))
            {
                if (SumarizeFunctions.TryGetValue(column.AggregationType!.Value, out Func<IList<DataRow>, ColumnBase, object?>? function))
                {
                    object? value = function(rows, column);

                    if (value is null)
                    {
                        totals.Add(column.Name, null);
                    }
                    else if (!string.IsNullOrEmpty(column.Format) && formatData)
                    {
                        string svalue = column.DoFormat(value);
                        totals.Add(column.Name, svalue);
                    }
                    else
                    {
                        totals.Add(column.Name, value);
                    }
                }
            }

            return totals;
        }

        return null;
    }

    private static object? Sum(IList<DataRow> rowCollection, ColumnBase column)
    {
        return NumericSums.TryGetValue(column.Type, out Func<IEnumerable<DataRow>, ColumnBase, object?>? sum)
            ? sum(rowCollection, column)
            : null;
    }

    private static object? Average(IList<DataRow> rowCollection, ColumnBase column)
    {
        return NumericAverages.TryGetValue(column.Type, out Func<IEnumerable<DataRow>, ColumnBase, object?>? average)
            ? average(rowCollection, column)
            : null;
    }

    private static object? Count(IList<DataRow> rowCollection, ColumnBase column)
    {
        return rowCollection.Count(r => r[column.Name] != null);
    }

    private static object? Min(IList<DataRow> rowCollection, ColumnBase column)
    {
        return rowCollection.Min(r => r[column.Name]);
    }

    private static object? Max(IList<DataRow> rowCollection, ColumnBase column)
    {
        return rowCollection.Max(r => r[column.Name]);
    }
}
