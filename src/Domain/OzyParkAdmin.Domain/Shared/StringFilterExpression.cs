using OzyParkAdmin.Domain.Shared.Expressions;
using System.Diagnostics;
using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa el filtrado de un elemento usando un miembro de tipo <see cref="string"/>.
/// </summary>
/// <typeparam name="T">El tipo del elemento.</typeparam>
public class StringFilterExpression<T> : FilterExpression<T>
    where T : class
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="StringFilterExpression{T}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    /// <param name="value">El valor que se usará para filtrar el elemento.</param>
    public StringFilterExpression(Expression<Func<T, string?>> member, string @operator, string? value = null)
        : base(CreatePredicate(member, @operator, value).Reduce())
    {
    }

    private static FilterOperationExpression<T> CreatePredicate(Expression<Func<T, string?>> member, string @operator, string? value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(@operator);
        return @operator.ToLowerInvariant() switch
        {
            "contains" => new ContainsExpression<T>(member, value),
            "notcontains" => new NotContainsExpression<T>(member, value),
            "equals" => new EqualsExpression<T, string>(member, value),
            "notequals" => new NotEqualsExpression<T, string>(member, value),
            "starts with" => new StartsWithExpression<T>(member, value),
            "ends with" => new EndsWithExpression<T>(member, value),
            "is empty" => new IsNullOrEmptyExpression<T>(member),
            "is not empty" => new IsNotNullOrEmptyExpression<T>(member),
            _ => throw new UnreachableException(),
        };
    }
}
