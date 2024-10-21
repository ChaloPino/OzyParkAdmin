using MassTransit.Mediator;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha;

/// <summary>
/// Request para cualquier cambio de estado de un cupo fecha.
/// </summary>
public interface ICupoFechaStateChangeable : Request<ResultOf<CupoFechaFullInfo>>;
