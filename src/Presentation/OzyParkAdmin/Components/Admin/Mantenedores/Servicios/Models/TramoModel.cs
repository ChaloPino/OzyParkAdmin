namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// El tramo.
/// </summary>
public sealed record TramoModel
{
    /// <summary>
    /// El id del tramo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El aka del tramo.
    /// </summary>
    public string Aka { get; set; } = string.Empty;

    /// <summary>
    /// El nombre del tramo.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;
}