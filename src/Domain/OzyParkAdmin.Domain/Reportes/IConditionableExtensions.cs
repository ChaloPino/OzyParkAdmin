using System.Data;
using System.Globalization;
using System.Reflection;

namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// Contiene métodos de extensión para <see cref="IConditionable"/>.
/// </summary>
public static class IConditionableExtensions
{
    private static readonly MethodInfo CompareMethod = typeof(IConditionableExtensions).GetMethod(nameof(Compare), BindingFlags.Public | BindingFlags.Static)!;

    private readonly static Dictionary<DbType, Func<object?, object?>> TypeConverters = new()
    {
        { DbType.AnsiString, (value) => value?.ToString()! },
        { DbType.AnsiStringFixedLength, (value) => value?.ToString()! },
        { DbType.Binary, (value) => value?.ToString()!.ToCharArray().Select(c => Convert.ToByte(c, CultureInfo.InvariantCulture)).ToArray() },
        { DbType.Boolean, (value) => Convert.ToBoolean(value, CultureInfo.InvariantCulture) },
        { DbType.Byte, (value) => Convert.ToByte(value, CultureInfo.InvariantCulture) },
        { DbType.Currency, (value) => Convert.ToDecimal(value, CultureInfo.InvariantCulture) },
        { DbType.Date, (value) => Convert.ToDateTime(value, CultureInfo.InvariantCulture) },
        { DbType.DateTime, (value) => Convert.ToDateTime(value, CultureInfo.InvariantCulture) },
        { DbType.DateTime2, (value) => Convert.ToDateTime(value, CultureInfo.InvariantCulture) },
        { DbType.DateTimeOffset, (value) => value is not null ? DateTimeOffset.Parse(value.ToString()!, CultureInfo.InvariantCulture) : null },
        { DbType.Decimal, (value) => Convert.ToDecimal(value, CultureInfo.InvariantCulture) },
        { DbType.Double, (value) => Convert.ToDouble(value, CultureInfo.InvariantCulture) },
        { DbType.Guid, (value) => value is not null ? new Guid(value.ToString()!) : null },
        { DbType.Int16, (value) => Convert.ToInt16(value, CultureInfo.InvariantCulture) },
        { DbType.Int32, (value) => Convert.ToInt32(value, CultureInfo.InvariantCulture) },
        { DbType.Int64, (value) => Convert.ToInt64(value, CultureInfo.InvariantCulture) },
        { DbType.Object, (value) => value },
        { DbType.SByte, (value) => Convert.ToSByte(value, CultureInfo.InvariantCulture) },
        { DbType.Single, (value) => Convert.ToSingle(value, CultureInfo.InvariantCulture) },
        { DbType.String, (value) => value },
        { DbType.StringFixedLength, (value) => value },
        { DbType.Time, (value) => value is not null ? TimeSpan.Parse(value.ToString()!, CultureInfo.InvariantCulture) : null },
        { DbType.UInt16, (value) => Convert.ToUInt16(value, CultureInfo.InvariantCulture) },
        { DbType.UInt32, (value) => Convert.ToUInt32(value, CultureInfo.InvariantCulture) },
        { DbType.UInt64, (value) => Convert.ToUInt64(value, CultureInfo.InvariantCulture) },
        { DbType.VarNumeric, (value) => Convert.ToDecimal(value, CultureInfo.InvariantCulture) },
        { DbType.Xml, (value) => value }
    };

