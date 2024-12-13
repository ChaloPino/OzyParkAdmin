using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasProductos.Models;
/// <summary>
/// El modelo para crear varias tarifas de servicio.
/// </summary>
public sealed class TarifaProductoCreateModel
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
    public ProductoInfo Producto { get; set; } = default!;

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

    internal CanalVenta? ValidationCanalesVenta => CanalesVenta.FirstOrDefault();

    internal TipoDia? ValidationTiposDia => TiposDia.FirstOrDefault();

    internal TipoHorario? ValidationTiposHorario => TiposHorario.FirstOrDefault();

    internal DateTime InicioVigenciaFinal => InicioVigencia!.Value.Add(HoraVigencia!.Value);
}
