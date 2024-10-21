using MassTransit.Mediator;
using OzyParkAdmin.Domain.Reportes.Charts;
using OzyParkAdmin.Domain.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Reportes.Find;

/// <summary>
/// Busca el último dashboard que esté publicado.
/// </summary>
/// <param name="User">El usuario que realiza la consulta.</param>
public sealed record FindDashboard(ClaimsPrincipal User) : Request<ResultOf<ChartReport>>;
