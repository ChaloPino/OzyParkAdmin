using MassTransit.Mediator;
using OzyParkAdmin.Domain.Franquicias;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Franquicias.List;

/// <summary>
/// Lista todas las franquicias activas.
/// </summary>
/// <param name="User">El usuario que realiza la consulta para restringir el listado.</param>
public sealed record ListFranquicias(ClaimsPrincipal User) : Request<ResultListOf<Franquicia>>;
