using OzyParkAdmin.Domain.Shared.Expressions;
using System.Diagnostics;
using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa el filtrado de un elemento usando un miembro de tipo <see cref="bool"/>.
/// </summary>
/// <typeparam name="T">El tipo del elemento.</typeparam>
public class BooleanFilterExpression<T> : FilterExpression<T>
    where T : class
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="BooleanFilterExpression{T}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    /// <param name="value">El valor que se usará para filtrar el elemento.</param>
    public BooleanFilterExpression(Expression<Func<T, bool>> member, string @operator, bool value)
        : base(CreatePredicate(member, @operator, value))
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="BooleanFilterExpression{T}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    /// <param name="value">El valor que se usará para filtrar el elemento.</param>
    public BooleanFilterExpression(Expression<Func<T, bool?>> member, string @operator, bool? value)
        : base(CreatePredicate(member, @operator, value))
    {
    }

    private static FilterOperationExpression<T> CreatePredicate(Expression<Func<T, bool>> member, string @operator, bool value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(@operator);
        return @operator.ToLowerInvariant() switch
        {
            "is" => new EqualsExpression<T, bool>(member, value),
            _ => throw new UnreachableException(),
        };
    }

    private static FilterOperationExpression<T> CreatePredicate(Expression<Func<T, bool?>> member, string @operator, bool? value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(@operator);
        return @operator.ToLowerInvariant() switch
        {
            "is" => new EqualsExpression<T, bool?>(member, value),
            _ => throw new UnreachableException(),
        };
    }
}
