using OzyParkAdmin.Domain.Shared.Expressions;
using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa el filtrado de un elemento.
/// </summary>
/// <typeparam name="T">El tipo del elemento a ser filtrado.</typeparam>
public abstract class FilterExpression<T> : IFilterExpression<T>
    where T : class
{
    private readonly FilterOperationExpression<T> _operatorExpression;
    private Expression<Func<T, bool>> _predicate;

    /// <summary>
    /// Crea una instancia de <see cref="FilterExpression{T}"/>.
    /// </summary>
    /// <param name="operatorExpression">El <see cref="FilterOperationExpression{T}"/>.</param>
    
    protected FilterExpression(FilterOperationExpression<T> operatorExpression)
    {
        ArgumentNullException.ThrowIfNull(operatorExpression);
        _operatorExpression = operatorExpression;
        _predicate = operatorExpression.Reduce();
        MemberName = operatorExpression.MemberName;
    }

    /// <inheritdoc/>
    public string MemberName { get; }

    /// <inheritdoc/>
    public void Replace(LambdaExpression replacement)
    {
        _operatorExpression.Generate(replacement);
        _predicate = _operatorExpression.Reduce();
    }

    /// <inheritdoc/>
    public IQueryable<T> Where(IQueryable<T> query)
    {
        return query.Where(_predicate);
    }
}
