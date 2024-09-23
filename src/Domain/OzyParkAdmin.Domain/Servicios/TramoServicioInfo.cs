using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Información de la asociación entre un tramo con un servicio.
/// </summary>
public sealed record TramoServicioInfo
{
    /// <summary>
    /// El centro de costo.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; init; } = default!;

    /// <summary>
    /// El tramo.
    /// </summary>
    public TramoInfo Tramo { get; init; } = default!;

    /// <summary>
    /// El nombre.
    /// </summary>
    public string? Nombre { get; init; }

    /// <summary>
    /// La cantidad de permisos.
    /// </summary>
    public int? CantidadPermisos { get; init; }

    internal (int CentroCostoId, int TramoId) Id => (CentroCosto.Id, Tramo.Id);
}