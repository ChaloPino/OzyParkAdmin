using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa una lista de <see cref="IFilterExpression{T}"/>.
/// </summary>
/// <typeparam name="T">El tipo del elemento a filtrar.</typeparam>
public class FilterExpressionCollection<T>
    where T : class
{
    private readonly Dictionary<string, IFilterExpression<T>> _expressions = new(StringComparer.OrdinalIgnoreCase);

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
    /// <param name="expressions">La lista de <see cref="IFilterExpression{T}"/> para inicializar la nueva instancia.</param>
    public FilterExpressionCollection(IEnumerable<IFilterExpression<T>> expressions)
    {
        ArgumentNullException.ThrowIfNull(expressions);

        foreach (IFilterExpression<T> expression in expressions)
        {
            _expressions[expression.MemberName] = expression;
        }
    }

    /// <summary>
    /// Agrega una expresión de filtrado.
    /// </summary>
    /// <param name="filterExpression">La expresión de filtrado.</param>
    public void Add(IFilterExpression<T> filterExpression)
    {
        _expressions[filterExpression.MemberName] = filterExpression;
    }

    /// <summary>
    /// Reemplaza uno de las expresiones de filtro.
    /// </summary>
    /// <typeparam name="TProperty">El tipo de la propiedad.</typeparam>
    /// <param name="memberName">El nombre del filtro que se va a reemplazar.</param>
    /// <param name="replacement">La expresión lambda con que se reemplazará.</param>
    /// <returns>El mismo <see cref="FilterExpressionCollection{T}"/> de tal forma que se pueda concatenar otras llamadas.</returns>
    public FilterExpressionCollection<T> Replace<TProperty>(string memberName, Expression<Func<T, TProperty>> replacement)
    {
        if (_expressions.TryGetValue(memberName, out var filterExpression))
        {
            filterExpression.Replace(replacement);
        }

        return this;
    }


    /// <summary>
    /// Aplica el filtrado a la consulta.
    /// </summary>
    /// <param name="query">La consulta a ser filtrada.</param>
    /// <returns>La consulta filtrada.</returns>
    public IQueryable<T> Where(IQueryable<T> query)
    {
        foreach (var expression in _expressions.Values)
        {
            query = expression.Where(query);
        }

        return query;
    }

}