    private readonly static Dictionary<DbType, Func<string?, object?>> StringTypeConverters = new()
    {
        { DbType.AnsiString, (value) => value },
        { DbType.AnsiStringFixedLength, (value) => value },
        { DbType.Binary, (value) => value?.ToCharArray().Select(c => Convert.ToByte(c, CultureInfo.InvariantCulture)).ToArray() },
        { DbType.Boolean, (value) => Convert.ToBoolean(value, CultureInfo.InvariantCulture) },
        { DbType.Byte, (value) => Convert.ToByte(value, CultureInfo.InvariantCulture) },
        { DbType.Currency, (value) => Convert.ToDecimal(value, CultureInfo.InvariantCulture) },
        { DbType.Date, (value) => Convert.ToDateTime(value, CultureInfo.InvariantCulture) },
        { DbType.DateTime, (value) => Convert.ToDateTime(value, CultureInfo.InvariantCulture) },
        { DbType.DateTime2, (value) => Convert.ToDateTime(value, CultureInfo.InvariantCulture) },
        { DbType.DateTimeOffset, (value) => value is null ? null : DateTimeOffset.Parse(value, CultureInfo.InvariantCulture) },
        { DbType.Decimal, (value) => Convert.ToDecimal(value, CultureInfo.InvariantCulture) },
        { DbType.Double, (value) => Convert.ToDouble(value, CultureInfo.InvariantCulture) },
        { DbType.Guid, (value) => value is null ? null : new Guid(value) },
        { DbType.Int16, (value) => Convert.ToInt16(value, CultureInfo.InvariantCulture) },
        { DbType.Int32, (value) => Convert.ToInt32(value, CultureInfo.InvariantCulture) },
        { DbType.Int64, (value) => Convert.ToInt64(value, CultureInfo.InvariantCulture) },
        { DbType.Object, (value) => value },
        { DbType.SByte, (value) => value is null ? null : Convert.ToSByte(value, CultureInfo.InvariantCulture) },
        { DbType.Single, (value) => Convert.ToSingle(value, CultureInfo.InvariantCulture) },
        { DbType.String, (value) => value },
        { DbType.StringFixedLength, (value) => value },
        { DbType.Time, (value) => value is null ? null : TimeSpan.Parse(value, CultureInfo.InvariantCulture) },
        { DbType.UInt16, (value) => Convert.ToUInt16(value, CultureInfo.InvariantCulture) },
        { DbType.UInt32, (value) => Convert.ToUInt32(value, CultureInfo.InvariantCulture) },
        { DbType.UInt64, (value) => Convert.ToUInt64(value, CultureInfo.InvariantCulture) },
        { DbType.VarNumeric, (value) => Convert.ToDecimal(value, CultureInfo.InvariantCulture) },
        { DbType.Xml, (value) => value }
    };

    private readonly static Dictionary<OperatorStyleType, Func<EvaluatorContext, bool>> OperatorEvaluators = new()
    {
        { OperatorStyleType.Equal, EqualEvaluator },
        { OperatorStyleType.NotEqual, NotEqualEvaluator },
        { OperatorStyleType.GreaterThan, GreaterThanEvaluator },
        { OperatorStyleType.LessThan, LessThanEvaluator },
        { OperatorStyleType.GreaterThanOrEqual, GreaterThanOrEqualEvaluator },
        { OperatorStyleType.LessThanOrEqual, LessThanOrEqualEvaluator },
        { OperatorStyleType.Between, BetweenEvaluator }
    };

    private readonly static Dictionary<DbType, Type> DbTypeMappers = new()
    {
        [DbType.AnsiString] = typeof(string),
        [DbType.AnsiStringFixedLength] = typeof(string),
        [DbType.Binary] = typeof(byte[]),
        [DbType.Boolean] = typeof(bool),
        [DbType.Byte] = typeof(byte),
        [DbType.Currency] = typeof(decimal),
        [DbType.Date] = typeof(DateTime),
        [DbType.DateTime] = typeof(DateTime),
        [DbType.DateTime2] = typeof(DateTime),
        [DbType.DateTimeOffset] = typeof(DateTimeOffset),
        [DbType.Decimal] = typeof(decimal),
        [DbType.Double] = typeof(double),
        [DbType.Guid] = typeof(Guid),
        [DbType.Int16] = typeof(short),
        [DbType.Int32] = typeof(int),
        [DbType.Int64] = typeof(long),
        [DbType.Object] = typeof(object),
        [DbType.SByte] = typeof(sbyte),
        [DbType.String] = typeof(string),
        [DbType.StringFixedLength] = typeof(string),
        [DbType.Time] = typeof(TimeSpan),
        [DbType.UInt16] = typeof(ushort),
        [DbType.UInt32] = typeof(uint),
        [DbType.UInt64] = typeof(ulong),
        [DbType.VarNumeric] = typeof(decimal),
        [DbType.Xml] = typeof(object),
    };

    /// <summary>
    /// Realiza la comparación de dos valores.
    /// </summary>
    /// <typeparam name="T">El tipo de los valores.</typeparam>
    /// <param name="first">El primer valor.</param>
    /// <param name="second">El segundo valor.</param>
    /// <returns>
    /// <para>
    /// 1, si <paramref name="first"/> es mayor a <paramref name="second"/>.
    /// </para>
    /// <para>
    /// -1, si <paramref name="second"/> es mayor a <paramref name="first"/>.
    /// </para>
    /// <para>
    /// 0, si <paramref name="first"/> y <paramref name="second"/> son iguales.</para>
    /// </returns>
    public static int Compare<T>(T first, T second)
    {
        return Comparer<T>.Default.Compare(first, second);
    }

