using MassTransit.Mediator;
using OzyParkAdmin.Domain.EscenariosCupo;
using System.Security.Claims;

namespace OzyParkAdmin.Application.EscenariosCupo.List;

/// <summary>
/// Lista todos los escenarios de cupo.
/// </summary>
/// <param name="User">El usuario que realiza la consulta.</param>
public sealed record ListEscenariosCupo(ClaimsPrincipal User) : Request<ResultListOf<EscenarioCupoInfo>>;
