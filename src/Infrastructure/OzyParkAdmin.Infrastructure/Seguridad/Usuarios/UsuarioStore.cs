using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.Seguridad.Roles;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Infrastructure.Properties;
using System.Globalization;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Seguridad.Usuarios;

/// <summary>
/// Representa una nueva instancia de un almacén de persistencia para usuarios.
/// </summary>
public class UsuarioStore : UsuarioStoreBase, IProtectedUserStore<Usuario>
{
    /// <summary>
    /// Crea una nueva instancia.
    /// </summary>
    /// <param name="context">El contexto de base de datos.</param>
    /// <param name="describer">El <see cref="IdentityErrorDescriber"/> usado para describir los errores del almacén.</param>
    public UsuarioStore(OzyParkAdminContext context, IdentityErrorDescriber describer)
        : base(describer)
    {
        ArgumentNullException.ThrowIfNull(context);
        Context = context;
    }

    /// <summary>
    /// El contexto de base de datos para este almacén.
    /// </summary>
    public OzyParkAdminContext Context { get; }

    private DbSet<Usuario> UsersSet => Context.Set<Usuario>();
    private DbSet<Rol> Roles => Context.Set<Rol>();
    private DbSet<ClaimUsuario> UserClaims => Context.Set<ClaimUsuario>();
    private DbSet<UsuarioRol> UserRoles => Context.Set<UsuarioRol>();
    private DbSet<UsuarioLogin> UserLogins => Context.Set<UsuarioLogin>();
    private DbSet<UsuarioToken> UserTokens => Context.Set<UsuarioToken>();

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
    public override async Task<IdentityResult> CreateAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        Context.Add(user);
        await SaveChanges(cancellationToken);
        return IdentityResult.Success;
    }

    /// <inheritdoc/>
    public override async Task<IdentityResult> UpdateAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        Context.Attach(user);
        ChangeConcurrencyStamp(user);
        Context.Update(user);

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
    public override async Task<IdentityResult> DeleteAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        Context.Remove(user);

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
    public override Task<Usuario?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        Guid id = ConvertIdFromString(userId);
        return UsersSet.SingleOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public override Task<Usuario?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        return Users.AsSplitQuery().FirstOrDefaultAsync(u => u.UserName == normalizedUserName, cancellationToken);
    }

    /// <inheritdoc/>
    public override IQueryable<Usuario> Users => UsersSet;

    /// <inheritdoc/>
    protected override Task<Rol?> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken) =>
        Roles.SingleOrDefaultAsync(r => r.Name == normalizedRoleName, cancellationToken);

    /// <inheritdoc/>
    protected override Task<UsuarioRol?> FindUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken) =>
        UserRoles.FindAsync([userId, roleId], cancellationToken).AsTask();

    /// <inheritdoc/>
    protected override Task<Usuario?> FindUserAsync(Guid userId, CancellationToken cancellationToken) =>
        Users.AsSplitQuery().SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);

    /// <inheritdoc/>
    protected override Task<UsuarioLogin?> FindUserLoginAsync(Guid userId, string loginProvider, string providerKey, CancellationToken cancellationToken) =>
        UserLogins.SingleOrDefaultAsync(userLogin => userLogin.UserId == userId && userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);

    /// <inheritdoc/>
    protected override Task<UsuarioLogin?> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken) =>
        UserLogins.SingleOrDefaultAsync(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);

    /// <inheritdoc/>
    public override async Task AddToRoleAsync(Usuario user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName);

        var roleEntity = await FindRoleAsync(roleName, cancellationToken) ?? throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.RoleNotFound, roleName));
        UserRoles.Add(CreateUserRole(user, roleEntity));
    }

    /// <inheritdoc/>
    public override async Task RemoveFromRoleAsync(Usuario user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName);

        var roleEntity = await FindRoleAsync(roleName, cancellationToken);

        if (roleEntity is not null)
        {
            var userRole = await FindUserRoleAsync(user.Id, roleEntity.Id, cancellationToken);

            if (userRole is not null)
            {
                RemoveUserRole(user, roleEntity);
                UserRoles.Remove(userRole);
            }
        }
    }

    /// <inheritdoc/>
    public override async Task<IList<string>> GetRolesAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        var userId = user.Id;

        var query = from userRole in UserRoles
                    join role in Roles on userRole.RoleId equals role.Id
                    where userRole.UserId == userId
                    select role.Name;

        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task<bool> IsInRoleAsync(Usuario user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName);

        var role = await FindRoleAsync(roleName, cancellationToken);

        if (role is not null)
        {
            var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
            return userRole is not null;
        }

        return false;
    }

    /// <inheritdoc/>
    public override async Task<IList<Claim>> GetClaimsAsync(Usuario user, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        return await UserClaims.Where(uc => uc.UserId == user.Id).Select(c => c.ToClaim()).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override Task AddClaimsAsync(Usuario user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(claims);

        foreach (var claim in claims)
        {
            UserClaims.Add(CreateUserClaim(user, claim));
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override async Task ReplaceClaimAsync(Usuario user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(claim);
        ArgumentNullException.ThrowIfNull(newClaim);

        var matchedClaims = await UserClaims.Where(uc => uc.UserId == user.Id && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync(cancellationToken);

        foreach (var matchedClaim in matchedClaims)
        {
            ReplaceClaim(matchedClaim, newClaim);
        }
    }

    /// <inheritdoc/>
    public override async Task RemoveClaimsAsync(Usuario user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(claims);

        foreach (var claim in claims)
        {
            var matchedClaims = await UserClaims.Where(uc => uc.UserId == user.Id && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync(cancellationToken);

            foreach (var c in matchedClaims)
            {
                RemoveClaims(user, claim);
                UserClaims.Remove(c);
            }
        }
    }

    /// <inheritdoc/>
    public override Task AddLoginAsync(Usuario user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(login);

        UserLogins.Add(CreateUserLogin(user, login));
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override async Task RemoveLoginAsync(Usuario user, string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        var entry = await FindUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);

        if (entry is not null)
        {
            RemoveUserLogin(user, loginProvider, providerKey);
            UserLogins.Remove(entry);
        }
    }

    /// <inheritdoc/>
    public override async Task<IList<UserLoginInfo>> GetLoginsAsync(Usuario user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(user);

        var userId = user.Id;
        return await UserLogins.Where(l => l.UserId == userId)
            .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderKey)).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task<Usuario?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        var userLogin = await FindUserLoginAsync(loginProvider, providerKey, cancellationToken);

        return userLogin is not null ? await FindUserAsync(userLogin.UserId, cancellationToken) : null;
    }

    /// <inheritdoc/>
    public override Task<Usuario?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        return Users.SingleOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task<IList<Usuario>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(claim);

        var query = from userClaims in UserClaims
                    join user in Users on userClaims.UserId equals user.Id
                    where userClaims.ClaimValue == claim.Value
                    && userClaims.ClaimType == claim.Type
                    select user;

        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task<IList<Usuario>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        ArgumentException.ThrowIfNullOrEmpty(roleName);

        var role = await FindRoleAsync(roleName, cancellationToken);

        if (role is not null)
        {
            var query = from userrole in UserRoles
                        join user in Users on userrole.UserId equals user.Id
                        where userrole.RoleId == role.Id
                        select user;

            return await query.ToListAsync(cancellationToken);
        }

        return [];
    }

    /// <inheritdoc/>
    protected override Task<UsuarioToken?> FindTokenAsync(Usuario user, string loginProvider, string name, CancellationToken cancellationToken) =>
        UserTokens.FindAsync([user.Id, loginProvider, name], cancellationToken).AsTask();

    /// <inheritdoc/>
    protected override Task AddUserTokenAsync(UsuarioToken token)
    {
        UserTokens.Add(token);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override Task RemoveUserTokenAsync(UsuarioToken token)
    {
        UserTokens.Remove(token);
        return Task.CompletedTask;
    }
}
