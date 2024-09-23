namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// El modelo de caja.
/// </summary>
public sealed record CajaModel
{
    /// <summary>
    /// El id de la caja.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El aka de la caja.
    /// </summary>
    public string Aka { get; set; } = string.Empty;

    /// <summary>
    /// El nombre de la caja.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

}