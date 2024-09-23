using Microsoft.AspNetCore.Identity;
using OzyParkAdmin.Domain.Seguridad.Roles;
using System.Reflection.PortableExecutable;
using System.Security.Claims;

namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Entidad usuario.
/// </summary>
public sealed class Usuario
{
    private readonly List<UsuarioRol> _roles = [];
    private readonly List<ClaimUsuario> _claims = [];
    private readonly List<UsuarioLogin> _logins = [];
    private readonly List<UsuarioToken> _tokens = [];
    private readonly List<CentroCostoUsuario> _centrosCosto = [];
    private readonly List<FranquiciaUsuario> _franquicias = [];

    /// <summary>
    /// Identificador único del usuario.
    /// </summary>
    [PersonalData]
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Nombre de usuario.
    /// </summary>
    [ProtectedPersonalData]
    public string UserName { get; private set; } = string.Empty;

    /// <summary>
    /// Una representación salt y hash de la contraseña de este usuario.
    /// </summary>
    public string? PasswordHash { get; private set; }

    /// <summary>
    /// Email.
    /// </summary>
    [ProtectedPersonalData]
    public string? Email { get; private set; }

    /// <summary>
    /// Si el email está confirmado o no.
    /// </summary>
    [PersonalData]
    public bool EmailConfirmed { get; private set; }

    /// <summary>
    /// Un valor aleatorio que debería cambiar si las credenciales del usuario cambiaron (cambio de contraseña, inicio de sesión eliminado).
    /// </summary>
    public string SecurityStamp { get; private set; } = string.Empty;

    /// <summary>
    /// Un valor aleatorio que debe cambiar si un usuario es persistido en el almacén.
    /// </summary>
    public string? ConcurrencyStamp { get; private set; }

    /// <summary>
    /// Teléfono del usuario.
    /// </summary>
    [ProtectedPersonalData]
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Si el teléfono está confirmado o no.
    /// </summary>
    [PersonalData]
    public bool PhoneNumberConfirmed { get; private set; }

    /// <summary>
    /// Si el doble factor está habilitado para el usuario.
    /// </summary>
    [PersonalData]
    public bool TwoFactorEnabled { get; private set; }

    /// <summary>
    /// Fecha y hora en UTC cuando el bloqueo termina, cualquir hora en el pasado es considerado no bloqueado.
    /// </summary>
    public DateTime? LockoutEndDateUtc { get; private set; }

    /// <summary>
    /// Si el bloqueo está habilitado para este usuario.
    /// </summary>
    public bool LockoutEnabled { get; private set; }

    /// <summary>
    /// Usado para registrar las fallas con el fin de bloquear.
    /// </summary>
    public int AccessFailedCount { get; private set; }

    /// <summary>
    /// El nombre completo del usuario.
    /// </summary>
    public string FriendlyName { get; private set; } = string.Empty;

    /// <summary>
    /// El rut del usuario.
    /// </summary>
    public string? Rut { get; private set; }

    /// <summary>
    /// Si el usuario debe cambiar su contraseña en el siguiente inicio de sesión.
    /// </summary>
    public bool ChangePasswordNextLogon { get; private set; }

    /// <summary>
    /// Roles del usuario.
    /// </summary>
    public IEnumerable<UsuarioRol> Roles => _roles;

    /// <summary>
    /// Claims del usuario.
    /// </summary>
    public IEnumerable<ClaimUsuario> Claims => _claims;

    /// <summary>
    /// Inicios de sesión del usuario.
    /// </summary>
    public IEnumerable<UsuarioLogin> Logins => _logins;

    /// <summary>
    /// Tokens del usuario.
    /// </summary>
    public IEnumerable<UsuarioToken> Tokens => _tokens;

    /// <summary>
    /// Centros de costo del usuario.
    /// </summary>
    public IEnumerable<CentroCostoUsuario> CentrosCosto => _centrosCosto;

    /// <summary>
    /// Franquicias del usuario.
    /// </summary>
    public IEnumerable<FranquiciaUsuario> Franquicias => _franquicias;

    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    /// <param name="userName">El nombre de usuario.</param>
    /// <param name="friendlyName">El nombre completo del usuario.</param>
    /// <param name="email">La dirección de correo electrónico del usuario.</param>
    /// <returns>El nuevo usuario creado.</returns>
    public static Usuario Create(string userName, string friendlyName, string? rut, string? email)
    {
        return new Usuario
        {
            UserName = userName,
            FriendlyName = friendlyName,
            Rut = rut,
            Email = email,
            ChangePasswordNextLogon = false,
        };
    }

    internal void ChangeConcurrencyStamp() =>
        ConcurrencyStamp = Guid.NewGuid().ToString();

    internal ClaimUsuario CreateClaim(Claim claim)
    {
        ClaimUsuario claimUsuario = ClaimUsuario.Create(this, claim);
        _claims.Add(claimUsuario);
        return claimUsuario;
    }

