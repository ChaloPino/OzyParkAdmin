using MassTransit.Mediator;
using OzyParkAdmin.Domain.OmisionesCupo;
using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.OmisionesCupo.Delete;

/// <summary>
/// Elimina varias omisiones de exclusiones de escenarios de cupo.
/// </summary>
/// <param name="Omisiones">La omisiones de exlusiones de fecha a eliminar.</param>
public sealed record DeleteOmisionesEscenarioCupoExclusion(ImmutableArray<IgnoraEscenarioCupoExclusionFullInfo> Omisiones) : Request<SuccessOrFailure>;
