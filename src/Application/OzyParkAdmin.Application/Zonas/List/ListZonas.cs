using MassTransit.Mediator;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Application.Zonas.List;

/// <summary>
/// Lista todas las zonas.
/// </summary>
public sealed record ListZonas : Request<ResultListOf<ZonaInfo>>;
