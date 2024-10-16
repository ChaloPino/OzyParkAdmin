namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// La plantilla de un reporte.
/// </summary>
public abstract class ReportTemplate
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="ReportTemplate"/>.
    /// </summary>
    protected ReportTemplate()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ReportTemplate"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertenece la plantilla.</param>
    /// <param name="type">El tipo de acción de la plantilla.</param>
    protected ReportTemplate(Report report, ActionType type)
    {
        ReportId = report.Id;
        Type = type;
    }

    /// <summary>
    /// El id del reporte.
    /// </summary>
    public Guid ReportId { get; private set; }

    /// <summary>
    /// El tipo de acción de la plantilla.
    /// </summary>
    public ActionType Type { get; private set; }

    /// <summary>
    /// El patrón para crear el nombre del archivo a exportar.
    /// </summary>
    public string? FileNamePattern { get; private set; }
}
