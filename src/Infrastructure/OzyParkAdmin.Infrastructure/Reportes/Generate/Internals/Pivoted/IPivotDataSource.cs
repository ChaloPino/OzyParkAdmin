using System.Collections;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;

internal interface IPivotDataSource
{
    void ReadData(Action<IEnumerable, Func<object, string?, object?>> handler);
}