namespace OzyParkAdmin.Domain.Reportes.Excel;

/// <summary>
/// La plantilla que tendrá un reporte generado para Excel.
/// </summary>
public sealed class ExcelReportTemplate : ReportTemplate
{
    private ExcelReportTemplate()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ExcelReportTemplate"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertenece la plantilla.</param>
    public ExcelReportTemplate(Report report)
        : base(report, ActionType.Excel)
    {
    }

    /// <summary>
    /// Si tiene cabecera.
    /// </summary>
    public bool HasHeader { get; private set; }

    /// <summary>
    /// El título de la cabecera.
    /// </summary>
    public string? HeaderTitle { get; private set; }

    public ExcelReportTemplate ChangeHasHeader(bool hasHeader)
    {
        HasHeader = hasHeader;
        return this;
    }

    public ExcelReportTemplate ChangeHeaderTitle(string? headerTitle)
    {
        HeaderTitle = string.IsNullOrWhiteSpace(headerTitle) ? null : headerTitle.Trim();
        return this;
    }
}
