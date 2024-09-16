using OzyParkAdmin.Domain.Seguridad.Roles;

namespace OzyParkAdmin.Infrastructure.Seguridad.Permisos;

/// <summary>
/// El permiso por rol.
/// </summary>
public sealed class PermisoRol
{
    /// <summary>
    /// El id del permiso por rol.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// El controller.
    /// </summary>
    public string Recurso { get; init; } = string.Empty;

    /// <summary>
    /// El rol asociado.
    /// </summary>
    public Rol Rol { get; init; } = default!;
}
