using System.Linq.Expressions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// El repositorio genérico de cualquier entidad.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad.</typeparam>
public interface IGenericRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Lista todas las entidades que coincidan con el criterio de búsqueda definido por <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">El criterio de búsqueda.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de <typeparamref name="TEntity"/>.</returns>
    Task<List<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista todas las entidades que coincidan con el criterio de búsqueda definido por <paramref name="predicate"/> y
    /// proyectado a <typeparamref name="TResult"/> usando <paramref name="selector"/>.
    /// </summary>
    /// <typeparam name="TResult">El tipo del resultado.</typeparam>
    /// <param name="selector">La función usada para proyectar el resultado.</param>
    /// <param name="predicate">El criterio de búsqueda.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La lista de <typeparamref name="TResult"/>.</returns>
    Task<List<TResult>> ListAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
}
