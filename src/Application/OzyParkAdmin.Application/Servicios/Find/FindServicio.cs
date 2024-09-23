using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Find;

/// <summary>
/// Busca un servicio dado el id de servicio.
/// </summary>
/// <param name="ServicioId">El id del servicio a buscar.</param>
public sealed record FindServicio(int ServicioId) : Request<ResultOf<ServicioFullInfo>>;
