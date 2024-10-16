using System.Collections;
using System.Data;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;

internal class DataTableSource(IEnumerable<DataRow> rows) : IPivotDataSource
{
    private readonly IEnumerable<DataRow> _rows = rows;

    private static object? Read(object obj, string? str)
    {
        if (obj is DataRow row && str is not null)
        {
            object? item = row[str];

            return DBNull.Value.Equals(item) ? null : item;
        }

        return null;
    }

    public void ReadData(Action<IEnumerable, Func<object, string?, object?>> handler)
    {
        handler(_rows, Read);
    }
}