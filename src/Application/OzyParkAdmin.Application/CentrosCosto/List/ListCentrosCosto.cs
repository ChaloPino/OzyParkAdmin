using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CentrosCosto;
using System.Security.Claims;

namespace OzyParkAdmin.Application.CentrosCosto.List;

/// <summary>
/// Lista todos los centros de costo activo.
/// </summary>
/// <param name="User">El usuario que realiza la consulta.</param>
public sealed record ListCentrosCosto(ClaimsPrincipal User) : IQueryListOf<CentroCostoInfo>;
