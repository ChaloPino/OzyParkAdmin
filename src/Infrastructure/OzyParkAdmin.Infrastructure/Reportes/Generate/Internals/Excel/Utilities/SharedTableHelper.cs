using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Excel;
using System.Data;
using System.Globalization;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel.Utilities;

internal static class SharedTableHelper
{
    internal static SharedTable CreateSharedTable(ExcelReportTemplate template, IList<ExcelFilter> filters, IList<ExcelColumn> columns, List<DataRow> rows, IDictionary<string, object?>? totals)
    {
        SharedTable sharedTable = new();

        if (template.HasHeader)
        {
            AddSharedString(template, sharedTable);
            AddSharedString(filters, sharedTable);
        }

        AddSharedString(columns, sharedTable);
        AddSharedString(rows, columns, sharedTable);
        AddSharedString(totals, columns, sharedTable);

        return sharedTable;
    }

    internal static List<ExcelColumn> CreateSharedTable(IEnumerable<ColumnBase> columns, ClaimsPrincipal user, SharedTable sharedTable)
    {
        List<ExcelColumn> orderedColumns = columns
            .Where(c => c.IsAccessibleByUser(user))
            .OrderBy(c => c.Order)
            .Select(c => new ExcelColumn(c))
            .ToList();

        AddSharedString(orderedColumns, sharedTable);
        return orderedColumns;
    }

    internal static void CreateSharedTable(DataSet dataSet, int? index, string? name, SharedTable sharedTable, List<ExcelColumn> excelColumns)
    {
        DataTable? dataTable = index.HasValue ? dataSet.Tables[index.Value] : dataSet.Tables[name];

        if (dataTable is not null)
        {
            AddSharedString([.. dataTable.AsEnumerable()], excelColumns, sharedTable);
        }
    }

    internal static SharedTable CreateSharedTable(ExcelReportTemplate template, List<ExcelFilter> filters)
    {
        SharedTable sharedTable = new();

        if (template.HasHeader)
        {
            AddSharedString(template, sharedTable);
            AddSharedString(filters, sharedTable);
        }

        return sharedTable;
    }

    public static void AddSharedString(ExcelReportTemplate template, SharedTable sharedTable)
    {
        if (!string.IsNullOrEmpty(template.HeaderTitle))
        {
            _ = sharedTable.AddString(template.HeaderTitle);
        }
    }

    public static void AddSharedString(IList<ExcelFilter> filters, SharedTable sharedTable)
    {
        foreach (ExcelFilter filter in filters)
        {
            if (!string.IsNullOrEmpty(filter.Label))
            {
                filter.HeaderSharedTable = sharedTable.AddString(filter.Label);
            }
        }

        foreach (ExcelFilter filter in filters.Where(f => f.HasTextValue()))
        {
            string? value = filter.GetValueAsString();

            if (!string.IsNullOrEmpty(value))
            {
                filter.SharedTable = sharedTable.AddString(value);
            }
        }
    }

    public static void AddSharedString(IList<ExcelColumn> columns, SharedTable sharedTable)
    {
        foreach (ExcelColumn column in columns.Where(c => c.Header is not null))
        {
            column.HeaderSharedTable = sharedTable.AddString(column.Header!);
        }
    }

    public static void AddSharedString(List<DataRow> rows, IList<ExcelColumn> columns, SharedTable sharedTable)
    {
        List<ExcelColumn> stringColumns = columns.Where(c => c.IsText()).ToList();
        rows.ForEach(row =>
            stringColumns.ForEach(column =>
            {
                column.SharedTable.Add(row, null);

                string? sValue = null;
                object value = row[column.Name];
                if (value != null)
                {
                    if (column.IsStringType())
                    {
                        sValue = value.ToString();
                    }
                    else if (column.Type == DbType.Boolean)
                    {
                        bool bVal = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                        sValue = bVal ? "Sí" : "No";
                    }
                    if (sValue != null)
                    {
                        column.SharedTable[row] = sharedTable.AddString(sValue);
                    }
                }
            }));
    }

    public static void AddSharedString(IDictionary<string, object?>? totals, IList<ExcelColumn> columns, SharedTable sharedTable)
    {
        if (totals is not null)
        {
            List<ExcelColumn> stringColumns = columns.Where(c => c.IsText()).ToList();

            stringColumns.ForEach(column =>
            {
                string? sValue = null;

                if (totals.TryGetValue(column.Name, out object? value) && value != null)
                {
                    if (column.IsStringType())
                    {
                        sValue = value.ToString();
                    }
                    else if (column.Type == DbType.Boolean)
                    {
                        bool bVal = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                        sValue = bVal ? "Sí" : "No";
                    }

                    if (sValue is not null)
                    {
                        column.FooterSharedTable = sharedTable.AddString(sValue);
                    }
                }
            });
        }
    }
}
