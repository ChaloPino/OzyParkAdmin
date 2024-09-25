using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.Shared;
using System.Linq.Expressions;

namespace OzyParkAdmin.Infrastructure.Shared;

/// <summary>
/// El repositorio de la entidad de tipo <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad.</typeparam>
/// <remarks>
/// Crea una nueva instancia de <see cref="GenericRepository{TEntity}"/>.
/// </remarks>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class GenericRepository<TEntity>(OzyParkAdminContext context) : Repository<TEntity>(context), IGenericRepository<TEntity>
    where TEntity : class
{
    /// <inheritdoc/>
    public async Task<List<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        SortExpressionCollection<TEntity>? sortExpressions = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = predicate is null
            ? EntitySet
            : EntitySet.Where(predicate);

        if (sortExpressions is not null)
        {
            query = sortExpressions.Sort(query);
        }

        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<List<TResult>> ListAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        SortExpressionCollection<TEntity>? sortExpressions = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(selector);

        IQueryable<TEntity> query = predicate is null
            ? EntitySet
            : EntitySet.Where(predicate);

        if (sortExpressions is not null)
        {
            query = sortExpressions.Sort(query);
        }

        return await query.Select(selector).ToListAsync(cancellationToken).ConfigureAwait(false);
    }
}