    /// <summary>
    /// Evalua la condición de éxito.
    /// </summary>
    /// <param name="conditionable">El <see cref="IConditionable"/> a evaluar.</param>
    /// <param name="type">El tipo de dato del valor a evaluar.</param>
    /// <param name="variable">El valor a evaluar.</param>
    /// <returns><c>true</c> si fue evaluado con éxito; en caso contrario, <c>false</c>.</returns>
    public static bool EvaluateSuccessCondition(this IConditionable conditionable, DbType type, object? variable) =>
        EvaluateCondition(type, variable, conditionable.SuccessOperator!.Value, conditionable.SuccessConditionalValue, conditionable.SuccessAlternateConditionalValue);

    /// <summary>
    /// Evalua la condición de advertencia.
    /// </summary>
    /// <param name="conditionable">El <see cref="IConditionable"/> a evaluar.</param>
    /// <param name="type">El tipo de dato del valor a evaluar.</param>
    /// <param name="variable">El valor a evaluar.</param>
    /// <returns><c>true</c> si fue evaluado con éxito; en caso contrario, <c>false</c>.</returns>
    public static bool EvaluateWarningCondition(this IConditionable conditionable, DbType type, object? variable) =>
        EvaluateCondition(type, variable, conditionable.WarningOperator!.Value, conditionable.WarningConditionalValue, conditionable.WarningAlternateConditionalValue);

    private static bool EvaluateCondition(DbType type, object? variable, OperatorStyleType @operator, string? value, string? alternateValue)
    {
        object? oVariable = TypeConverters[type](variable);
        object? oValue = StringTypeConverters[type](value);
        object? oAlternateValue = StringTypeConverters[type](alternateValue);
        EvaluatorContext context = new(type, oVariable, oValue, oAlternateValue);
        return OperatorEvaluators[@operator](context);
    }

    private static bool EqualEvaluator(EvaluatorContext context)
    {
        return Equals(context.Variable, context.Value);
    }

    private static bool NotEqualEvaluator(EvaluatorContext context)
    {
        return !Equals(context.Variable, context.Value);
    }

    private static bool GreaterThanEvaluator(EvaluatorContext context)
    {
        object? variable = context.Variable;
        object? value = context.Value;
        Type type = DbTypeMappers[context.Type];
        int result = (int)CompareMethod.MakeGenericMethod(type).Invoke(null, [variable, value])!;
        return result > 0;
    }

    private static bool LessThanEvaluator(EvaluatorContext context)
    {
        object? variable = context.Variable;
        object? value = context.Value;
        Type type = DbTypeMappers[context.Type];
        int result = (int)CompareMethod.MakeGenericMethod(type).Invoke(null, [variable, value])!;
        return result < 0;
    }

    private static bool GreaterThanOrEqualEvaluator(EvaluatorContext context)
    {
        object? variable = context.Variable;
        object? value = context.Value;
        Type type = DbTypeMappers[context.Type];
        int result = (int)CompareMethod.MakeGenericMethod(type).Invoke(null, [variable, value])!;
        return result >= 0;
    }

    private static bool LessThanOrEqualEvaluator(EvaluatorContext context)
    {
        object? variable = context.Variable;
        object? value = context.Value;
        Type type = DbTypeMappers[context.Type];
        int result = (int)CompareMethod.MakeGenericMethod(type).Invoke(null, [variable, value])!;
        return result <= 0;
    }

    private static bool BetweenEvaluator(EvaluatorContext context)
    {
        object? variable = context.Variable;
        object? value = context.Value;
        object? alternateValue = context.AlternateValue!;
        Type type = DbTypeMappers[context.Type];
        MethodInfo compareMethod = CompareMethod.MakeGenericMethod(type);
        int firstResult = (int)compareMethod.Invoke(null, [variable, value])!;
        int secondResult = (int)compareMethod.Invoke(null, [variable, alternateValue])!;

        return firstResult >= 0 && secondResult <= 0;
    }

    private sealed record EvaluatorContext(DbType Type, object? Variable, object? Value, object? AlternateValue);
}
