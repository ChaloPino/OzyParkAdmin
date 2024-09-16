using Microsoft.AspNetCore.Identity;

namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Token del usuario.
/// </summary>
public sealed class UsuarioToken
{
    /// <summary>
    /// Identificador del usuario.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// El proveedor de inicio de sesión de donde es este token.
    /// </summary>
    public string LoginProvider { get; private set; } = string.Empty;

    /// <summary>
    /// El nombre del token.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// El valor del token.
    /// </summary>
    [ProtectedPersonalData]
    public string? Value { get; private set; }

    internal static UsuarioToken Create(Usuario usuario, string loginProvider, string name, string? value) =>
        new()
        {
            UserId = usuario.Id,
            LoginProvider = loginProvider,
            Name = name,
            Value = value,
        };

    internal void SetValue(string? value)
    {
        Value = value;
    }
}
