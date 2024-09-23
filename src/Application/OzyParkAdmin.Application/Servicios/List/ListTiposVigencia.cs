using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// Lista todos los tipos de vigencia.
/// </summary>
public sealed record ListTiposVigencia : Request<ResultListOf<TipoVigencia>>;
