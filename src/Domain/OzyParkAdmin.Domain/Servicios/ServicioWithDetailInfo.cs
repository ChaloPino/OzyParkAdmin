using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// La información de un servicio con los detalle de tramos y grupos etarios.
/// </summary>
public class ServicioWithDetailInfo : ServicioInfo
{
    /// <summary>
    /// Los tramos del servicio.
    /// </summary>
    public IEnumerable<TramoInfo> Tramos { get; set; } = [];

    /// <summary>
    /// Los grupos etarios asociados al servicio.
    /// </summary>
    public IEnumerable<GrupoEtarioInfo> GruposEtarios { get; set; } = [];
}
