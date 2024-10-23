using MassTransit.Mediator;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.OmisionesCupo.Create;

/// <summary>
/// Crea varias omisiones de las exclusiones de escenarios de cupo.
/// </summary>
/// <param name="EscenariosCupo">Los escenarios de cupo usados para crear las omisiones.</param>
/// <param name="CanalesVenta">Los canales de venta usados para crear las omisiones.</param>
/// <param name="FechaDesde">La fecha desde para crear la omisiones.</param>
/// <param name="FechaHasta">La fecha hasta para crear las omisiones.</param>
public sealed record CreateOmisionesEscenarioCupoExclusion(
    ImmutableArray<EscenarioCupoInfo> EscenariosCupo,
    ImmutableArray<CanalVenta> CanalesVenta,
    DateOnly FechaDesde,
    DateOnly FechaHasta) : Request<SuccessOrFailure>;
