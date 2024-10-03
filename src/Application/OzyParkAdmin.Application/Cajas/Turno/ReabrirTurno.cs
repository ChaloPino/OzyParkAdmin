using MassTransit.Mediator;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.Cajas.Turno;

/// <summary>
/// Reabre una turno de una caja.
/// </summary>
/// <param name="DiaId">El id de la apertura del día de la caja.</param>
/// <param name="TurnoId">El id del turno que se quiere reabrir.</param>
/// <param name="Movimientos">Los movimientos del turno.</param>
public sealed record ReabrirTurno(Guid DiaId, Guid TurnoId, ImmutableArray<DetalleTurnoInfo> Movimientos) : Request<ResultOf<TurnoCajaInfo>>;
