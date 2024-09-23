using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Información del permiso de un servicio.
/// </summary>
public sealed record PermisoServicioInfo
{
    /// <summary>
    /// El tramo.
    /// </summary>
    public TramoInfo Tramo { get; init; } = default!;

    /// <summary>
    /// El centro de costo.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; init; } = default!;

    internal (int TramoId, int CentroCostoId) Id => (Tramo.Id, CentroCosto.Id);
}