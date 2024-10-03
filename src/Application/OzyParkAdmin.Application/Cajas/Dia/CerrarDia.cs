using MassTransit.Mediator;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Cajas.Dia;

/// <summary>
/// Cierra el día de una caja.
/// </summary>
/// <param name="DiaId">El id del día de apertura de la caja.</param>
/// <param name="User">El usuario que está cerrando el día.</param>
/// <param name="Comentario">El comentario al cierre del día.</param>
/// <param name="MontoEfectivoParaCierre">El monto efectivo para el cierre.</param>
/// <param name="MontoTransbankParaCierre">El monto de tarjetas de crédito y débito para el cierre.</param>
public sealed record CerrarDia(Guid DiaId, ClaimsPrincipal User, string Comentario, decimal MontoEfectivoParaCierre, decimal MontoTransbankParaCierre) : Request<ResultOf<AperturaCajaInfo>>;
