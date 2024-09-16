namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa una lista de <see cref="ISortExpression{T}"/>
/// </summary>
/// <typeparam name="T">El tipo de elemento que se usará para ordenar.</typeparam>
public class SortExpressionCollection<T>
    where T : class
{
    private readonly List<ISortExpression<T>> _expressions = [];

    /// <summary>
    /// Crea una nueva instancia de <see cref="SortExpressionCollection{T}"/>.
    /// </summary>
    public SortExpressionCollection()
        : this([])
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="SortExpressionCollection{T}"/> con una lista de <see cref="ISortExpression{T}"/> para inicializar.
    /// </summary>
    /// <param name="expressions">La lista de <see cref="ISortExpression{T}"/> para inicializar la nueva instancia</param>
    public SortExpressionCollection(IEnumerable<ISortExpression<T>> expressions)
    {
        ArgumentNullException.ThrowIfNull(expressions);
        _expressions.AddRange(expressions);
    }

    /// <summary>
    /// Aplica el ordenamiento a la consulta.
    /// </summary>
    /// <param name="query">La consulta a ser ordenada.</param>
    /// <returns>La consulta ordenada.</returns>
    public IQueryable<T> Sort(IQueryable<T> query)
    {
        if (_expressions.Count == 0)
        {
            return query;
        }

        IOrderedQueryable<T> orderedQuery = _expressions[0].Sort(query);

        for (int i = 1; i < _expressions.Count; i++)
        {
            orderedQuery = _expressions[i].ThenSort(orderedQuery);
        }

        return orderedQuery;
    }
}
