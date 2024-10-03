using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Models;

/// <summary>
/// El modelo para la búsqueda de cupos y presentación en el calendario.
/// </summary>
public class CalendarioSearchModel
{
    /// <summary>
    /// El centro de costo.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; set; } = default!;

    /// <summary>
    /// El canal de venta.
    /// </summary>
    public CanalVenta CanalVenta { get; set; } = default!;

    /// <summary>
    /// La funcionalidad que se está consultado.
    /// </summary>
    public CupoAlcance Alcance { get; set; } = CupoAlcance.Venta;

    /// <summary>
    /// El servicio.
    /// </summary>
    public ServicioInfo Servicio { get; set; } = default!;


    /// <summary>
    /// La zona de origen.
    /// </summary>
    public ZonaInfo? ZonaOrigen { get; set; }
}
