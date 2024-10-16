using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa una lista de <see cref="ISortExpression{T}"/>
/// </summary>
/// <typeparam name="T">El tipo de elemento que se usará para ordenar.</typeparam>
public class SortExpressionCollection<T>
    where T : class
{
    private readonly Dictionary<string, ISortExpression<T>> _expressions = new(StringComparer.OrdinalIgnoreCase);

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

        foreach (var expression in expressions)
        {
            _expressions[expression.MemberName] = expression;
        }
    }

    /// <summary>
    /// Agrega una expresión de ordenamiento.
    /// </summary>
    /// <typeparam name="TProperty">El tipo de la propiedad de ordenamiento.</typeparam>
    /// <param name="keySelector">La expresión lambda que contiene la propiedad de ordenamiento.</param>
    /// <param name="descending">Si el ordenamiento es descendente.</param>
    public SortExpressionCollection<T> Add<TProperty>(Expression<Func<T, TProperty?>> keySelector, bool descending) =>
        Add(new SortExpression<T, TProperty>(keySelector, descending));

    /// <summary>
    /// Agrega una expresión de ordenamiento.
    /// </summary>
    /// <param name="sortExpression">La expresión de ordenamiento.</param>
    public SortExpressionCollection<T> Add(ISortExpression<T> sortExpression)
    {
        _expressions[sortExpression.MemberName] = sortExpression;
        return this;
    }

    /// <summary>
    /// Reemplaza una expresión de ordenamiento por otra.
    /// </summary>
    /// <typeparam name="TProperty">El tipo de la propiedad.</typeparam>
    /// <param name="memberName">El nombre de la expresión de ordenamiento a reemplazar.</param>
    /// <param name="replacement">La expresión lambda que reemplazará a la actual.</param>
    /// <returns>El mismo <see cref="SortExpressionCollection{T}"/> de tal manera que se puedan hacer llamadas concatenadas.</returns>
    public SortExpressionCollection<T> Replace<TProperty>(string memberName, Expression<Func<T, TProperty>> replacement)
    {
        if (_expressions.TryGetValue(memberName, out var expression))
        {
            expression.Replace(replacement);
        }

        return this;
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

        List<ISortExpression<T>> sortExpressions = [.. _expressions.Values];

        IOrderedQueryable<T> orderedQuery = sortExpressions[0].Sort(query);

        for (int i = 1; i < sortExpressions.Count; i++)
        {
            orderedQuery = sortExpressions[i].ThenSort(orderedQuery);
        }

        return orderedQuery;
    }

    /// <summary>
    /// Aplica el ordenamiento a la consulta.
    /// </summary>
    /// <param name="source">La fuente a ser ordenada.</param>
    /// <returns>La consulta ordenada.</returns>
    public IEnumerable<T> Sort(IEnumerable<T> source)
    {
        if (_expressions.Count == 0)
        {
            return source;
        }

        List<ISortExpression<T>> sortExpressions = [.. _expressions.Values];

        IOrderedEnumerable<T> orderedSource = sortExpressions[0].Sort(source);

        for (int i = 1; i < sortExpressions.Count; i++)
        {
            orderedSource = sortExpressions[i].ThenSort(orderedSource);
        }

        return orderedSource;
    }
}
