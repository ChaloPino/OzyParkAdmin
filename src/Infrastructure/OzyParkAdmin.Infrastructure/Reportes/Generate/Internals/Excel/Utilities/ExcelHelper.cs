using DocumentFormat.OpenXml.Spreadsheet;
using OzyParkAdmin.Domain.Reportes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Utilities;
internal static partial class ExcelHelper
{
    private static readonly Dictionary<DbType, Func<object, string?>> Converters = new()
    {
        { DbType.Boolean, (value) => Convert.ToBoolean(value, CultureInfo.InvariantCulture) ? "Sí" : "No" },
        { DbType.Byte, (value) => Convert.ToByte(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) },
        { DbType.Int16, (value) => Convert.ToInt16(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) },
        { DbType.Int32, (value) => Convert.ToInt32(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) },
        { DbType.Int64, (value) => Convert.ToInt64(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) },
        { DbType.Double, (value) => Convert.ToDouble(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) },
        { DbType.Single, (value) => Convert.ToSingle(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) },
        { DbType.SByte, (value) => Convert.ToSByte(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) },
        { DbType.UInt16, (value) => Convert.ToUInt16(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) },
        { DbType.UInt32, (value) => Convert.ToUInt32(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) },
        { DbType.UInt64, (value) => Convert.ToUInt64(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) },
        { DbType.Decimal, ConvertToDecimal },
        { DbType.Currency, ConvertToDecimal },
        { DbType.VarNumeric, ConvertToDecimal },
        { DbType.Date, (value) => Convert.ToDateTime(value, CultureInfo.InvariantCulture).ToOADate().ToString(CultureInfo.InvariantCulture) },
        { DbType.DateTime, (value) => Convert.ToDateTime(value, CultureInfo.InvariantCulture).ToOADate().ToString(CultureInfo.InvariantCulture) },
        { DbType.DateTime2, (value) => Convert.ToDateTime(value, CultureInfo.InvariantCulture).ToOADate().ToString(CultureInfo.InvariantCulture) },
        { DbType.DateTimeOffset, (value) => ((DateTimeOffset)value).Date.ToOADate().ToString(CultureInfo.InvariantCulture) },
        { DbType.Time, (value) => ((TimeSpan)value).TotalDays.ToString(CultureInfo.InvariantCulture) },
        { DbType.Guid, (value) => value.ToString() },
        { DbType.String, (value) => value.ToString() },
        { DbType.StringFixedLength, (value) => value.ToString() },
        { DbType.AnsiString, (value) => value.ToString() },
        { DbType.AnsiStringFixedLength, (value) => value.ToString() }
    };

    private readonly static Dictionary<Type, Func<object, object>> RawConverters = new()
    {
        { typeof(bool), (value) => Convert.ToBoolean(value, CultureInfo.InvariantCulture) ? "Sí" : "No" },
        { typeof(byte), (value) => Convert.ToByte(value, CultureInfo.InvariantCulture) },
        { typeof(short), (value) => Convert.ToInt16(value, CultureInfo.InvariantCulture) },
        { typeof(int), (value) => Convert.ToInt32(value, CultureInfo.InvariantCulture) },
        { typeof(long), (value) => Convert.ToInt64(value, CultureInfo.InvariantCulture) },
        { typeof(double), (value) => Convert.ToDouble(value, CultureInfo.InvariantCulture) },
        { typeof(float), (value) => Convert.ToSingle(value, CultureInfo.InvariantCulture) },
        { typeof(sbyte), (value) => Convert.ToSByte(value, CultureInfo.InvariantCulture) },
        { typeof(ushort), (value) => Convert.ToUInt16(value, CultureInfo.InvariantCulture) },
        { typeof(uint), (value) => Convert.ToUInt32(value, CultureInfo.InvariantCulture) },
        { typeof(ulong), (value) => Convert.ToUInt64(value, CultureInfo.InvariantCulture) },
        { typeof(decimal), ConvertToRawDecimal },
        { typeof(DateTime), (value) => Convert.ToDateTime(value, CultureInfo.InvariantCulture).ToOADate() },
        { typeof(DateTimeOffset), (value) => ((DateTimeOffset)value).Date.ToOADate() },
        { typeof(TimeSpan), (value) => ((TimeSpan)value).TotalDays },
        { typeof(Guid), (value) => value },
        { typeof(string), (value) => value }
    };

    public static Stream GetThemeStream()
    {
        return typeof(ExcelHelper).Assembly.GetManifestResourceStream("OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.ExcelTheme.xml")!;
    }

    public static string? GetValue(DataRow row, ExcelColumn column)
    {
        object value = row[column.Name];

        return value switch
        {
            null => null,
            _ => GetValue(row, column, value),
        };
    }

    private static string? GetValue(DataRow row, ExcelColumn column, object value) =>
        column.IsText() && column.SharedTable.TryGetValue(row, out int? index) && index is not null ? index.Value.ToString(CultureInfo.InvariantCulture) : GetValue(column.Type, value);