    internal void RemoveClaims(Claim claim)
    {
        foreach (var claimUsuario in _claims.FindAll(c => string.Equals(c.ClaimType, claim.Type, StringComparison.OrdinalIgnoreCase) && string.Equals(c.ClaimValue, claim.Value, StringComparison.OrdinalIgnoreCase)))
        {
            _claims.Remove(claimUsuario);
        }
    }

    internal UsuarioLogin CreateLogin(UserLoginInfo login)
    {
        UsuarioLogin usuarioLogin = UsuarioLogin.Create(this, login);
        _logins.Add(usuarioLogin);
        return usuarioLogin;
    }

    internal void RemoveLogin(string loginProvider, string providerKey)
    {
        UsuarioLogin? usuarioLogin = _logins.Find(l => string.Equals(l.LoginProvider, loginProvider, StringComparison.OrdinalIgnoreCase) && string.Equals(l.ProviderKey, providerKey, StringComparison.OrdinalIgnoreCase));

        if (usuarioLogin is not null)
        {
            _logins.Remove(usuarioLogin);
        }
    }

    internal UsuarioToken CreateToken(string loginProvider, string name, string? value)
    {
        UsuarioToken usuarioToken = UsuarioToken.Create(this, loginProvider, name, value);
        _tokens.Add(usuarioToken);
        return usuarioToken;
    }

    internal void ChangePasswordHash(string? passwordHash)
    {
        PasswordHash = passwordHash;
    }

    internal void SetEmail(string? email)
    {
        Email = email;
    }

    internal void ConfirmEmail(bool confirmed)
    {
        EmailConfirmed = confirmed;
    }

    internal void SetLockoutEnabled(bool enabled)
    {
        LockoutEnabled = enabled;
    }

    internal DateTimeOffset? GetLockoutEndDate() =>
        LockoutEndDateUtc is null ? null : new DateTimeOffset(LockoutEndDateUtc.Value);

    internal void SetLockoutEndDate(DateTimeOffset? lockoutEnd) =>
        LockoutEndDateUtc = lockoutEnd?.DateTime;

    internal void IncrementAccessFailedCount()
    {
        AccessFailedCount++;
    }

    internal void ResetAccessFailedCount()
    {
        AccessFailedCount = 0;
    }

    internal void SetPhoneNumber(string? phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }

    internal void ConfirmPhoneNumber(bool confirmed)
    {
        PhoneNumberConfirmed = confirmed;
    }

    internal void SetSecurityStamp(string stamp)
    {
        SecurityStamp = stamp;
    }

    internal void SetTwoFactorEnabled(bool enabled)
    {
        TwoFactorEnabled = enabled;
    }

    internal void SetUserName(string userName)
    {
        UserName = userName;
    }

    internal UsuarioRol AssociateRole(Rol rol)
    {
        UsuarioRol usuarioRol = UsuarioRol.Create(this, rol);
        _roles.Add(usuarioRol);
        return usuarioRol;
    }

    internal void DesassociateRole(Rol rol)
    {
        UsuarioRol? usuarioRol = _roles.Find(ur => ur.RoleId == rol.Id);

        if (usuarioRol is not null)
        {
            _roles.Remove(usuarioRol);
        }
    }

    internal void SetFriendlyName(string friendlyName)
    {
        FriendlyName = friendlyName;
    }

    internal void SetRut(string? rut)
    {
        Rut = rut;
    }

    internal void NeedToChangePassword(bool needed)
    {
        ChangePasswordNextLogon = needed;
    }

    internal void AddCentrosCosto(IEnumerable<int> centrosCosto)
    {
        foreach (int centroCosto in centrosCosto)
        {
            _centrosCosto.Add(CentroCostoUsuario.Create(this, centroCosto));
        }
    }

    internal void RemoveCentrosCosto(IEnumerable<int> centrosCosto)
    {
        foreach (int centroCosto in centrosCosto)
        {
            CentroCostoUsuario? centroCostoToRemove = _centrosCosto.Find(ucc => ucc.CentroCostoId == centroCosto);

            if (centroCostoToRemove is not null)
            {
                _centrosCosto.Remove(centroCostoToRemove);
            }
        }
    }

    internal void AddFranquicias(IEnumerable<int> franquicias)
    {
        foreach (int franquicia in franquicias)
        {
            _franquicias.Add(FranquiciaUsuario.Create(this, franquicia));
        }
    }

    internal void RemoveFranquicias(IEnumerable<int> franquicias)
    {
        foreach (int franquicia in franquicias)
        {
            FranquiciaUsuario? franquiciasToRemove = _franquicias.Find(uf => uf.FranquiciaId == franquicia);

            if (franquiciasToRemove is not null)
            {
                _franquicias.Remove(franquiciasToRemove);
            }
        }
    }
}
