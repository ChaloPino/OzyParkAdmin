using MassTransit.Mediator;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Application.Cupos.Search;

/// <summary>
/// Busca los cupos para un calendario.
/// </summary>
/// <param name="CanalVenta">El canal de venta a buscar.</param>
/// <param name="Alcance">El alcance del cupo.</param>
/// <param name="Servicio">El servcio a buscar.</param>
/// <param name="ZonaOrigen">La zona de origen a buscar.</param>
/// <param name="Inicio">La fecha de inicio.</param>
/// <param name="Fin">La fecha de fin.</param>
public sealed record SearchCalendario(
    CanalVenta CanalVenta,
    CupoAlcance Alcance,
    ServicioInfo Servicio,
    ZonaInfo? ZonaOrigen,
    DateTime? Inicio,
    DateTime? Fin) : Request<ResultListOf<CupoFechaInfo>>;
