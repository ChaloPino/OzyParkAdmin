namespace OzyParkAdmin.Components.Admin.Mantenedores.Seguridad.Usuarios;

/// <summary>
/// Representa un centro de costo asociado al usuario.
/// </summary>
public sealed record UsuarioCentroCostoModel
{
    /// <summary>
    /// El id del centro de costo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El nombre del centro de costo.
    /// </summary>
    public required string Nombre { get; set; }
}
