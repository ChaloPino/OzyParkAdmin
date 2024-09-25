using OzyParkAdmin.Domain.Shared.Expressions;
using System.Diagnostics;
using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa el filtrado de un elemento usando un miembro de tipo enumerador.
/// </summary>
/// <typeparam name="T">El tipo del elemento.</typeparam>
/// <typeparam name="TEnum">El tipo del enumerador.</typeparam>
public class EnumFilterExpression<T, TEnum> : FilterExpression<T>
    where T : class
    where TEnum : struct, Enum
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="EnumFilterExpression{T, TEnum}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    /// <param name="value">El valor que se usará para filtrar el elemento.</param>
    public EnumFilterExpression(Expression<Func<T, TEnum>> member, string @operator, TEnum value)
        : base(CreatePredicate(member, @operator, value))
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="EnumFilterExpression{T, TEnum}"/>.
    /// </summary>
    /// <param name="member">Una expresión que representa el miembro del elemento.</param>
    /// <param name="operator">El operador que se usará para el filtrado.</param>
    /// <param name="value">El valor que se usará para filtrar el elemento.</param>
    public EnumFilterExpression(Expression<Func<T, TEnum?>> member, string @operator, TEnum? value)
        : base(CreatePredicate(member, @operator, value))
    {
    }

    private static FilterOperationExpression<T> CreatePredicate(Expression<Func<T, TEnum?>> member, string @operator, TEnum? value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(@operator);
        return @operator.ToLowerInvariant() switch
        {
            "is" => new EqualsExpression<T, TEnum?>(member, value),
            "is not" => new NotEqualsExpression<T, TEnum?>(member, value),
            _ => throw new UnreachableException(),
        };
    }

    private static FilterOperationExpression<T> CreatePredicate(Expression<Func<T, TEnum>> member, string @operator, TEnum value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(@operator);
        return @operator.ToLowerInvariant() switch
        {
            "is" => new EqualsExpression<T, TEnum>(member, value),
            "is not" => new NotEqualsExpression<T, TEnum>(member, value),
            _ => throw new UnreachableException(),
        };
    }
}
