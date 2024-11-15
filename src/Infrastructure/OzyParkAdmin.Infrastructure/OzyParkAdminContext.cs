using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OzyParkAdmin.Application;

namespace OzyParkAdmin.Infrastructure;

/// <summary>
/// Contexto de base de datos en Entity Framework usado por OzyParkAdmin.
/// </summary>
public sealed class OzyParkAdminContext : DbContext, IOzyParkAdminContext
{
    public OzyParkAdminContext(DbContextOptions<OzyParkAdminContext> options)
        : base(options)
    {
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OzyParkAdminContext).Assembly);
    }

    // Bulk Operations
    async Task IOzyParkAdminContext.BulkInsertAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        where TEntity : class
    {
        await this.BulkInsertAsync(entities, cancellationToken: cancellationToken);
    }

    async Task IOzyParkAdminContext.BulkDeleteAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        where TEntity : class
    {
        await this.BulkDeleteAsync(entities, cancellationToken: cancellationToken);
    }

    async Task IOzyParkAdminContext.BulkUpdateAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        where TEntity : class
    {
        await this.BulkUpdateAsync(entities, cancellationToken: cancellationToken);
    }

    // Standard CRUD Operations
    void IOzyParkAdminContext.Add<TEntity>(TEntity entity)
        where TEntity : class
    {
        Add(entity);
    }

    async Task IOzyParkAdminContext.AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
        where TEntity : class
    {
        await AddAsync(entity, cancellationToken);
    }

    void IOzyParkAdminContext.Attach<TEntity>(TEntity entity)
        where TEntity : class
    {
        Attach(entity);
    }

    void IOzyParkAdminContext.Remove<TEntity>(TEntity entity)
        where TEntity : class
    {
        Remove(entity);
    }

    void IOzyParkAdminContext.Update<TEntity>(TEntity entity)
        where TEntity : class
    {
        Update(entity);
    }

    // Transaction Management
    private IDbContextTransaction? _currentTransaction;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No transaction in progress to commit.");
        }

        await _currentTransaction.CommitAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No transaction in progress to roll back.");
        }

        await _currentTransaction.RollbackAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public override async ValueTask DisposeAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        await base.DisposeAsync();
    }
}
