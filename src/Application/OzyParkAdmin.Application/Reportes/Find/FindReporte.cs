using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Reportes;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Reportes.Find;

/// <summary>
/// Busca un reporte dado su aka.
/// </summary>
/// <param name="User">El usuario que realiza la consulta.</param>
/// <param name="Aka">El aka del reporte a buscar.</param>
public sealed record FindReporte(ClaimsPrincipal User, string Aka) : IQuery<Report>;
