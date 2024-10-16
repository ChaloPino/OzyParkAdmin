using OzyParkAdmin.Domain.Reportes.Filters;
using System.Data;
using System.Globalization;

namespace OzyParkAdmin.Infrastructure.Reportes.Internals;
internal static class ParameterUtils
{
    internal static object? ConvertValue(object? value, DbType type, Filter filter, bool otherValues)
    {
        value = !otherValues ? filter.GetValue(value) : filter.GetOtherValue(value);

        if (value is null)
        {
            return DBNull.Value;
        }

        object? result = type switch
        {
            DbType.Guid => new Guid(value.ToString()!),
            DbType.Int32 => Convert.ToInt32(value, CultureInfo.CurrentCulture),
            DbType.Int64 => Convert.ToInt64(value, CultureInfo.CurrentCulture),
            DbType.Int16 => Convert.ToInt16(value, CultureInfo.CurrentCulture),
            DbType.Byte => Convert.ToByte(value, CultureInfo.CurrentCulture),
            DbType.Boolean => Convert.ToBoolean(value, CultureInfo.CurrentCulture),
            DbType.Decimal => Convert.ToDecimal(value, CultureInfo.CurrentCulture),
            DbType.Double => Convert.ToDouble(value, CultureInfo.CurrentCulture),
            DbType.Single => Convert.ToDouble(value, CultureInfo.CurrentCulture),
            DbType.String or DbType.StringFixedLength or DbType.AnsiString or DbType.AnsiStringFixedLength => value,
            DbType.Date or DbType.DateTime or DbType.DateTime2 => DateTime.Parse(value.ToString()!, CultureInfo.CurrentCulture, DateTimeStyles.RoundtripKind),
            DbType.DateTimeOffset => DateTimeOffset.Parse(value.ToString()!, CultureInfo.CurrentCulture, DateTimeStyles.RoundtripKind),
            DbType.Time => TimeSpan.Parse(value.ToString()!, CultureInfo.CurrentCulture),
            _ => value,
        };

        return filter.AdjustValue(result);
    }
}
