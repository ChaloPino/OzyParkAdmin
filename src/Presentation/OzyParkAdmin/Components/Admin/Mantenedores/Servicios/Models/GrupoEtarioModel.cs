namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// El grupo etario asociado a un servicio.
/// </summary>
public sealed record GrupoEtarioModel
{
    /// <summary>
    /// El id del grupo etario.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El aka del grupo etario.
    /// </summary>
    public string Aka { get; set; } = string.Empty;

    /// <summary>
    /// El nombre del grupo etario.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;
}