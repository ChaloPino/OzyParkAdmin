using OzyParkAdmin.Domain.CentrosCosto;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Información sobre el centro de costo asociado al servicio.
/// </summary>
public sealed record CentroCostoServicioInfo
{
    /// <summary>
    /// El centro de costo asociado al servicio.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; init; } = default!;

    /// <summary>
    /// El nombre del servicio en la asociación con el centro de costo.
    /// </summary>
    public string? Nombre { get; set; }

}
