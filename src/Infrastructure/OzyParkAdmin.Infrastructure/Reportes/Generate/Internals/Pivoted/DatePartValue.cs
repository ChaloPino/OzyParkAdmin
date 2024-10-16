namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;
internal class DatePartValue
{
    private readonly string? _fieldName;

    public DatePartValue() { }

    public DatePartValue(string? fieldName)
    {
        _fieldName = fieldName;
    }

    public Func<object, string?, object?> DateOnlyHandler(Func<object, string?, object?> getValue)
    {
        return (item, fieldName) =>
        {
            object? value = getValue(item, _fieldName ?? fieldName);

            return value is not DateTime dateTime ? null : dateTime.Date;
        };
    }

    public Func<object, string?, object?> DayHandler(Func<object, string?, object?> getValue)
    {
        return (item, fieldName) =>
        {
            object? value = getValue(item, _fieldName ?? fieldName);

            return value is not DateTime dateTime ? null : dateTime.Day;
        };
    }

    public Func<object, string?, object?> MonthNumberHandler(Func<object, string?, object?> getValue)
    {
        return (item, fieldName) =>
        {
            object? value = getValue(item, _fieldName ?? fieldName);
            return value is not DateTime dateTime ? null : dateTime.Month;
        };
    }

    public Func<object, string?, object?> MonthShortNameHandler(Func<object, string?, object?> getValue)
    {
        return (item, fieldName) =>
        {
            object? value = getValue(item, _fieldName ?? fieldName);
            return value is not DateTime dateTime ? null : (object)dateTime.ToString("MMM");
        };
    }

    public Func<object, string?, object?> MonthLongNameHandler(Func<object, string?, object?> getValue)
    {
        return (item, fieldName) =>
        {
            object? value = getValue(item, _fieldName ?? fieldName);
            return value is not DateTime dateTime ? null : (object)dateTime.ToString("MMMM");
        };
    }

    public Func<object, string?, object?> YearHandler(Func<object, string?, object?> getValue)
    {
        return (item, fieldName) =>
        {
            object? value = getValue(item, _fieldName ?? fieldName);
            return value is not DateTime dateTime ? null : dateTime.Year;
        };
    }

    public Func<object, string?, object?> QuarterHandler(Func<object, string?, object?> getValue)
    {
        int[] quarterMonths = new int[12];

        for (int i = 0; i < quarterMonths.Length; i++)
        {
            quarterMonths[i] = (int)Math.Ceiling((i + 1) / 3f);
        }

        return (item, fieldName) =>
        {
            object? value = getValue(item, _fieldName ?? fieldName);
            return value is not DateTime dateTime ? null : quarterMonths[dateTime.Month - 1];
        };
    }
}
