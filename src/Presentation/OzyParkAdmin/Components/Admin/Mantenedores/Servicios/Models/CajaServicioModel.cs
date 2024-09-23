namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// La caja asociada al servicio.
/// </summary>
public sealed record CajaServicioModel
{
    /// <summary>
    /// La caja asociada al servicio.
    /// </summary>
    public CajaModel Caja { get; set; } = default!;

    /// <summary>
    /// Si no usa zona.
    /// </summary>
    public bool NoUsaZona { get; set; }

    /// <summary>
    /// Indica si la asociación es nueva.
    /// </summary>
    public bool IsNew { get; set; }
}