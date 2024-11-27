using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.TarifasServicio;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Application.TarfiasServicio.Update;

/// <summary>
/// Actualiza una tarifa de servicio.
/// </summary>
/// <param name="InicioVigencia">El inicio de vigencia de la tarifa a actualizar.</param>
/// <param name="Moneda">La momeda de la tarifa a actualizar.</param>
/// <param name="Servicio">El servicio de la tarifa a actualizar.</param>
/// <param name="Tramo">El tramo de la tarifa a actualizar.</param>
/// <param name="GrupoEtario">El grupo etario de la tarifa a actualizar.</param>
/// <param name="CanalVenta">El canal de venta de la tarifa a actualizar.</param>
/// <param name="TipoDia">El tipo de día de la tarifa a actualizar.</param>
/// <param name="TipoHorario">El tipo de horario de la tarifa a actualizar.</param>
/// <param name="TipoSegmentacion">El tipo de segmentación de la tarifa a actualizar.</param>
/// <param name="ValorAfecto">El valor afecto de la tarifa.</param>
/// <param name="ValorExento">El valor exento de la tarifa.</param>
public sealed record UpdateTarifaServicio(
    DateTime InicioVigencia,
    Moneda Moneda,
    ServicioInfo Servicio,
    TramoInfo Tramo,
    GrupoEtarioInfo GrupoEtario,
    CanalVenta CanalVenta,
    TipoDia TipoDia,
    TipoHorario TipoHorario,
    TipoSegmentacion TipoSegmentacion,
    decimal ValorAfecto,
    decimal ValorExento) : ICommand<TarifaServicioFullInfo>;
