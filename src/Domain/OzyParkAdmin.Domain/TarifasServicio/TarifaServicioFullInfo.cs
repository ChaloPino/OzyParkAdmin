using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Domain.TarifasServicio;

/// <summary>
/// La información completa de una tarifa de servicio.
/// </summary>
public sealed class TarifaServicioFullInfo
{
    /// <summary>
    /// El inicio de vigencia de la tarifa.
    /// </summary>
    public DateTime InicioVigencia { get; set; }

    /// <summary>
    /// La moneda de la tarifa.
    /// </summary>
    public Moneda Moneda { get; set; } = default!;

    /// <summary>
    /// El servicio de la tarifa.
    /// </summary>
    public ServicioInfo Servicio { get; set; } = default!;

    /// <summary>
    /// El tramo de la tarifa.
    /// </summary>
    public TramoInfo Tramo { get; set; } = default!;

    /// <summary>
    /// El grupo etario de la tarifa.
    /// </summary>
    public GrupoEtarioInfo GrupoEtario { get; set; } = default!;

    /// <summary>
    /// El tipo de día de la tarifa.
    /// </summary>
    public TipoDia TipoDia { get; set; } = default!;

    /// <summary>
    /// El tipo de horario de la tarifa.
    /// </summary>
    public TipoHorario TipoHorario { get; set; } = default!;

    /// <summary>
    /// El canal de venta de la tarifa.
    /// </summary>
    public CanalVenta CanalVenta { get; set; } = default!;

    /// <summary>
    /// El tipo de segmentación de la tarifa.
    /// </summary>
    public TipoSegmentacion TipoSegmentacion { get; set; } = default!;

    /// <summary>
    /// El valor afecto.
    /// </summary>
    public decimal ValorAfecto { get; set; }

    /// <summary>
    /// El valor exento.
    /// </summary>
    public decimal ValorExento { get; set; }

    /// <summary>
    /// El valor de la tarifa.
    /// </summary>
    public decimal Valor { get; set; }
}
