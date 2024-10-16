namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;

internal class HeaderPartValue
{
    private readonly string _fieldName;
    public HeaderPartValue(string fieldName)
    {
        _fieldName = fieldName;
    }

    public Func<object, string?, object?> HeaderName(Func<object, string?, object?> getValue)
    {
        return (item, fieldName) =>
        {
            object? value = getValue(item, _fieldName ?? fieldName);
            return value;
        };
    }
}