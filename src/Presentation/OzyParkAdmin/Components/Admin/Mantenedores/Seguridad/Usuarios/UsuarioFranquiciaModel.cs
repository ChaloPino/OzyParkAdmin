namespace OzyParkAdmin.Components.Admin.Mantenedores.Seguridad.Usuarios;

/// <summary>
/// Representa una franquicia asociada al usuario.
/// </summary>
public sealed record UsuarioFranquiciaModel
{
    /// <summary>
    /// El id de la franquicia.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El nombre de la franquicia.
    /// </summary>
    public required string Nombre { get; set; }
}
