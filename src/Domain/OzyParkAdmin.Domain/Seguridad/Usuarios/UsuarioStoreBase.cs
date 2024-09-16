using Microsoft.AspNetCore.Identity;
using OzyParkAdmin.Domain.Seguridad.Roles;
using System.Security.Claims;

namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Representa una nueva instancia de un almacén persistente para el usuario de tipo <see cref="Usuario"/>.
/// </summary>
public abstract class UsuarioStoreBase :
    IUserLoginStore<Usuario>,
    IUserClaimStore<Usuario>,
    IUserPasswordStore<Usuario>,
    IUserSecurityStampStore<Usuario>,
    IUserEmailStore<Usuario>,
    IUserLockoutStore<Usuario>,
    IUserPhoneNumberStore<Usuario>,
    IQueryableUserStore<Usuario>,
    IUserTwoFactorStore<Usuario>,
    IUserAuthenticationTokenStore<Usuario>,
    IUserAuthenticatorKeyStore<Usuario>,
    IUserTwoFactorRecoveryCodeStore<Usuario>,
    IUserRoleStore<Usuario>
{
    private bool _disposed;

    /// <summary>
    /// Crea una nueva instancia.
    /// </summary>
    /// <param name="describer">El <see cref="IdentityErrorDescriber"/> usado para describir los errores del almacén.</param>
    protected UsuarioStoreBase(IdentityErrorDescriber describer)
    {
        ArgumentNullException.ThrowIfNull(describer);
        ErrorDescriber = describer;
    }

    /// <summary>
    /// El <see cref="IdentityErrorDescriber"/> para cualquier error que ocurrió con la operación actual.
    /// </summary>
    public IdentityErrorDescriber ErrorDescriber { get; }

    /// <inheritdoc/>
    public abstract IQueryable<Usuario> Users { get; }

    /// <summary>
    /// Llamado para cambiar el sello de concurrencia.
    /// </summary>
    /// <param name="usuario">El usuario.</param>
    protected virtual void ChangeConcurrencyStamp(Usuario usuario) =>
        usuario.ChangeConcurrencyStamp();

    /// <summary>
    /// Llamado para crear una nueva instancia de <see cref="ClaimUsuario"/>.
    /// </summary>
    /// <param name="usuario">El usuario asociado.</param>
    /// <param name="claim">El claim asociado.</param>
    /// <returns>Una nueva instancia de <see cref="ClaimUsuario"/>.</returns>
    protected virtual ClaimUsuario CreateUserClaim(Usuario usuario, Claim claim) =>
        usuario.CreateClaim(claim);

    /// <summary>
    /// Llamado para reemplazar el <paramref name="claimUsuario"/>.
    /// </summary>
    /// <param name="claimUsuario">El <see cref="ClaimUsuario"/> a reemplazar.</param>
    /// <param name="newClaim">El nuevo claim que reemplaza the <paramref name="claimUsuario"/>.</param>
    protected virtual void ReplaceClaim(ClaimUsuario claimUsuario, Claim newClaim) =>
        claimUsuario.Replace(newClaim);

    /// <summary>
    /// Llamado para eliminar el <paramref name="claim"/>.
    /// </summary>
    /// <param name="usuario">El usuario asociado.</param>
    /// <param name="claim">El claim de usuario a eliminar.</param>
    protected virtual void RemoveClaims(Usuario usuario, Claim claim) =>
        usuario.RemoveClaims(claim);

    /// <summary>
    /// Llamado para crear una nueva instancia de <see cref="UsuarioLogin"/>.
    /// </summary>
    /// <param name="usuario">El usuario asociado.</param>
    /// <param name="login">El proveedor de inicio de sesión asociado.</param>
    /// <returns>Una nueva instancia de <see cref="UsuarioLogin"/>.</returns>
    protected virtual UsuarioLogin CreateUserLogin(Usuario usuario, UserLoginInfo login) =>
        usuario.CreateLogin(login);

    /// <summary>
    /// Llamado para eliminar un inicio de sesión del <paramref name="usuario"/>.
    /// </summary>
    /// <param name="usuario">El usuario asociado.</param>
    /// <param name="loginProvider">El proveedor de inicio de sesión asociado.</param>
    /// <param name="providerKey">La clave del proveedor.</param>
    protected virtual void RemoveUserLogin(Usuario usuario, string loginProvider, string providerKey) =>
        usuario.RemoveLogin(loginProvider, providerKey);

    /// <summary>
    /// Llamado para crear una nueva instancia de <see cref="UsuarioToken"/>.
    /// </summary>
    /// <param name="usuario">El usuario asociado.</param>
    /// <param name="loginProvider">El proveedor de inicio de sesión asociado.</param>
    /// <param name="name">El nombre del token del usuario.</param>
    /// <param name="value">El valor del token del usuario.</param>
    /// <returns>Una nueva instancia de <see cref="UsuarioToken"/>.</returns>
    protected virtual UsuarioToken CreateUserToken(Usuario usuario, string loginProvider, string name, string? value) =>
        usuario.CreateToken(loginProvider, name, value);

    /// <summary>
    /// Llamado para crear una nueva instance de <see cref="UsuarioRol"/>.
    /// </summary>
    /// <param name="usuario">El usuario asociado.</param>
    /// <param name="rol">El rol asociado.</param>
    /// <returns>Una nueva instancia de <see cref="UsuarioRol"/>.</returns>
    protected virtual UsuarioRol CreateUserRole(Usuario usuario, Rol rol) =>
        usuario.AssociateRole(rol);

    /// <summary>
    /// Llamado para eliminar una asociación usuario rol.
    /// </summary>
    /// <param name="usuario">El usuario asociado.</param>
    /// <param name="rol">El rol asociado.</param>
    protected virtual void RemoveUserRole(Usuario usuario, Rol rol) =>
        usuario.DesassociateRole(rol);

    /// <inheritdoc/>
    public abstract Task AddLoginAsync(Usuario user, UserLoginInfo login, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<IdentityResult> CreateAsync(Usuario user, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<IdentityResult> DeleteAsync(Usuario user, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<Usuario?> FindByIdAsync(string userId, CancellationToken cancellationToken);

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
    public virtual async Task<Usuario?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        var userLogin = await FindUserLoginAsync(loginProvider, providerKey, cancellationToken).ConfigureAwait(false);

        return userLogin is not null ? await FindUserAsync(userLogin.UserId, cancellationToken).ConfigureAwait(false) : null;
    }

    /// <inheritdoc/>
    public abstract Task<Usuario?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<IList<UserLoginInfo>> GetLoginsAsync(Usuario user, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public virtual Task<string?> GetNormalizedUserNameAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult<string?>(user.UserName.ToUpperInvariant());
    }

    /// <inheritdoc/>
    public virtual Task<string> GetUserIdAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(ConvertIdToString(user.Id)!);
    }

    /// <inheritdoc/>
    public virtual Task<string?> GetUserNameAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult<string?>(user.UserName);
    }

    /// <inheritdoc/>
    public abstract Task RemoveLoginAsync(Usuario user, string loginProvider, string providerKey, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public virtual Task SetNormalizedUserNameAsync(Usuario user, string? normalizedName, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task SetUserNameAsync(Usuario user, string? userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        user.SetUserName(userName);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public abstract Task<IdentityResult> UpdateAsync(Usuario user, CancellationToken cancellationToken);

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

    /// <summary>
    /// Lanza un error si esta clase ha sido liberada.
    /// </summary>
    protected void ThrowIfDisposed() =>
        ObjectDisposedException.ThrowIf(_disposed, this);

    /// <inheritdoc/>
    public abstract Task<IList<Claim>> GetClaimsAsync(Usuario user, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task AddClaimsAsync(Usuario user, IEnumerable<Claim> claims, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task ReplaceClaimAsync(Usuario user, Claim claim, Claim newClaim, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task RemoveClaimsAsync(Usuario user, IEnumerable<Claim> claims, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<IList<Usuario>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public virtual Task SetPasswordHashAsync(Usuario user, string? passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        user.ChangePasswordHash(passwordHash);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task<string?> GetPasswordHashAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.PasswordHash);
    }

    /// <inheritdoc/>
    public virtual Task<bool> HasPasswordAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.PasswordHash is not null);
    }

    /// <summary>
    /// Retorna un usuario con el <paramref name="userId"/> coincidente si existe.
    /// </summary>
    /// <param name="userId">El id del usuario.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El usuario si existe.</returns>
    protected abstract Task<Usuario?> FindUserAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Retorna un inicio de sesión del usuario con el <paramref name="userId"/>, <paramref name="loginProvider"/>, <paramref name="providerKey"/> coincidentes si existe.
    /// </summary>
    /// <param name="userId">El id del usuario.</param>
    /// <param name="loginProvider">El nombre del proveedor de inicio de sesión.</param>
    /// <param name="providerKey">La clave proporcionada por el <paramref name="loginProvider"/>.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El inicio de sesión si existe.</returns>
    protected abstract Task<UsuarioLogin?> FindUserLoginAsync(Guid userId, string loginProvider, string providerKey, CancellationToken cancellationToken);

    /// <summary>
    /// Retorna un inicio de sesión del usuario con el <paramref name="loginProvider"/>, <paramref name="providerKey"/> coincidentes si existe.
    /// </summary>
    /// <param name="loginProvider">El nombre del proveedor de inicio de sesión.</param>
    /// <param name="providerKey">La clave proporcionada por el <paramref name="loginProvider"/>.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El inicio de sesión si existe.</returns>
    protected abstract Task<UsuarioLogin?> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public virtual Task SetSecurityStampAsync(Usuario user, string stamp, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(stamp);
        user.SetSecurityStamp(stamp);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task<string?> GetSecurityStampAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult<string?>(user.SecurityStamp);
    }

    /// <inheritdoc/>
    public virtual Task SetEmailAsync(Usuario user, string? email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        user.SetEmail(email);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task<string?> GetEmailAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.Email);
    }

    /// <inheritdoc/>
    public virtual Task<bool> GetEmailConfirmedAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.EmailConfirmed);
    }

    /// <inheritdoc/>
    public virtual Task SetEmailConfirmedAsync(Usuario user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        user.ConfirmEmail(confirmed);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public abstract Task<Usuario?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public virtual Task<string?> GetNormalizedEmailAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.Email?.ToUpperInvariant());
    }

    /// <inheritdoc/>
    public virtual Task SetNormalizedEmailAsync(Usuario user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task SetPhoneNumberAsync(Usuario user, string? phoneNumber, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        user.SetPhoneNumber(phoneNumber);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task<string?> GetPhoneNumberAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.PhoneNumber);
    }

    /// <inheritdoc/>
    public virtual Task<bool> GetPhoneNumberConfirmedAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.PhoneNumberConfirmed);
    }

    /// <inheritdoc/>
    public virtual Task SetPhoneNumberConfirmedAsync(Usuario user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        user.ConfirmPhoneNumber(confirmed);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task SetTwoFactorEnabledAsync(Usuario user, bool enabled, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        user.SetTwoFactorEnabled(enabled);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task<bool> GetTwoFactorEnabledAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.TwoFactorEnabled);
    }

    /// <summary>
    /// Busca un token de usuario si existe.
    /// </summary>
    /// <param name="user">El propietario del token.</param>
    /// <param name="loginProvider">El proveedor de inicio de sesión para el token.</param>
    /// <param name="name">El nombre del token.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El token de usuario si existe.</returns>
    protected abstract Task<UsuarioToken?> FindTokenAsync(Usuario user, string loginProvider, string name, CancellationToken cancellationToken);

    /// <summary>
    /// Agrega un nuevo token de usuario.
    /// </summary>
    /// <param name="token">El token a ser agregado.</param>
    /// <returns>Un <see cref="Task"/> que representa la operación asíncrona.</returns>
    protected abstract Task AddUserTokenAsync(UsuarioToken token);

    /// <summary>
    /// Elimina un token de usuario.
    /// </summary>
    /// <param name="token">El token a ser eliminado.</param>
    /// <returns>Un <see cref="Task"/> que representa la operación asíncrona.</returns>
    protected abstract Task RemoveUserTokenAsync(UsuarioToken token);

    /// <inheritdoc/>
    public virtual async Task SetTokenAsync(Usuario user, string loginProvider, string name, string? value, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        var token = await FindTokenAsync(user, loginProvider, name, cancellationToken).ConfigureAwait(false);

        if (token is null)
        {
            await AddUserTokenAsync(CreateUserToken(user, loginProvider, name, value)).ConfigureAwait(false);
        }
        else
        {
            token.SetValue(value);
        }
    }

    /// <inheritdoc/>
    public virtual async Task RemoveTokenAsync(Usuario user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        var entry = await FindTokenAsync(user, loginProvider, name, cancellationToken).ConfigureAwait(false);

        if (entry is not null)
        {
            await RemoveUserTokenAsync(entry).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public virtual async Task<string?> GetTokenAsync(Usuario user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        var entry = await FindTokenAsync(user, loginProvider, name, cancellationToken).ConfigureAwait(false);
        return entry?.Value;
    }

    private const string InternalLoginProvider = "[AspNetUserStore]";
    private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
    private const string RecoveryCodeTokenName = "RecoveryCodes";

    /// <inheritdoc/>
    public virtual Task SetAuthenticatorKeyAsync(Usuario user, string key, CancellationToken cancellationToken) =>
        SetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);

    /// <inheritdoc/>
    public virtual Task<string?> GetAuthenticatorKeyAsync(Usuario user, CancellationToken cancellationToken) =>
        GetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);

    /// <inheritdoc/>
    public virtual Task ReplaceCodesAsync(Usuario user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
    {
        string mergedCodes = string.Join(";", recoveryCodes);
        return SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> RedeemCodeAsync(Usuario user, string code, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(code);

        string mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken).ConfigureAwait(false) ?? "";
        string[] splitCodes = mergedCodes.Split(';');

        if (splitCodes.Contains(code))
        {
            var updatedCodes = new List<string>(splitCodes.Where(s => s != code));
            await ReplaceCodesAsync(user, updatedCodes, cancellationToken).ConfigureAwait(false);
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountCodesAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        string? mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken).ConfigureAwait(false) ?? "";

        return mergedCodes.Length > 0 ? mergedCodes.AsSpan().Count(';') + 1 : 0;
    }

    /// <inheritdoc/>
    public abstract Task AddToRoleAsync(Usuario user, string roleName, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task RemoveFromRoleAsync(Usuario user, string roleName, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<IList<string>> GetRolesAsync(Usuario user, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<bool> IsInRoleAsync(Usuario user, string roleName, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<IList<Usuario>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken);

    /// <summary>
    /// Retorna un rol con el nombre normalizado si existe.
    /// </summary>
    /// <param name="normalizedRoleName">El nombre normalizado del rol.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El rol si existe.</returns>
    protected abstract Task<Rol?> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken);

    /// <summary>
    /// Retorna un usuario rol para el <paramref name="userId"/> y el <paramref name="roleId"/> si existe.
    /// </summary>
    /// <param name="userId">El id del usuario.</param>
    /// <param name="roleId">El id del rol.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El usuario rol si existe.</returns>
    protected abstract Task<UsuarioRol?> FindUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public virtual Task<DateTimeOffset?> GetLockoutEndDateAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.GetLockoutEndDate());
    }

    /// <inheritdoc/>
    public virtual Task SetLockoutEndDateAsync(Usuario user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        user.SetLockoutEndDate(lockoutEnd);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task<int> IncrementAccessFailedCountAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        user.IncrementAccessFailedCount();
        return Task.FromResult(user.AccessFailedCount);
    }

    /// <inheritdoc/>
    public virtual Task ResetAccessFailedCountAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        user.ResetAccessFailedCount();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task<int> GetAccessFailedCountAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.AccessFailedCount);
    }

    /// <inheritdoc/>
    public virtual Task<bool> GetLockoutEnabledAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.LockoutEnabled);
    }

    /// <inheritdoc/>
    public virtual Task SetLockoutEnabledAsync(Usuario user, bool enabled, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        user.SetLockoutEnabled(enabled);
        return Task.CompletedTask;
    }
}
