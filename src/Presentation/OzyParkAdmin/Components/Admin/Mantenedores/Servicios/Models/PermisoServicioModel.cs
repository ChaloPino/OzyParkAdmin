using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// El permiso que tiene un servicio.
/// </summary>
public sealed record PermisoServicioModel
{
    /// <summary>
    /// El tramo.
    /// </summary>
    public TramoInfo Tramo { get; set; } = default!;

    /// <summary>
    /// El centro de costo.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; set; } = default!;
}