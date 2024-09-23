using MassTransit.Mediator;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Application.Tramos.List;

/// <summary>
/// Lista todos los tramos.
/// </summary>
public sealed record ListTramos : Request<ResultListOf<TramoInfo>>;
