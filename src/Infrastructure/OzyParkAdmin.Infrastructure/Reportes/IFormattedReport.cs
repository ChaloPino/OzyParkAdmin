using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes;

namespace OzyParkAdmin.Infrastructure.Reportes;

/// <summary>
/// El reporte formateado.
/// </summary>
public interface IFormattedReport
{
    /// <summary>
    /// El formato del reporte generado.
    /// </summary>
    public abstract ActionType Format { get; }

    /// <summary>
    /// El tipo del reporte.
    /// </summary>
    public abstract ReportType Type { get; }

    /// <summary>
    /// Genera el reporte.
    /// </summary>
    /// <returns>El reporte generado.</returns>
    ReportGenerated Generate();
}