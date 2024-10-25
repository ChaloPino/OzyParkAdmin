using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Reportes;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Reportes.List;

/// <summary>
/// Lista todos los reportes agrupados.
/// </summary>
/// <param name="User">El usuario que realiza la consulta.</param>
public sealed record class ListReportes(ClaimsPrincipal User) : IQueryListOf<ReportGroupInfo>;
