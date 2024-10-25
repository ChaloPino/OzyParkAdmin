using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Reportes;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Reportes.Generate;

/// <summary>
/// Genera el reporte en otro formato que puede ser descargado.
/// </summary>
/// <param name="Aka">El aka del reporte a generar.</param>
/// <param name="Format">El formato que se quiere genera.</param>
/// <param name="Filter">Los filtros usados para generar el reporte.</param>
/// <param name="User">El usuario que solicita la generación del reporte.</param>
public sealed record GenerateOtherFormat(string Aka, ActionType Format, ReportFilter Filter, ClaimsPrincipal User): IQuery<ReportGenerated>;
