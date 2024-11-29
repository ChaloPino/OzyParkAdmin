using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.TarifasServicio;
using OzyParkAdmin.Domain.Tramos;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.TarfiasServicio.Create;

/// <summary>
/// Crea varias tarifas de servicio.
/// </summary>
/// <param name="InicioVigencia">El inicio de vigencia de la tarifa.</param>
/// <param name="Moneda">La moneda de la tarifa.</param>
/// <param name="Servicio">El servicio de la tarifa.</param>
/// <param name="Tramos">Lista de tramos del servicio.</param>
/// <param name="GruposEtarios">Lista de grupos etarios del servicio.</param>
/// <param name="CanalesVenta">Lista de canales de venta.</param>
/// <param name="TiposDia">Lista de tipos de día.</param>
/// <param name="TiposHorario">Lista de tipos de horario.</param>
/// <param name="TiposSegmentacion">Lista de tipos de segmentación.</param>
/// <param name="ValorAfecto">El valor afecto de la tarifa.</param>
/// <param name="ValorExento">El valor exento de la tarifa.</param>
public sealed record CreateTarifasServicio(
    DateTime InicioVigencia,
    Moneda Moneda,
    ServicioInfo Servicio,
    ImmutableArray<TramoInfo> Tramos,
    ImmutableArray<GrupoEtarioInfo> GruposEtarios,
    ImmutableArray<CanalVenta> CanalesVenta,
    ImmutableArray<TipoDia> TiposDia,
    ImmutableArray<TipoHorario> TiposHorario,
    ImmutableArray<TipoSegmentacion> TiposSegmentacion,
    decimal ValorAfecto,
    decimal ValorExento) : ICommand;