    public static string? GetValue(IDictionary<string, object?> totals, ExcelColumn column)
    {
        object? value = totals[column.Name];

        return value switch
        {
            null => null,
            _ => column.IsText() && column.FooterSharedTable is not null ? column.FooterSharedTable.Value.ToString(CultureInfo.InvariantCulture) : GetValue(column.Type, value),
        };
    }

    private static string? GetValue(DbType type, object value)
    {
        return value switch
        {
            null or DBNull => null,
            _ => Converters.TryGetValue(type, out Func<object, string?>? converter) ? converter(value) : value.ToString(),
        };
    }

    public static object? GetRawValue(object value)
    {
        return RawConverters.TryGetValue(value.GetType(), out Func<object, object>? converter) ? converter(value) : value.ToString();
    }

    private static string ConvertToDecimal(object value)
    {
        decimal val = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        decimal integral = Math.Truncate(val);
        return decimal.Subtract(val, integral) == 0M
            ? integral.ToString(CultureInfo.InvariantCulture)
            : val.ToString(CultureInfo.InvariantCulture);
    }

    private static object ConvertToRawDecimal(object value)
    {
        decimal val = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        decimal integral = Math.Truncate(val);
        return decimal.Subtract(val, integral) == 0M
            ? integral
            : (object)val;
    }

    internal static string GetRange(string startColumn, int startRow, string endColumn, int endRow)
    {
        return $"{startColumn}{startRow}:{endColumn}{endRow}";
    }

    internal static string GetColumnLetter(int columnNumber)
    {
        int dividend = columnNumber + 1;
        List<char> chars = [];
        int modulo;

        while (dividend > 0)
        {
            modulo = (dividend - 1) % 26;
            chars.Insert(0, Convert.ToChar(65 + modulo));
            dividend = (dividend - modulo) / 26;
        }

        return new string([.. chars]);
    }

    private static readonly Regex referenceExtractor = ReferenceExtractor();

    internal static bool TryExtractReference(this Cell cell, out int row, out int column)
    {
        row = -1;
        column = -1;

        if (cell.CellReference?.Value is not null)
        {
            Match match = referenceExtractor.Match(cell.CellReference.Value);

            if (match.Success)
            {
                string columnReference = match.Groups[1].Value;
                string rowReference = match.Groups[2].Value;

                row = Convert.ToInt32(rowReference);

                column = ExcelColumnLetterToNumber(columnReference);

                return true;
            }
        }

        return false;
    }

    internal static bool TryExtractReference(this Cell cell, [NotNullWhen(true)] out int? row, [NotNullWhen(true)] out string? column)
    {
        row = -1;
        column = null;

        if (cell.CellReference?.Value is not null)
        {
            Match match = referenceExtractor.Match(cell.CellReference.Value);

            if (match.Success)
            {
                string columnReference = match.Groups[1].Value;
                string rowReference = match.Groups[2].Value;

                row = Convert.ToInt32(rowReference);

                column = columnReference;

                return true;
            }
        }
        return false;
    }

    internal static int GetRow(this Cell cell)
    {
        _ = cell.TryExtractReference(out int row, out _);
        return row;
    }

    internal static int GetColumn(this Cell cell)
    {
        _ = cell.TryExtractReference(out _, out int column);
        return column;
    }

    internal static int CompareTo(this Cell cell1, Cell cell2)
    {
        _ = cell1.TryExtractReference(out int row1, out int col1);
        _ = cell2.TryExtractReference(out int row2, out int col2);

        int compareRows = row1.CompareTo(row2);
        if (compareRows < 0)
        {
            return -1;
        }
        else if (compareRows > 0)
        {
            return 1;
        }
        return col1.CompareTo(col2);
    }

    private static int ExcelColumnLetterToNumber(string columnReference)
    {
        int sum = 0;

        for (int i = 0; i < columnReference.Length; i++)
        {
            sum *= 26;
            sum += columnReference[i] - 'A' + 1;
        }

        return sum - 1;
    }

    internal static double MeasureText(string text)
    {
        return Math.Truncate(((text.Length * 7) + 5) / 7.0 * 256.0) / 256.0;
    }

    internal static string? GetSubtotal(ExcelColumn column, string range)
    {
        return column.AggregationType.HasValue ? $"SUBTOTAL({SumarizationMap[column.AggregationType.Value]},{range})" : null;
    }

    internal static string GetTotalRowFunction(AggregationType sumarizeType)
    {
        return SumarizationRowFunctionMap[sumarizeType];
    }

    private static readonly Dictionary<AggregationType, int> SumarizationMap = new()
    {
        { AggregationType.Sum, 109 },
        { AggregationType.Average, 101 },
        { AggregationType.Count, 103 },
        { AggregationType.Max, 104 },
        { AggregationType.Min, 105 }
    };

    private static readonly Dictionary<AggregationType, string> SumarizationRowFunctionMap = new()
    {
        { AggregationType.Sum, "sum" },
        { AggregationType.Average, "average" },
        { AggregationType.Count, "count" },
        { AggregationType.Max, "max" },
        { AggregationType.Min, "min" }
    };

    [GeneratedRegex(@"^([a-zA-Z]+)(\d+)$")]
    private static partial Regex ReferenceExtractor();
}
