using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Domain.TarifasProducto;
public sealed class TarifaProducto
{
    /// <summary>
    /// El inicio de vigencia de la tarifa.
    /// </summary>
    public DateTime InicioVigencia { get; private set; }

    /// <summary>
    /// La moneda de la tarifa.
    /// </summary>
    public Moneda Moneda { get; private set; } = default!;
    public int MonedaId { get; set; }
    public int ProductoId { get; set; }
    public int TipoDiaId { get; set; }
    public int TipoHorarioId { get; set; }

    public int CanalVentaId { get; set; }

    /// <summary>
    /// El producto de la tarifa.
    /// </summary>
    public Producto Producto { get; private set; } = default!;

    /// <summary>
    /// El tipo de día de la tarifa.
    /// </summary>
    public TipoDia TipoDia { get; private set; } = default!;

    /// <summary>
    /// El tipo de horario de la tarifa.
    /// </summary>
    public TipoHorario TipoHorario { get; private set; } = default!;

    /// <summary>
    /// El canal de venta de la tarifa.
    /// </summary>
    public CanalVenta CanalVenta { get; private set; } = default!;

    /// <summary>
    /// El valor afecto.
    /// </summary>
    public decimal ValorAfecto { get; private set; }

    /// <summary>
    /// El valor exento.
    /// </summary>
    public decimal ValorExento { get; private set; }

    /// <summary>
    /// El valor de la tarifa.
    /// </summary>
    public decimal Valor { get; private set; }

    internal static TarifaProducto Create(
        DateTime inicioVigencia,
        Moneda moneda,
        Producto producto,
        CanalVenta canalVenta,
        TipoDia tipoDia,
        TipoHorario tipoHorario,
        decimal valorAfecto,
        decimal valorExento) =>
        new()
        {
            InicioVigencia = inicioVigencia,
            Moneda = moneda,
            Producto = producto,
            CanalVenta = canalVenta,
            TipoDia = tipoDia,
            TipoHorario = tipoHorario,
            ValorAfecto = valorAfecto,
            ValorExento = valorExento,
            Valor = valorExento + valorAfecto,
        };

    internal void Update(decimal valorAfecto, decimal valorExento)
    {
        ValorAfecto = valorAfecto;
        ValorExento = valorExento;
        Valor = valorAfecto + valorExento;
    }
}
