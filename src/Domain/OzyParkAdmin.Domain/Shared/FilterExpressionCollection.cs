namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa una lista de <see cref="IFilterExpression{T}"/>.
/// </summary>
/// <typeparam name="T">El tipo del elemento a filtrar.</typeparam>
public class FilterExpressionCollection<T>
    where T : class
{
    private readonly List<IFilterExpression<T>> _expressions = [];

    /// <summary>
    /// Crea una nueva instancia de <see cref="FilterExpressionCollection{T}"/>.
    /// </summary>
    public FilterExpressionCollection()
        : this([])
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="FilterExpressionCollection{T}"/> con una lista de <see cref="IFilterExpression{T}"/> para inicializar.
    /// </summary>
    /// <param name="expression">La lista de <see cref="IFilterExpression{T}"/> para inicializar la nueva instancia.</param>
    public FilterExpressionCollection(IEnumerable<IFilterExpression<T>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        _expressions.AddRange(expression);
    }

    /// <summary>
    /// Aplica el filtrado a la consulta.
    /// </summary>
    /// <param name="query">La consulta a ser filtrada.</param>
    /// <returns>La consulta filtrada.</returns>
    public IQueryable<T> Where(IQueryable<T> query)
    {
        foreach (var expression in _expressions)
        {
            query = expression.Where(query);
        }

        return query;
    }
}
