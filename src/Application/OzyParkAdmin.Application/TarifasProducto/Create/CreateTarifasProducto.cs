using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Productos;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.TarfiasProducto.Create;

/// <summary>
/// Crea varias tarifas de Producto.
/// </summary>
/// <param name="InicioVigencia">El inicio de vigencia de la tarifa.</param>
/// <param name="Moneda">La moneda de la tarifa.</param>
/// <param name="Producto">El Producto de la tarifa.</param>
/// <param name="CanalesVenta">Lista de canales de venta.</param>
/// <param name="TiposDia">Lista de tipos de día.</param>
/// <param name="TiposHorario">Lista de tipos de horario.</param>
/// <param name="ValorAfecto">El valor afecto de la tarifa.</param>
/// <param name="ValorExento">El valor exento de la tarifa.</param>
public sealed record CreateTarifasProducto(
    DateTime InicioVigencia,
    Moneda Moneda,
    ProductoInfo Producto,
    ImmutableArray<CanalVenta> CanalesVenta,
    ImmutableArray<TipoDia> TiposDia,
    ImmutableArray<TipoHorario> TiposHorario,
    decimal ValorAfecto,
    decimal ValorExento) : ICommand;
