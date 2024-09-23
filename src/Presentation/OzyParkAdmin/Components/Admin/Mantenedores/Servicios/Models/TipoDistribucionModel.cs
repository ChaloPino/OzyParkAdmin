namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// El tipo de distribución de un servicio.
/// </summary>
public sealed record TipoDistribucionModel
{
    /// <summary>
    /// El id del tipo de distribución.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El nombre del tipo de distribución.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Si el tipo de distribución está activo.
    /// </summary>
    public bool EsActivo { get; set; }
}