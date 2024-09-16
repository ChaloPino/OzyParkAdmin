using Microsoft.AspNetCore.Identity;

namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Inicio de sesión del usuario.
/// </summary>
public class UsuarioLogin
{
    /// <summary>
    /// Identificador del usuario al que pertenece este inicio de sesión..
    /// </summary>
    public Guid UserId { get; private set; }
    /// <summary>
    /// El proveedor del inicio de sesión (por ejemplo. facebook, google).
    /// </summary>
    public string LoginProvider { get; private set; } = string.Empty;

    /// <summary>
    /// La clave que representa el inicio de sesión para el proveedor.
    /// </summary>
    public string ProviderKey { get; private set; } = string.Empty;

    internal static UsuarioLogin Create(Usuario usuario, UserLoginInfo login) =>
        new()
        {
            UserId = usuario.Id,
            LoginProvider = login.LoginProvider,
            ProviderKey = login.ProviderKey,
        };
}