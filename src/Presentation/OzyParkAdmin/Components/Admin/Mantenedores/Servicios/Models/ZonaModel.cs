namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// La zona.
/// </summary>
public sealed record ZonaModel
{
    /// <summary>
    /// El id de la zona.
    /// </summary>
    public int Id { get; set; }


    /// <summary>
    /// El nombre de la zona.
    /// </summary>
    public string Nombre { get; set; } = default!;
}