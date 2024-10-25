using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Reportes.Charts;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Reportes.Find;

/// <summary>
/// Busca el último dashboard que esté publicado.
/// </summary>
/// <param name="User">El usuario que realiza la consulta.</param>
public sealed record FindDashboard(ClaimsPrincipal User) : IQuery<ChartReport>;
