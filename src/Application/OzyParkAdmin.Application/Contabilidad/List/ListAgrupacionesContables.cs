using MassTransit.Mediator;
using OzyParkAdmin.Domain.Contabilidad;

namespace OzyParkAdmin.Application.Contabilidad.List;

/// <summary>
/// Lista todas las agrupaciones contables.
/// </summary>
public sealed record ListAgrupacionesContables : Request<ResultListOf<AgrupacionContable>>;
