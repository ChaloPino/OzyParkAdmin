using OzyParkAdmin.Domain.Shared.Expressions;
using System.Diagnostics;
using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa el filtrado de un elemento usando un miembro de tipo <see cref="Guid"/>.
/// </summary>
/// <typeparam name="T">El tipo del elemento.</typeparam>
public class GuidFilterExpression<T> : FilterExpression<T>
    where T : class
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="GuidFilterExpression{T}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    /// <param name="value">El valor que se usará para filtrar el elemento.</param>
    public GuidFilterExpression(Expression<Func<T, Guid>> member, string @operator, Guid value) 
        : base(CreatePredicate(member, @operator, value))
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="GuidFilterExpression{T}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    /// <param name="value">El valor que se usará para filtrar el elemento.</param>
    public GuidFilterExpression(Expression<Func<T, Guid?>> member, string @operator, Guid? value)
        : base(CreatePredicate(member, @operator, value))
    {
    }

    private static FilterOperationExpression<T> CreatePredicate(Expression<Func<T, Guid?>> member, string @operator, Guid? value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(@operator);
        return @operator.ToLowerInvariant() switch
        {
            "equals" => new EqualsExpression<T, Guid?>(member, value),
            "not equals" => new NotEqualsExpression<T, Guid?>(member, value),
            _ => throw new UnreachableException(),
        };
    }

    private static FilterOperationExpression<T> CreatePredicate(Expression<Func<T, Guid>> member, string @operator, Guid value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(@operator);
        return @operator.ToLowerInvariant() switch
        {
            "equals" => new EqualsExpression<T, Guid>(member, value),
            "not equals" => new NotEqualsExpression<T, Guid>(member, value),
            _ => throw new UnreachableException(),
        };
    }
}
