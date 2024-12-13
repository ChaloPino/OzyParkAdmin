using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.TarifasProducto;

namespace OzyParkAdmin.Application.TarfiasProducto.Update;

/// <summary>
/// Actualiza una tarifa de Producto.
/// </summary>
/// <param name="InicioVigencia">El inicio de vigencia de la tarifa a actualizar.</param>
/// <param name="Moneda">La momeda de la tarifa a actualizar.</param>
/// <param name="Producto">El Producto de la tarifa a actualizar.</param>
/// <param name="CanalVenta">El canal de venta de la tarifa a actualizar.</param>
/// <param name="TipoDia">El tipo de día de la tarifa a actualizar.</param>
/// <param name="TipoHorario">El tipo de horario de la tarifa a actualizar.</param>
/// <param name="ValorAfecto">El valor afecto de la tarifa.</param>
/// <param name="ValorExento">El valor exento de la tarifa.</param>
public sealed record UpdateTarifaProducto(
    DateTime InicioVigencia,
    Moneda Moneda,
    ProductoInfo Producto,
    CanalVenta CanalVenta,
    TipoDia TipoDia,
    TipoHorario TipoHorario,
    decimal ValorAfecto,
    decimal ValorExento) : ICommand<TarifaProductoFullInfo>;
