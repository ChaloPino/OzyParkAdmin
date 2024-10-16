namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// Representa una acción asociada a un reporte.
/// </summary>
public sealed class ReportAction
{
    private ReportAction()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ReportAction"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertenece la acción.</param>
    /// <param name="type">El tipo de acción del reporte.</param>
    public ReportAction(Report report, ActionType type)
    {
        ReportId = report.Id;
        Type = type;
    }

    /// <summary>
    /// El id del reporte.
    /// </summary>
    public Guid ReportId { get; private set; }

    /// <summary>
    /// El tipo de acción.
    /// </summary>
    public ActionType Type { get; private set; }
}
