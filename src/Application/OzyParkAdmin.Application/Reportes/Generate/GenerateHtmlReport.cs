using OzyParkAdmin.Application.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Reportes.Generate;

/// <summary>
/// Genera un reporte para html.
/// </summary>
/// <param name="Aka">El aka del reporte a generar.</param>
/// <param name="Filter">Los filtros usados para generar el reporte.</param>
/// <param name="User">El usuario que solicita la generación del reporte.</param>
public sealed record GenerateHtmlReport(string Aka, ReportFilter Filter, ClaimsPrincipal User) : IQuery<ReportGenerated>;
