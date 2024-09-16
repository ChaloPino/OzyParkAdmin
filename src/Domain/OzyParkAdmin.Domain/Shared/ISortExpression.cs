namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa una expressión de ordenamiento para el <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">El tipo de elemento a ordenar.</typeparam>
public interface ISortExpression<T>
    where T : class
{
    /// <summary>
    /// Ejecuta el ordenamiento definido.
    /// </summary>
    /// <param name="query">La consulta que será ordenada.</param>
    /// <returns>Una nueva consulta ordenada.</returns>
    IOrderedQueryable<T> Sort(IQueryable<T> query);

    /// <summary>
    /// Ejecuta el ordenamiento definido.
    /// </summary>
    /// <param name="query">La consulta que será ordenada.</param>
    /// <returns>Una nueva consulta ordenada.</returns>
    IOrderedQueryable<T> ThenSort(IOrderedQueryable<T> query);
}