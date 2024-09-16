namespace OzyParkAdmin.Components.Admin.Mantenedores.Seguridad.Usuarios;

/// <summary>
/// Representa un rol asociado al usuario.
/// </summary>
public sealed record UsuarioRolModel
{
    /// <summary>
    /// El id del rol.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// El nombre del rol.
    /// </summary>
    public required string Nombre { get; set; }
}
