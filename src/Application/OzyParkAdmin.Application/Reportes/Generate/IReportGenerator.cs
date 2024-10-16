using OzyParkAdmin.Domain.Reportes;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Reportes.Generate;

/// <summary>
/// Contiene métodos para generar un reporte.
/// </summary>
public interface IReportGenerator
{
    /// <summary>
    /// Genera el reporte para html.
    /// </summary>
    /// <param name="aka">El aka del reporte a generar.</param>
    /// <param name="filter">La información de filtrado para generar el reporte.</param>
    /// <param name="user">El usuario que solicita la generación del reporte.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de la generación del reporte.</returns>
    Task<ReportResult> GenerateHtmlReportAsync(string aka, ReportFilter filter, ClaimsPrincipal user, CancellationToken cancellationToken);

    /// <summary>
    /// Genera el reporte para un formato de exportación.
    /// </summary>
    /// <param name="aka">El aka del reporte a generar.</param>
    /// <param name="format">El formato de exportación.</param>
    /// <param name="filter">La información de filtrado para generar el reporte.</param>
    /// <param name="user">El usuario que solicita la generación del reporte.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de la generación del reporte en el formato <paramref name="format"/>.</returns>
    Task<ReportResult> GenerateOhterFormatReportAsync(string aka, ActionType format, ReportFilter filter, ClaimsPrincipal user, CancellationToken cancellationToken);
}