using OzyParkAdmin.Components.Admin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// El nombre que tiene el servicio para un centro de costo.
/// </summary>
public sealed record CentroCostoServicioModel
{
    /// <summary>
    /// El centro de costo asociado.
    /// </summary>
    public CentroCostoModel CentroCosto { get; set; } = default!;

    /// <summary>
    /// El nombre que reemplaza.
    /// </summary>
    public string? Nombre { get; set; }
}