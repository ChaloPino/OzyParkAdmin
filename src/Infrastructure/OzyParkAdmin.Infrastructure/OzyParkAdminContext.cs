using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Application;

namespace OzyParkAdmin.Infrastructure;

/// <summary>
/// Context de base de datos en Entity Framework usado por OzyParkAdmin.
/// </summary>
/// <remarks>
/// Crea una nueva instancia.
/// </remarks>
/// <param name="options">Las opciones a ser usadas por <see cref="OzyParkAdminContext"/>.</param>
public sealed class OzyParkAdminContext(DbContextOptions<OzyParkAdminContext> options) : DbContext(options), IOzyParkAdminContext
{
    /// <inheritdoc/>
    async Task IOzyParkAdminContext.BulkInsertAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        where TEntity : class
    {
        await this.BulkInsertAsync(entities, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    async Task IOzyParkAdminContext.BulkDeleteAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        where TEntity : class
    {
        await this.BulkDeleteAsync(entities, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    async Task IOzyParkAdminContext.BulkUpdateAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        where TEntity : class
    {
        await this.BulkUpdateAsync(entities, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OzyParkAdminContext).Assembly);
    }

    void IOzyParkAdminContext.Add<TEntity>(TEntity entity)
    {
        Add(entity);
    }

    async Task IOzyParkAdminContext.AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
    {
        await AddAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    void IOzyParkAdminContext.Attach<TEntity>(TEntity entity)
    {
        Attach(entity);
    }

    void IOzyParkAdminContext.Remove<TEntity>(TEntity entity)
    {
        Remove(entity);
    }

    void IOzyParkAdminContext.Update<TEntity>(TEntity entity)
    {
        Update(entity);
    }
}
