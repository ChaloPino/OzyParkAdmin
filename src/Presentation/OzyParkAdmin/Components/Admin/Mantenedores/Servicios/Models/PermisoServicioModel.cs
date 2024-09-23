using OzyParkAdmin.Components.Admin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// El permiso que tiene un servicio.
/// </summary>
public sealed record PermisoServicioModel
{
    /// <summary>
    /// El tramo.
    /// </summary>
    public TramoModel Tramo { get; set; } = default!;

    /// <summary>
    /// El centro de costo.
    /// </summary>
    public CentroCostoModel CentroCosto { get; set; } = default!;
}