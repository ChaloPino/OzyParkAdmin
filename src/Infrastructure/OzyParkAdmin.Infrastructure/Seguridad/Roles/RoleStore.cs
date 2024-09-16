using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.Seguridad.Roles;

namespace OzyParkAdmin.Infrastructure.Seguridad.Roles;

/// <summary>
/// Representa una nueva instancia de un almacén de persistencia para roles.
/// </summary>
public class RoleStore : RoleStoreBase
{
    /// <summary>
    /// Crea una nueva instancia.
    /// </summary>
    /// <param name="context">El contexto de base de datos.</param>
    /// <param name="describer">El <see cref="IdentityErrorDescriber"/> usado para describir los errores del almacén.</param>
    public RoleStore(OzyParkAdminContext context, IdentityErrorDescriber describer)
        : base(describer)
    {
        ArgumentNullException.ThrowIfNull(context);
        Context = context;
    }

    /// <summary>
    /// El contexto de base de datos para este almacén.
    /// </summary>
    public OzyParkAdminContext Context { get; }

    /// <inheritdoc/>
    public override IQueryable<Rol> Roles => Context.Set<Rol>();

    /// <summary>
    /// Flag que indica si los cambios deberían ser persistidos después de que sean llamados <see cref="CreateAsync"/>, <see cref="UpdateAsync"/> y <see cref="DeleteAsync"/>.
    /// </summary>
    public bool AutoSaveChanges { get; set; } = true;

    /// <summary>
    /// Guarda el almacén actual.
    /// </summary>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar las notificaciones que la operación debería ser cancelada.</param>
    /// <returns>El <see cref="Task"/> que representa la operación asíncrona.</returns>
    protected Task SaveChanges(CancellationToken cancellationToken) =>
        AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;

    /// <inheritdoc/>
    public override async Task<IdentityResult> CreateAsync(Rol role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(role);
        Context.Add(role);
        await SaveChanges(cancellationToken);
        return IdentityResult.Success;
    }

    /// <inheritdoc/>
    public override async Task<IdentityResult> UpdateAsync(Rol role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(role);
        Context.Attach(role);
        ChangeConcurrencyStamp(role);
        Context.Update(role);

        try
        {
            await SaveChanges(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    /// <inheritdoc/>
    public override async Task<IdentityResult> DeleteAsync(Rol role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(role);
        Context.Remove(role);

        try
        {
            await SaveChanges(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    /// <inheritdoc/>
    public override Task<Rol?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        var id = ConvertIdFromString(roleId);
        return Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public override Task<Rol?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        return Roles.FirstOrDefaultAsync(r => r.Name == normalizedRoleName, cancellationToken);
    }
}
