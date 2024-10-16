using NReco.PivotData;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Pivoted;
using System.Data;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;
internal static class PivotFactory
{
    private readonly static Dictionary<AggregationType, Func<string, IAggregatorFactory>> RegisteredAggregatorFactories = new()
    {
        { AggregationType.Sum, (field) => new SumAggregatorFactory(field) },
        { AggregationType.Count, (_) => new CountAggregatorFactory() },
        { AggregationType.Average, (field) => new AverageAggregatorFactory(field) },
        { AggregationType.Min, (field) => new MinAggregatorFactory(field) },
        { AggregationType.Max, (field) => new MaxAggregatorFactory(field) }
    };

    private readonly static Dictionary<string, Func<string, Func<Func<object, string?, object?>, Func<object, string?, object?>>>> RegisteredParts = new(StringComparer.OrdinalIgnoreCase)
    {
        { PivotDatePartNames.YearName, (field) => new DatePartValue(field).YearHandler },
        { PivotDatePartNames.QuarterName, (field) => new DatePartValue(field).QuarterHandler },
        { PivotDatePartNames.MonthName, (field) => new DatePartValue(field).MonthNumberHandler },
        { PivotDatePartNames.DayName, (field) => new DatePartValue(field).DayHandler },
        { PivotDatePartNames.DateName, (field) => new DatePartValue(field).DateOnlyHandler },
        { PivotDatePartNames.ShortMonthName, (field) => new DatePartValue(field).MonthShortNameHandler },
        { PivotDatePartNames.LongMonthName, (field) => new DatePartValue(field).MonthLongNameHandler }
    };

    public static IPivotTable CreatePivotTable(IList<PivotedMember> pivotedMembers, DataTable dataTable)
    {
        return CreatePivotTable(pivotedMembers, dataTable, dataTable.AsEnumerable());
    }

    public static IPivotTable CreatePivotTable(IList<PivotedMember> pivotedMembers, DataTable dataTable, IEnumerable<DataRow> dataRows)
    {
        DataTableSource dataTableSource = new(dataRows);

        DerivedValueSource derivedValueSource = new(dataTableSource);

        foreach (var pivotMember in pivotedMembers)
        {
            if (pivotMember.PivotType != PivotType.Value && !string.IsNullOrEmpty(pivotMember.Property) && pivotMember.Column.IsDateType() && !string.IsNullOrEmpty(pivotMember.PropertyDisplay))
            {
                _ = derivedValueSource.Register(pivotMember.PropertyDisplay, RegisteredParts[pivotMember.Property](pivotMember.Column.Name));
                continue;
            }

            if (!string.IsNullOrEmpty(pivotMember.Header))
            {
                _ = derivedValueSource.Register(pivotMember.Header, new HeaderPartValue(pivotMember.Column.Name).HeaderName);
            }
        }

        string[] dimensions = pivotedMembers
                               .Where(pm => pm.PivotType != PivotType.Value)
                               .Select(pm => pm.GetFullName())
                               .Distinct()
                               .ToArray();

        var aggregatorFactory = new CompositeAggregatorFactory(
            pivotedMembers
                  .Where(pm => pm.PivotType == PivotType.Value && pm.AggregationType.HasValue)
                  .Select(pm => RegisteredAggregatorFactories[pm.AggregationType!.Value](pm.Column.Name))
                  .ToArray());

        PivotData pivotData = new(dimensions, aggregatorFactory);
        pivotData.ProcessData(derivedValueSource);

        string[] rows = pivotedMembers
                              .Where(pm => pm.PivotType == PivotType.Row)
                              .Select(pm => pm.GetFullName())
                              .Distinct()
                              .ToArray();

        string[] columns = pivotedMembers
                              .Where(pm => pm.PivotType == PivotType.Column)
                              .Select(pm => pm.GetFullName())
                              .ToArray();

        var rowComparers = pivotedMembers
                            .Where(pm => pm.PivotType == PivotType.Row)
                            .Select(CreateSortKeyComparer(dataTable)).ToArray();

        var columnComparers = pivotedMembers
                               .Where(pm => pm.PivotType == PivotType.Column)
                               .Select(CreateSortKeyComparer(dataTable)).ToArray();

        return new PivotTable(rows, columns, pivotData, new CustomSortKeyComparer(rowComparers), new CustomSortKeyComparer(columnComparers));
    }

    private static Func<PivotedMember, IComparer<object>> CreateSortKeyComparer(DataTable dataTable)
    {
        return pm =>
        {
            if (pm.IsSpecialDateOrderable())
            {
                return new SortAsComparer(PivotHelper.GetMonthsList(string.Equals(pm.Property, PivotDatePartNames.ShortMonthName, StringComparison.OrdinalIgnoreCase), pm.IsAscendingOrder()));
            }
            else
            {
                if (pm.IsAscendingOrder())
                {
                    return pm.IsOrderable()
                    ? new SortAsComparer(PivotHelper.GetDimensionList(pm, dataTable, true))
                    : NaturalSortKeyComparer.Instance;
                }
                else
                {
                    return pm.IsOrderable()
                    ? new SortAsComparer(PivotHelper.GetDimensionList(pm, dataTable, true))
                    : NaturalSortKeyComparer.ReverseInstance;
                }
            }
        };
    }
}
