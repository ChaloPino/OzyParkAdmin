using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Domain.Seguridad.Roles;

/// <summary>
/// Entidad rol.
/// </summary>
public sealed class Rol
{
    private readonly List<UsuarioRol> _users = [];
    private readonly List<Rol> _childRoles = [];
    /// <summary>
    /// Identificador único del rol.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Nombre del rol.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Un valor aleatorio que debe cambiar si un rol es persistido en el almacén.
    /// </summary>
    public string? ConcurrencyStamp { get; private set; }

    /// <summary>
    /// Usuarios en el rol.
    /// </summary>
    public IEnumerable<UsuarioRol> Users => _users;

    /// <summary>
    /// Roles hijo.
    /// </summary>
    public IEnumerable<Rol> ChildRoles => _childRoles;

    internal void SetRoleName(string name)
    {
        Name = name;
    }

    internal void ChangeConcurrencyStamp() =>
        ConcurrencyStamp = Guid.NewGuid().ToString();
}
