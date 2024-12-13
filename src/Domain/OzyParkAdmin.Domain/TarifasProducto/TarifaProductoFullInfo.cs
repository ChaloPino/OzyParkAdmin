using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Domain.TarifasProducto;
public sealed class TarifaProductoFullInfo
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
    public ProductoInfo Producto { get; set; } = default!;

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
