using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios;

/// <summary>
/// Request para cualquier cambio de estado del servicio.
/// </summary>
public interface IServicioStateChangeable : Request<ResultOf<ServicioFullInfo>>;
