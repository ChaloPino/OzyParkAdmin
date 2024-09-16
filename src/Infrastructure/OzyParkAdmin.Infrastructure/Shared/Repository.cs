using Microsoft.EntityFrameworkCore;

namespace OzyParkAdmin.Infrastructure.Shared;

/// <summary>
/// Representa el repositorio base que usa Entity Framework.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad.</typeparam>
public abstract class Repository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Crea una nueva instance.
    /// </summary>
    /// <param name="context">El contexto de base de datos.</param>
    protected Repository(OzyParkAdminContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        Context = context;
        EntitySet = context.Set<TEntity>();
    }

    /// <summary>
    /// El <see cref="OzyParkAdminContext"/> usado.
    /// </summary>
    public OzyParkAdminContext Context { get; }

    /// <summary>
    /// El <see cref="DbSet{TEntity}"/>.
    /// </summary>
    public DbSet<TEntity> EntitySet { get; }
}
