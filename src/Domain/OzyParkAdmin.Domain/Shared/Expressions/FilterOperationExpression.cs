using OzyParkAdmin.Domain.Shared.Utils;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace OzyParkAdmin.Domain.Shared.Expressions;

/// <summary>
/// La expression de filtrado definida por una expressión lambda.
/// </summary>
/// <typeparam name="T">El tipo del elemento.</typeparam>
public abstract class FilterOperationExpression<T>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="FilterExpressionCollection{T}"/>.
    /// </summary>
    /// <param name="memberExpression">La expresión lambda que contiene el miembro.</param>
    protected FilterOperationExpression(LambdaExpression memberExpression)
    {
        ArgumentNullException.ThrowIfNull(memberExpression);
        Generate(memberExpression);
        var property = PropertyPath.Visit(memberExpression);

        MemberName = property.IsBodyMemberExpression
            ? property.GetPath()
            : Guid.NewGuid().ToString();
    }

    /// <summary>
    /// El parámetro.
    /// </summary>
    public ParameterExpression Parameter { get; private set; } = default!;

    /// <summary>
    /// El miembro.
    /// </summary>
    public MemberExpression Member { get; private set; } = default!;

    /// <summary>
    /// El nombre completo del miembro.
    /// </summary>
    public string MemberName { get; }

    internal Expression<Func<T, bool>> Reduce()
    {
        Expression body = GenerateBody();
        return Expression.Lambda<Func<T, bool>>(body, Parameter);
    }

    /// <summary>
    /// Genera el cuerpo de la expresión lambda.
    /// </summary>
    /// <returns></returns>
    protected abstract Expression GenerateBody();

    internal void Generate(LambdaExpression memberExpression)
    {
        Parameter = memberExpression.Parameters[0];
        Member = (MemberExpression)memberExpression.Body;
    }
}