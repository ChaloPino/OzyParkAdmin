using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio.Models;

/// <summary>
/// El modelo para crear varias tarifas de servicio.
/// </summary>
public sealed class TarifaServicioCreateModel
{
    /// <summary>
    /// El centro de costo usado para filtrar a los servicios.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; set; } = default!;
    /// <summary>
    /// El inicio de vigencia.
    /// </summary>
    public DateTime? InicioVigencia { get; set; } = DateTime.Today;

    /// <summary>
    /// La hora de vigencia.
    /// </summary>
    public TimeSpan? HoraVigencia { get; set; } = DateTime.Now.TimeOfDay;

    /// <summary>
    /// La moneda de la tarifa.
    /// </summary>
    public Moneda Moneda { get; set; } = default!;

    /// <summary>
    /// El servicio asociado.
    /// </summary>
    public ServicioWithDetailInfo Servicio { get; set; } = default!;

    /// <summary>
    /// Los tramos que se pueden asociar.
    /// </summary>
    public IEnumerable<TramoInfo> Tramos { get; set; } = [];

    /// <summary>
    /// Los grupos etarios a asociar.
    /// </summary>
    public IEnumerable<GrupoEtarioInfo> GruposEtarios { get; set; } = [];

    /// <summary>
    /// Los tipos de día.
    /// </summary>
    public IEnumerable<TipoDia> TiposDia { get; set; } = [];

    /// <summary>
    /// Los tipos de horario.
    /// </summary>
    public IEnumerable<TipoHorario> TiposHorario { get; set; } = [];

    /// <summary>
    /// Los canales de venta.
    /// </summary>
    public IEnumerable<CanalVenta> CanalesVenta { get; set; } = [];

    /// <summary>
    /// Los tipos de segmentación.
    /// </summary>
    public IEnumerable<TipoSegmentacion> TiposSegmentacion { get; set; } = [];

    /// <summary>
    /// El valor afecto de la tarifa.
    /// </summary>
    public decimal ValorAfecto { get; set; }

    /// <summary>
    /// El valor exento de la tarifa.
    /// </summary>
    public decimal ValorExento { get; set; }

    /// <summary>
    /// El valor de la tarifa.
    /// </summary>
    public decimal Valor => ValorAfecto + ValorExento;

    internal TramoInfo? ValidationTramos => Tramos.FirstOrDefault();

    internal GrupoEtarioInfo? ValidationGruposEtarios => GruposEtarios.FirstOrDefault();

    internal CanalVenta? ValidationCanalesVenta => CanalesVenta.FirstOrDefault();

    internal TipoDia? ValidationTiposDia => TiposDia.FirstOrDefault();

    internal TipoHorario? ValidationTiposHorario => TiposHorario.FirstOrDefault();

    internal TipoSegmentacion? ValidationTiposSegmentacion => TiposSegmentacion.FirstOrDefault();

    internal DateTime InicioVigenciaFinal => InicioVigencia!.Value.Add(HoraVigencia!.Value);
}
