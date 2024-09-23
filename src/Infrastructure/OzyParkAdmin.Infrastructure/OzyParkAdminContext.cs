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
