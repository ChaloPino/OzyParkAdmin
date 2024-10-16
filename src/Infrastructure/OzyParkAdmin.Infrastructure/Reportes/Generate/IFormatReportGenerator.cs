using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;
using System.Security.Claims;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate;

/// <summary>
/// Generador de reportes en un formato determinado.
/// </summary>
public interface IFormatReportGenerator
{
    /// <summary>
    /// Genera el reporte en el formato solicitado.
    /// </summary>
    /// <param name="report">El reporte a generar.</param>
    /// <param name="filter">La información de filtrado.</param>
    /// <param name="user">El usuario que solicita la generación.</param>
    /// <returns>El resultado de la generación del reporte en formato solicitado.</returns>
    IFormattedReport GenerateReport(Report report, ReportFilter filter, ClaimsPrincipal user);
}