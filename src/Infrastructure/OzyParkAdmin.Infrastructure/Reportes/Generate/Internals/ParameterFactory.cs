using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Data;
using OzyParkAdmin.Domain.Reportes.DataSources;
using System.Collections;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals;

internal static class ParameterFactory
{
    internal static void SetParameter(Parameter parameter, DbParameter dbParameter, object? value)
    {
        if (dbParameter is SqlParameter sqlParameter && parameter.AlternativeType is not null)
        {
            sqlParameter.SqlDbType = (SqlDbType)parameter.AlternativeType.Value;
            if (parameter.AlternativeTypeName is not null)
            {
                string[] parts = parameter.AlternativeTypeName.Split(':');
                sqlParameter.TypeName = parts[0].Trim();

                dbParameter.Value = CreateTableValue(parts[1].Trim(), value);
                return;
            }

        }

        dbParameter.Value = value;
    }

    private static object? CreateTableValue(string fieldName, object? value)
    {
        DataTable table = new();
        _ = table.Columns.Add(fieldName);

        if (value is not null)
        {
            if (value is IEnumerable enumerable)
            {
                IEnumerator enumerator = enumerable.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    table.Rows.Add(enumerator.Current);
                }
            }
            else
            {
                table.Rows.Add(value);
            }
        }

        return table;
    }
}