using OzyParkAdmin.Domain.Shared.Utils;
using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa el ordenamiento usando el nombre de una propiedad y definiendo si es de forma descendente o ascendente.
/// </summary>
/// <typeparam name="T">El tipo del elemento a ordenar.</typeparam>
/// <typeparam name="TProperty">El tipo de la propiedad con la que se ordenará.</typeparam>
public sealed class SortExpression<T, TProperty> : ISortExpression<T>
    where T : class
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="SortExpression{T, TProperty}"/>.
    /// </summary>
    /// <param name="keySelector">Una expressión para conseguir la propiedad que se usará para ordenar.</param>
    /// <param name="descending">Un flag que indica si se ordenará de forma descendente o ascendente.</param>
    public SortExpression(Expression<Func<T, TProperty?>> keySelector, bool descending)
    {
        ArgumentNullException.ThrowIfNull(keySelector);
        KeySelector = keySelector;
        Descending = descending;

        MemberName = PropertyPath.Visit(KeySelector).GetPath();
    }

    /// <inheritdoc/>
    public Expression<Func<T, TProperty?>> KeySelector { get; private set; }

    /// <inheritdoc/>
    public bool Descending { get; }

    /// <inheritdoc/>
    public string MemberName { get; }

    void ISortExpression<T>.Replace(LambdaExpression replacement) =>
        KeySelector = (Expression<Func<T, TProperty?>>)replacement;

    /// <inheritdoc/>
    public IOrderedQueryable<T> Sort(IQueryable<T> query) =>
        !Descending
            ? query.OrderBy(KeySelector)
            : query.OrderByDescending(KeySelector);

    /// <inheritdoc/>
    public IOrderedQueryable<T> ThenSort(IOrderedQueryable<T> query) =>
        !Descending
            ? query.ThenBy(KeySelector)
            : query.ThenByDescending(KeySelector);
}