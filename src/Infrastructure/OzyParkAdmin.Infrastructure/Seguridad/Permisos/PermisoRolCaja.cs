using OzyParkAdmin.Domain.Seguridad.Roles;

namespace OzyParkAdmin.Infrastructure.Seguridad.Permisos;

/// <summary>
/// El permiso rol - caja.
/// </summary>
public sealed class PermisoRolCaja
{
    /// <summary>
    /// El id del rol.
    /// </summary>
    public required Guid RoleId { get; init; }

    /// <summary>
    /// El rol.
    /// </summary>
    public required Rol Rol { get; init; }

    /// <summary>
    /// Las acciones que puede hacer en la caja.
    /// </summary>
    public CajaAcciones CajaAcciones { get; init; }
}
