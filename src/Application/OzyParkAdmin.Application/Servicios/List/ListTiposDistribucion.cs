using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// Lista todos los tipos de distribución.
/// </summary>
public sealed record ListTiposDistribucion : Request<ResultListOf<TipoDistribucion>>;
