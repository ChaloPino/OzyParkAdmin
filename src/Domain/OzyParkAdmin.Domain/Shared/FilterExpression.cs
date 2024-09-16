using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa el filtrado de un elemento.
/// </summary>
/// <typeparam name="T">El tipo del elemento a ser filtrado.</typeparam>
public abstract class FilterExpression<T> : IFilterExpression<T>
    where T : class
{
    private readonly Expression<Func<T, bool>> _predicate;

    /// <summary>
    /// Crea una instancia de <see cref="FilterExpression{T}"/>.
    /// </summary>
    /// <param name="predicate">Una expressión que define el predicado de filtrado.</param>
    protected FilterExpression(Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        _predicate = predicate;
    }

    /// <inheritdoc/>
    public IQueryable<T> Where(IQueryable<T> query)
    {
        return query.Where(_predicate);
    }
}
