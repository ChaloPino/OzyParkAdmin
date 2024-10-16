using Humanizer;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OzyParkAdmin.Domain.Reportes.Listed;
using OzyParkAdmin.Domain.Reportes.Pivoted;
using OzyParkAdmin.Infrastructure.Reportes.Internals;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;
internal static class PivotHelper
{
    private static readonly int[] quarters = new int[12];

    private readonly static MethodInfo FieldMethod = typeof(DataRowExtensions).GetMethods().First(m => m.Name == nameof(DataRowExtensions.Field) && m.GetParameters().Length == 2 && m.GetParameters()[1].ParameterType == typeof(string));
    private readonly static MethodInfo GroupByMethod = typeof(Enumerable).GetMethods().First(m => m.Name == nameof(Enumerable.GroupBy) && m.GetParameters().Length == 2);
    private readonly static MethodInfo SelectMethod = typeof(Enumerable).GetMethods().First(m => m.Name == nameof(Enumerable.Select) && m.GetParameters()[1].ParameterType.GetGenericArguments().Length == 2);
    private readonly static MethodInfo OrderByMethod = typeof(Enumerable).GetMethods().First(m => m.Name == nameof(Enumerable.OrderBy) && m.GetParameters().Length == 2);
    private readonly static MethodInfo OrderByDescendingMethod = typeof(Enumerable).GetMethods().First(m => m.Name == nameof(Enumerable.OrderByDescending) && m.GetParameters().Length == 2);
    private readonly static MethodInfo ToArrayMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToArray))!;

    private readonly static string[] SpecialPropertyNames =
    [
        PivotDatePartNames.QuarterName,
        PivotDatePartNames.ShortMonthName,
        PivotDatePartNames.LongMonthName
    ];

    private readonly static Dictionary<string, Type> SpecialPropertyTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        { PivotDatePartNames.QuarterName, typeof(int) },
        { PivotDatePartNames.ShortMonthName, typeof(string) },
        { PivotDatePartNames.LongMonthName, typeof(string) }
    };

    private readonly static Dictionary<string, MethodInfo> SpecialPropertyMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        { PivotDatePartNames.QuarterName, typeof(PivotHelper).GetMethod(nameof(Quarter), BindingFlags.Public | BindingFlags.Static)! },
        { PivotDatePartNames.ShortMonthName, typeof(PivotHelper).GetMethod(nameof(ShortMonth), BindingFlags.Public | BindingFlags.Static)! },
        { PivotDatePartNames.LongMonthName, typeof(PivotHelper).GetMethod(nameof(LongMonth), BindingFlags.Public | BindingFlags.Static)! }
    };

    public static int Quarter(this DateTime value)
    {
        return quarters[value.Month - 1];
    }

    public static string ShortMonth(this DateTime value)
    {
        string[] months = GetMonthsList(true, true);
        return months[value.Month - 1];
    }

    public static string LongMonth(DateTime value)
    {
        string[] months = GetMonthsList(false, true);
        return months[value.Month - 1];
    }

    public static string[] GetMonthsList(bool isShort, bool isAscending)
    {
        string[] names = isShort
            ? CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames
            : CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

        return isAscending ? names : names.Reverse().ToArray();
    }

    public static object[] GetDimensionList(PivotedMember pivotedMember, DataTable dataTable, bool ordered)
    {
        return !string.IsNullOrEmpty(pivotedMember.CustomSortList)
            ? GetCustomList(pivotedMember, dataTable.Columns[pivotedMember.Column.Name]!.DataType, ordered)
            : GetExternalColumnList(pivotedMember, dataTable, ordered);
    }

    private static object[] GetCustomList(PivotedMember pivotedMember, Type dataType, bool ordered)
    {
        if (!string.IsNullOrEmpty(pivotedMember.Property))
        {
            if (SpecialPropertyNames.Contains(pivotedMember.Property, StringComparer.OrdinalIgnoreCase))
            {
                dataType = SpecialPropertyTypes[pivotedMember.Property];
            }
            else
            {
                PropertyInfo propertyInfo = dataType.GetProperty(pivotedMember.Property.Titleize())!;
                dataType = propertyInfo.PropertyType;
            }
        }

        string[] stringList = pivotedMember.CustomSortList.NotNullSplit(',').Select(s => s.Trim()).ToArray();
        object[] list = stringList.Select(s => Convert.ChangeType(s, dataType, CultureInfo.InvariantCulture)).ToArray();

        if (!ordered && !pivotedMember.SortDirection.HasValue)
        {
            return list;
        }

        if (pivotedMember.SortDirection == PivotSortDirection.Ascending)
        {
            return [.. list.Order()];
        }

        return [.. list.OrderByDescending(s => s)];
    }

    private static object[] GetExternalColumnList(PivotedMember pivotedMember, DataTable dataTable, bool ordered)
    {
        Expression<Func<EnumerableRowCollection<DataRow>, object[]>> lamdaExpression = PrepareExpression(pivotedMember, dataTable, ordered);
        return lamdaExpression.Compile()(dataTable.AsEnumerable());
    }

    private static Expression<Func<EnumerableRowCollection<DataRow>, object[]>> PrepareExpression(PivotedMember pivotedMember, DataTable dataTable, bool ordered)
    {
        ParameterExpression dataRowExpression = Expression.Parameter(typeof(DataRow), "dataRow");
        Expression valueExpression = GetMemberExpression(dataRowExpression, pivotedMember, pivotedMember.Column, dataTable, out Type valueType);

        Expression orderValueExpression = ordered && pivotedMember.SortColumn != null
            ? GetMemberExpression(dataRowExpression, pivotedMember, pivotedMember.SortColumn, dataTable, out Type orderValueType)
            : GetMemberExpression(dataRowExpression, pivotedMember, pivotedMember.Column, dataTable, out orderValueType);

        Type orderResultType = typeof(OrderResult<,>).MakeGenericType(valueType, orderValueType);
        var ctor = orderResultType.GetConstructor([valueType, orderValueType])!;

        NewExpression newOrderResultExpression = Expression.New(ctor, valueExpression, orderValueExpression);

        //MemberBinding[] bindings =
        //[
        //    Expression.Bind(orderResultType.GetProperty(nameof(OrderResult<object, object>.Value))!, valueExpression),
        //    Expression.Bind(orderResultType.GetProperty(nameof(OrderResult<object, object>.OrderedValue))!, orderValueExpression)
        //];

        //MemberInitExpression orderResultExpression = Expression.MemberInit(newOrderResultExpression, bindings);

        LambdaExpression orderResultLambda = Expression.Lambda(newOrderResultExpression, dataRowExpression);

        ParameterExpression rowsExpression = Expression.Parameter(typeof(EnumerableRowCollection<DataRow>), "rows");

        MethodCallExpression groupByExpression = Expression.Call(GroupByMethod.MakeGenericMethod(typeof(DataRow), orderResultType), rowsExpression, orderResultLambda);

        MethodCallExpression orderExpression;

        if (ordered && pivotedMember.SortColumn != null)
        {
            ParameterExpression groupExpression = Expression.Parameter(typeof(IGrouping<,>).MakeGenericType(orderResultType, typeof(DataRow)), "group");

            MemberExpression keyExpression = Expression.Property(groupExpression, nameof(IGrouping<object, object>.Key));

            MemberExpression keyOrderExpression = Expression.PropertyOrField(keyExpression, nameof(OrderResult<object, object>.OrderedValue));

            LambdaExpression keySelectorLambda = Expression.Lambda(keyOrderExpression, groupExpression);

            MethodInfo orderByMethod = pivotedMember.IsAscendingOrder()
                ? OrderByMethod
                : OrderByDescendingMethod;

            orderExpression = Expression.Call(orderByMethod.MakeGenericMethod(groupExpression.Type, orderValueType), groupByExpression, keySelectorLambda);
        }
        else
        {
            orderExpression = groupByExpression;
        }

        ParameterExpression groupSelectorExpression = Expression.Parameter(typeof(IGrouping<,>).MakeGenericType(orderResultType, typeof(DataRow)), "group");

        MemberExpression keySelectorMemberExpression = Expression.Property(groupSelectorExpression, nameof(IGrouping<object, object>.Key));

        Expression keySelectorValueExpression = Expression.Convert(Expression.PropertyOrField(keySelectorMemberExpression, nameof(OrderResult<object, object>.Value)), typeof(object));

        LambdaExpression selectorLambda = Expression.Lambda(keySelectorValueExpression, groupSelectorExpression);

        MethodCallExpression selectExpression = Expression.Call(SelectMethod.MakeGenericMethod(groupSelectorExpression.Type, typeof(object)), orderExpression, selectorLambda);

        MethodCallExpression toArrayExpression = Expression.Call(ToArrayMethod.MakeGenericMethod(typeof(object)), selectExpression);

        return Expression.Lambda<Func<EnumerableRowCollection<DataRow>, object[]>>(toArrayExpression, rowsExpression);
    }

    private static Expression GetMemberExpression(ParameterExpression dataRowExpression, PivotedMember pivotedMember, Column column, DataTable dataTable, out Type dataType)
    {
        DataColumn dataColumn = dataTable.Columns[column.Name]!;

        dataType = dataColumn.DataType;

        Expression memberExpression = Expression.Call(FieldMethod.MakeGenericMethod(dataType), dataRowExpression, Expression.Constant(column.Name, typeof(string)));

        if (!string.IsNullOrEmpty(pivotedMember.Property))
        {
            if (SpecialPropertyNames.Contains(pivotedMember.Property, StringComparer.OrdinalIgnoreCase))
            {
                dataType = SpecialPropertyTypes[pivotedMember.Property];
                MethodInfo methodInfo = SpecialPropertyMethods[pivotedMember.Property];
                memberExpression = Expression.Call(methodInfo, memberExpression);
            }
            else
            {
                PropertyInfo propertyInfo = dataType.GetProperty(pivotedMember.Property.Titleize())!;
                dataType = propertyInfo.PropertyType;
                memberExpression = Expression.Property(memberExpression, propertyInfo);
            }
        }

        return memberExpression;
    }

    private sealed record OrderResult<T, TOrder>(T Value, TOrder OrderedValue);
}
