using OzyParkAdmin.Domain.Shared;
using System.Data;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals;
internal static class DataTableExtensions
{
    public static List<DataRow> Sort(this DataTable dataTable, bool canSort, SortExpressionCollection<DataRow> sortExpressionCollection)
    {
        List<DataRow> rows = dataTable.AsEnumerable().ToList();
        return rows.Sort(canSort, sortExpressionCollection);
    }

    public static List<DataRow> Sort(this List<DataRow> rows, bool canSort, SortExpressionCollection<DataRow> sortExpressionCollection)
    {
        if (canSort)
        {
            rows = sortExpressionCollection.Sort(rows).ToList();
        }

        return rows;
    }
}
