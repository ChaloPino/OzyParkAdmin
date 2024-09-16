using OzyParkAdmin.Domain.Shared.Expressions;
using System.Diagnostics;
using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa el filtrado de un elemento usando un miembro de tipo fecha y hora.
/// </summary>
/// <typeparam name="T">El tipo del elemento.</typeparam>
/// <typeparam name="TDateAndTime">El tipo fecha y hora.</typeparam>
public class DateAndTimeFilterExpression<T, TDateAndTime> : FilterExpression<T>
    where T : class
    where TDateAndTime : struct
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="DateAndTimeFilterExpression{T, TNumber}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    /// <param name="value">El valor que se usará para filtrar el elemento.</param>
    public DateAndTimeFilterExpression(Expression<Func<T, TDateAndTime?>> member, string @operator, TDateAndTime value)
        : base(CreatePredicate(member, @operator, value).Reduce())
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="DateAndTimeFilterExpression{T, TNumber}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    /// <param name="value">El valor que se usará para filtrar el elemento.</param>
    public DateAndTimeFilterExpression(Expression<Func<T, TDateAndTime>> member, string @operator, TDateAndTime value)
        : base(CreatePredicate(member, @operator, value).Reduce())
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="DateAndTimeFilterExpression{T, TNumber}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    public DateAndTimeFilterExpression(Expression<Func<T, TDateAndTime?>> member, string @operator)
        : base(CreatePredicate(member, @operator, default).Reduce())
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="DateAndTimeFilterExpression{T, TNumber}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    public DateAndTimeFilterExpression(Expression<Func<T, TDateAndTime>> member, string @operator)
        : base(CreatePredicate(member, @operator, default).Reduce())
    {
    }

    private static FilterOperationExpression<T> CreatePredicate(Expression<Func<T, TDateAndTime?>> member, string @operator, TDateAndTime value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(@operator);
        return @operator.ToLowerInvariant() switch
        {
            "is" => new EqualsExpression<T, TDateAndTime?>(member, value),
            "is not" => new NotEqualsExpression<T, TDateAndTime?>(member, value),
            "is after" => new GreaterThanExpression<T, TDateAndTime>(member, value),
            "is before" => new LessThanExpression<T, TDateAndTime>(member, value),
            "is on or after" => new GreaterThanOrEqualsExpression<T, TDateAndTime>(member, value),
            "is on or before" => new LessThanOrEqualsExpression<T, TDateAndTime>(member, value),
            "is empty" => new IsEmptyExpression<T, TDateAndTime>(member),
            "is not empty" => new IsNotEmptyExpression<T, TDateAndTime>(member),
            _ => throw new UnreachableException(),
        };
    }

    private static FilterOperationExpression<T> CreatePredicate(Expression<Func<T, TDateAndTime>> member, string @operator, TDateAndTime value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(@operator);
        return @operator.ToLowerInvariant() switch
        {
            "is" => new EqualsExpression<T, TDateAndTime>(member, value),
            "is not" => new NotEqualsExpression<T, TDateAndTime>(member, value),
            "is after" => new GreaterThanExpression<T, TDateAndTime>(member, value),
            "is before" => new LessThanExpression<T, TDateAndTime>(member, value),
            "is on or after" => new GreaterThanOrEqualsExpression<T, TDateAndTime>(member, value),
            "is on or before" => new LessThanOrEqualsExpression<T, TDateAndTime>(member, value),
            "is empty" => new IsEmptyExpression<T, TDateAndTime>(member),
            "is not empty" => new IsNotEmptyExpression<T, TDateAndTime>(member),
            _ => throw new UnreachableException(),
        };
    }
}
