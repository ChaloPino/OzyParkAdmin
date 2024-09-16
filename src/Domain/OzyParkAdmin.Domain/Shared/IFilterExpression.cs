namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa una expressión de filtrado para el <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">El tipo del elemento a filtrar.</typeparam>
public interface IFilterExpression<T>
    where T : class
{
    /// <summary>
    /// Ejecuta el filtrado definido.
    /// </summary>
    /// <param name="query">La consulta que será filtrada.</param>
    /// <returns>Una nueva consulta filtrada.</returns>
    IQueryable<T> Where(IQueryable<T> query);
}