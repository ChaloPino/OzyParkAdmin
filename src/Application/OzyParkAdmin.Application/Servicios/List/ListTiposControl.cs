using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// Lista todos los tipos de control.
/// </summary>
public sealed record ListTiposControl : Request<ResultListOf<TipoControl>>;
