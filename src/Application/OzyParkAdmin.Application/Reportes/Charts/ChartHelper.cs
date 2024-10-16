using OzyParkAdmin.Domain.Reportes.Charts;
using System.Data;
using System.Globalization;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Reportes.Charts;
internal static class ChartHelper
{
    private static object? ExtractValue(object? value) =>
        value is DBNull ? null : value;

    internal static DataTable? ExtractDataTable(DataSet dataSet, int? index, string tableName) =>
        index is not null ? dataSet.Tables[index.Value] : dataSet.Tables[tableName];

    internal static void ExtractColumnsAsLabels(DataTable? dataTable, List<object?> labels)
    {
        if ( dataTable is not null)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                labels.Add(column.ColumnName);
            }
        }
    }

    internal static ChartTableInfo? ExtractDataForTable(Chart chart, ChartReport report, DataSet dataSet, ClaimsPrincipal user)
    {
        ChartTableInfo? tableInfo = null;
        DataTable? dataTable = ExtractDataTable(dataSet, chart.TableDataSourceIndex, chart.TableDataSource?.Name ?? report.DataSource.Name);

        if (dataTable is not null)
        {
            List<DataInfo> data = [];
            List<ColumnInfo> columns = chart.Columns
                .Where(c => c.IsAccessibleByUser(user))
                .Select(ColumnInfo.FromColumn)
                .ToList();

            foreach (var row in dataTable.AsEnumerable())
            {
                DataInfo dataInfo = new();

                foreach (ColumnInfo column in columns)
                {
                    dataInfo[column] = row[column.Name];
                }

                data.Add(dataInfo);
            }

            tableInfo = new()
            {
                Colummns = columns,
                Data = data,
            };
        }

        return tableInfo;
    }

    internal static void ExtractData(DataTable? dataTable, string? columnName, List<object?> list)
    {
        if (dataTable is not null)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if (string.IsNullOrEmpty(columnName))
                {
                    list.Add(ExtractValue(row[0]));
                    continue;
                }

                list.Add(ExtractValue(row[columnName]));
            }
        }
    }

    internal static ChartDataValueInfo ExtractData(DataTable? dataTable, ChartDataSet chartDataSet)
    {
        return dataTable is not null
            ? chartDataSet.DataType switch
            {
                ChartDataType.Primitive => ExtractDataAsPrimitive(dataTable, chartDataSet.ColumnName, chartDataSet.ToFilterCriteria(dataTable)),
                ChartDataType.Array => ExtractDataAsArray(dataTable, chartDataSet.XColumnName, chartDataSet.YColumnName, chartDataSet.ToFilterCriteria(dataTable)),
                ChartDataType.CustomArray => ExtractDataAsCustom(dataTable, chartDataSet.ToFilterCriteria(dataTable)),
                ChartDataType.Object => ExtractDataAsObject(dataTable, chartDataSet.ToFilterCriteria(dataTable)),
                ChartDataType.ObjectToArray => ExtractDataAsObjectToArray(dataTable, chartDataSet.ToFilterCriteria(dataTable)),
                _ => new ChartPrimitiveValueInfo(),
            }
            : (ChartDataValueInfo)new ChartPrimitiveValueInfo();
    }

    private static void ExtractData(DataTable dataTable, string? columnName, FilterCriteria? filterCriteria, List<object?> list)
    {
        foreach (DataRow row in dataTable.Rows)
        {
            if (filterCriteria?.IsValid(row) != false)
            {
                if (string.IsNullOrEmpty(columnName))
                {
                    list.Add(ExtractValue(row[0]));
                    continue;
                }

                list.Add(ExtractValue(row[columnName]));
            }
        }
    }

    private static void ExtractXYData(DataTable dataTable, string? xColumnName, string? yColumnName, FilterCriteria? filterCriteria, List<Dictionary<string, object?>> list)
    {
        foreach (DataRow row in dataTable.Rows)
        {
            if (filterCriteria?.IsValid(row) != false)
            {
                Dictionary<string, object?> xyDictionary = [];

                if (string.IsNullOrEmpty(xColumnName))
                {
                    xyDictionary.Add("x", ExtractValue(row[0]));
                }
                else
                {
                    xyDictionary.Add("x", ExtractValue(row[xColumnName]));
                }

                if (string.IsNullOrEmpty(yColumnName))
                {
                    xyDictionary.Add("y", ExtractValue(row[1]));
                }
                else
                {
                    xyDictionary.Add("y", ExtractValue(row[yColumnName]));
                }

                list.Add(xyDictionary);
            }
        }
    }

    private static void ExtractCustomData(DataTable dataTable, FilterCriteria? filterCriteria, List<Dictionary<string, object?>> list)
    {
        foreach (DataRow row in dataTable.Rows)
        {
            if (filterCriteria?.IsValid(row) != false)
            {
                Dictionary<string, object?> custom = [];

                foreach (DataColumn column in dataTable.Columns)
                {
                    custom.Add(column.ColumnName, ExtractValue(row[column.ColumnName]));
                }

                list.Add(custom);
            }
        }
    }

    private static void ExtractComplexData(DataTable dataTable, FilterCriteria? filterCriteria, Dictionary<string, object?> complex)
    {
        foreach (DataColumn column in dataTable.Columns)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if (filterCriteria?.IsValid(row) != false)
                {
                    complex.Add(column.ColumnName, ExtractValue(row[column]));
                }
            }
        }
    }

    private static void ExtractComplexDataToArray(DataTable dataTable, FilterCriteria? filterCriteria, List<object?> obj)
    {
        foreach (DataRow row in dataTable.Rows)
        {
            if (filterCriteria is null || filterCriteria.IsValid(row))
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    obj.Add(ExtractValue(row[column]));
                }
            }
        }
    }

    private static ChartDataValueInfo ExtractDataAsPrimitive(DataTable dataTable, string? columnName, FilterCriteria? filterCiteria)
    {
        ChartPrimitiveValueInfo primitives = [];
        ExtractData(dataTable, columnName, filterCiteria, primitives);
        return primitives;
    }

    private static ChartDataValueInfo ExtractDataAsArray(DataTable dataTable, string? xColumnName, string? yColumnName, FilterCriteria? filterCiteria)
    {
        ChartComplexListValueInfo array = [];
        ExtractXYData(dataTable, xColumnName, yColumnName, filterCiteria, array);
        return array;
    }

    private static ChartDataValueInfo ExtractDataAsCustom(DataTable dataTable, FilterCriteria? filterCiteria)
    {
        ChartComplexListValueInfo custom = [];
        ExtractCustomData(dataTable, filterCiteria, custom);
        return custom;
    }

    private static ChartDataValueInfo ExtractDataAsObject(DataTable dataTable, FilterCriteria? filterCiteria)
    {
        ChartComplexValueInfo objects = [];
        ExtractComplexData(dataTable, filterCiteria, objects);
        return objects;
    }

    private static ChartDataValueInfo ExtractDataAsObjectToArray(DataTable dataTable, FilterCriteria? filterCriteria)
    {
        ChartPrimitiveValueInfo complex = [];
        ExtractComplexDataToArray(dataTable, filterCriteria, complex);
        return complex;
    }

    private static FilterCriteria? ToFilterCriteria(this ChartDataSet chartDataSet, DataTable dataTable)
    {
        return !string.IsNullOrEmpty(chartDataSet.FilterColumnName)
            ? new(chartDataSet.FilterColumnName, chartDataSet.FilterValue, dataTable)
            : null;
    }

    private sealed record FilterCriteria
    {
        public FilterCriteria(string columnName, string? value, DataTable dataTable)
        {
            ColumnName = columnName;
            Type type = dataTable.Columns[columnName]!.DataType;
            Value = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }

        public string ColumnName { get; set; }
        public object? Value { get; set; }

        public bool IsValid(DataRow row)
        {
            object value = row[ColumnName];

            return Equals(value, Value);
        }
    }
}
