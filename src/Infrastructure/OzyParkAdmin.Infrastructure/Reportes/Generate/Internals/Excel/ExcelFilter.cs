using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Domain.Reportes.Filters;
using System.Globalization;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel;
internal sealed class ExcelFilter(Filter filter, object? value)
{
    private readonly Filter _filter = filter;
    private readonly object? _value = value;

    public string? Label => _filter.Label;

    public ExcelFormat? ExcelFormat => _filter.CurrentExcelFormat;

    internal int StyleId { get; set; }
    internal int HeaderStyleId { get; set; }
    internal int HeaderSharedTable { get; set; }
    internal int? SharedTable { get; set; }

    public string? GetValueAsString()
    {
        return _value is null ? null : _value as string;
    }

    public bool HasTextValue()
    {
        return _value is string;
    }

    internal string? GetValue() =>
        _value switch
        {
            null => null,
            DateTime dateTime => dateTime.ToOADate().ToString(CultureInfo.InvariantCulture),
            TimeSpan timeSpan => timeSpan.TotalDays.ToString(CultureInfo.InvariantCulture),
            long number => number.ToString(CultureInfo.InvariantCulture),
            _ => _value.ToString(),
        };
}
