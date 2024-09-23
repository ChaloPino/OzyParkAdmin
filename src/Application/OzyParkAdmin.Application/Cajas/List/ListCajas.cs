using MassTransit.Mediator;
using OzyParkAdmin.Domain.Cajas;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Cajas.List;

/// <summary>
/// Lista todas las cajas.
/// </summary>
/// <param name="User">El usuario que realiza la consulta.</param>
public sealed record ListCajas(ClaimsPrincipal User) : Request<ResultListOf<CajaInfo>>;
