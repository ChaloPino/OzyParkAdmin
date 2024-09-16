using OzyParkAdmin.Domain.Seguridad.Roles;

namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Rol del usuario.
/// </summary>
public sealed class UsuarioRol
{
    /// <summary>
    /// Id del usuario que está en el rol.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Id del rol.
    /// </summary>
    public Guid RoleId { get; private set; }

    internal static UsuarioRol Create(Usuario usuario, Rol rol) =>
        new()
        {
            UserId = usuario.Id,
            RoleId = rol.Id,
        };
}
