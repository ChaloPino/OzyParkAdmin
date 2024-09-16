using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa el ordenamiento usando el nombre de una propiedad y definiendo si es de forma descendente o ascendente.
/// </summary>
/// <typeparam name="T">El tipo del elemento a ordenar.</typeparam>
/// <typeparam name="TProperty">El tipo de la propiedad con la que se ordenará.</typeparam>
/// <param name="KeySelector">Una expressión para conseguir la propiedad que se usará para ordenar.</param>
/// <param name="Descending">Un flag que indica si se ordenará de forma descendente o ascendente.</param>
public sealed record SortExpression<T, TProperty>(Expression<Func<T, TProperty?>> KeySelector, bool Descending) : ISortExpression<T>
    where T : class
{
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