using MassTransit.Mediator;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cajas.Dia;

/// <summary>
/// Reapertura el día de una caja.
/// </summary>
/// <param name="DiaId">El id de la caja a reaperturar.</param>
public sealed record ReabrirDia(Guid DiaId) : Request<ResultOf<AperturaCajaInfo>>;
