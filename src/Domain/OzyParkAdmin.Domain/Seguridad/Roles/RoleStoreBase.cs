using Microsoft.AspNetCore.Identity;

namespace OzyParkAdmin.Domain.Seguridad.Roles;

/// <summary>
/// Representa una nueva instancia de un almacén de pesistencia para los roles.
/// </summary>
public abstract class RoleStoreBase :
    IQueryableRoleStore<Rol>
{
    private bool _disposed;

    /// <summary>
    /// Crea una nueva instancia.
    /// </summary>
    /// <param name="describer">El <see cref="IdentityErrorDescriber"/>.</param>
    protected RoleStoreBase(IdentityErrorDescriber describer)
    {
        ArgumentNullException.ThrowIfNull(describer);
        ErrorDescriber = describer;
    }

    /// <summary>
    /// El <see cref="IdentityErrorDescriber"/> para cualquier error que ocurrió con la operación actual.
    /// </summary>
    public IdentityErrorDescriber ErrorDescriber { get; }

    /// <inheritdoc/>
    public abstract IQueryable<Rol> Roles { get; }


    /// <summary>
    /// Llamado para cambiar el sello de concurrencia.
    /// </summary>
    /// <param name="role">El rol.</param>
    protected virtual void ChangeConcurrencyStamp(Rol role) =>
        role.ChangeConcurrencyStamp();

    /// <inheritdoc/>
    public abstract Task<IdentityResult> CreateAsync(Rol role, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<IdentityResult> UpdateAsync(Rol role, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<IdentityResult> DeleteAsync(Rol role, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public virtual Task<string> GetRoleIdAsync(Rol role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(role);
        return Task.FromResult(ConvertIdToString(role.Id)!);
    }

    /// <inheritdoc/>
    public virtual Task<string?> GetRoleNameAsync(Rol role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(role);
        return Task.FromResult<string?>(role.Name);
    }

    /// <inheritdoc/>
    public virtual Task SetRoleNameAsync(Rol role, string? roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(role);
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName);
        role.SetRoleName(roleName);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Convierte el <paramref name="id"/> proporcionado a <see cref="Guid"/>.
    /// </summary>
    /// <param name="id">El id a convertir.</param>
    /// <returns>Una instancia de <see cref="Guid"/> que representa el <paramref name="id"/> proporcionado.</returns>
    protected static Guid ConvertIdFromString(string? id)
    {
        return id is null ? Guid.Empty : Guid.Parse(id);
    }

    /// <summary>
    /// Convierte el <paramref name="id"/> proporcionado a su representación en <see cref="string"/>.
    /// </summary>
    /// <param name="id">El id a convertir.</param>
    /// <returns>Una representación en <see cref="string"/> del <paramref name="id"/> proporcionado.</returns>
    protected static string? ConvertIdToString(Guid id)
    {
        return id == Guid.Empty ? null : id.ToString();
    }

    /// <inheritdoc/>
    public abstract Task<Rol?> FindByIdAsync(string roleId, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<Rol?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public virtual Task<string?> GetNormalizedRoleNameAsync(Rol role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(role);
        return Task.FromResult<string?>(role.Name.ToUpperInvariant());
    }

    /// <inheritdoc/>
    public virtual Task SetNormalizedRoleNameAsync(Rol role, string? normalizedName, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Lanza un error si esta clase ha sido liberada.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }

    /// <summary>
    /// Libera el almacén.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Libera el almacén.
    /// </summary>
    /// <param name="disposing">Valor que indica si se está liberando la clase.</param>
    protected virtual void Dispose(bool disposing)
    {
        _disposed = true;
    }
}
