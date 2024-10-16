using System.Collections;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;

internal class DerivedValueSource : IPivotDataSource
{
    private readonly Action<Action<IEnumerable, Func<object, string?, object?>>> readData;
    private readonly Dictionary<string, int> valueHandlerPosition;
    private readonly List<Func<Func<object, string?, object?>, Func<object, string?, object?>>> valueHandlers;
    private readonly static object _lock = new();

    public DerivedValueSource(IPivotDataSource source)
        : this(source.ReadData)
    {
    }

    public DerivedValueSource(Action<Action<IEnumerable, Func<object, string?, object?>>> readData)
    {
        this.readData = readData;
        valueHandlerPosition = [];
        valueHandlers = [];
    }

    protected IEnumerable GetWrappedEnum(IEnumerable data)
    {
        return data.Cast<object>().Select(current =>
        {
            object[] array = new object[valueHandlers.Count + 1];
            array[0] = current;
            return array;
        });
    }

    public void ReadData(Action<IEnumerable, Func<object, string?, object?>> handler)
    {
        readData((enumerable, readData) =>
        {
            Func<object, string?, object?>[] item = new Func<object, string?, object?>[valueHandlers.Count];

            for (int i = 0; i < valueHandlers.Count; i++)
            {
                item[i] = valueHandlers[i](readData);
            }

            handler(GetWrappedEnum(enumerable), (object obj, string? key) =>
            {
                object[] array = (object[])obj;

                if (key is not null)
                {
                    if (!valueHandlerPosition.TryGetValue(key, out int num))
                    {
                        return readData(array[0], key);
                    }

                    object? a1 = array[num + 1];

                    if (a1 != null)
                    {
                        return _lock != a1 ? a1 : null;
                    }

                    a1 = item[num](array[0], key);
                    array[num + 1] = a1 ?? _lock;
                    return a1;
                }

                return null;
            });
        });
    }

    public DerivedValueSource Register(string fieldName, Func<Func<object, string?, object?>, Func<object, string?, object?>> getValueHandler)
    {
        valueHandlerPosition[fieldName] = valueHandlers.Count;
        valueHandlers.Add(getValueHandler);
        return this;
    }
}