using OzyParkAdmin.Domain.Shared.Expressions;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa el filtrado de un elemento usando un miembro de tipo numérico.
/// </summary>
/// <typeparam name="T">El tipo del elemento.</typeparam>
/// <typeparam name="TNumber">El tipo numérico.</typeparam>
public class NumberFilterExpression<T, TNumber> : FilterExpression<T>
    where T : class
    where TNumber : struct, INumber<TNumber>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="NumberFilterExpression{T, TNumber}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    /// <param name="value">El valor que se usará para filtrar el elemento.</param>
    public NumberFilterExpression(Expression<Func<T, TNumber?>> member, string @operator, IConvertible value)
        : base(CreatePredicate(member, @operator, Convert(value)).Reduce())
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="NumberFilterExpression{T, TNumber}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    /// <param name="value">El valor que se usará para filtrar el elemento.</param>
    public NumberFilterExpression(Expression<Func<T, TNumber>> member, string @operator, IConvertible value)
        : base(CreatePredicate(member, @operator, Convert(value)).Reduce())
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="NumberFilterExpression{T, TNumber}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    public NumberFilterExpression(Expression<Func<T, TNumber?>> member, string @operator)
        : base(CreatePredicate(member, @operator, default).Reduce())
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="NumberFilterExpression{T, TNumber}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    public NumberFilterExpression(Expression<Func<T, TNumber>> member, string @operator)
        : base(CreatePredicate(member, @operator, default).Reduce())
    {
    }

    private static TNumber Convert(IConvertible value) =>
        (TNumber)value.ToType(typeof(TNumber), CultureInfo.InvariantCulture);

    private static FilterOperationExpression<T> CreatePredicate(Expression<Func<T, TNumber?>> member, string @operator, TNumber value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(@operator);
        return @operator.ToLowerInvariant() switch
        {
            "=" => new EqualsExpression<T, TNumber?>(member, value),
            "!=" => new NotEqualsExpression<T, TNumber?>(member, value),
            ">" => new GreaterThanExpression<T, TNumber>(member, value),
            "<" => new LessThanExpression<T, TNumber>(member, value),
            ">=" => new GreaterThanOrEqualsExpression<T, TNumber>(member, value),
            "<=" => new LessThanOrEqualsExpression<T, TNumber>(member, value),
            "is empty" => new IsEmptyExpression<T, TNumber>(member),
            "is not empty" => new IsNotEmptyExpression<T, TNumber>(member),
            _ => throw new UnreachableException(),
        };
    }

    private static FilterOperationExpression<T> CreatePredicate(Expression<Func<T, TNumber>> member, string @operator, TNumber value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(@operator);
        return @operator.ToLowerInvariant() switch
        {
            "=" => new EqualsExpression<T, TNumber>(member, value),
            "!=" => new NotEqualsExpression<T, TNumber>(member, value),
            ">" => new GreaterThanExpression<T, TNumber>(member, value),
            "<" => new LessThanExpression<T, TNumber>(member, value),
            ">=" => new GreaterThanOrEqualsExpression<T, TNumber>(member, value),
            "<=" => new LessThanOrEqualsExpression<T, TNumber>(member, value),
            "is empty" => new IsEmptyExpression<T, TNumber>(member),
            "is not empty" => new IsNotEmptyExpression<T, TNumber>(member),
            _ => throw new UnreachableException(),
        };
    }
}
