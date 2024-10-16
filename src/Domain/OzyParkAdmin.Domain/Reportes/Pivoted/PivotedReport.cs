namespace OzyParkAdmin.Domain.Reportes.Pivoted;

/// <summary>
/// Una reporte pivoteado.
/// </summary>
public sealed class PivotedReport : Report
{
    private readonly List<PivotedMember> _pivotedMembers = [];

    private PivotedReport()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="PivotedReport"/>.
    /// </summary>
    /// <param name="aka">El aka del reporte.</param>
    public PivotedReport(string aka)
        : base(ReportType.Pivoted, aka)
    {
    }

    /// <summary>
    /// Si incluye los totales por fila.
    /// </summary>
    public bool IncludeTotalsRow { get; private set; }

    /// <summary>
    /// Si incluye los totales por columna.
    /// </summary>
    public bool IncludeTotalsColumn { get; private set; }

    /// <summary>
    /// Si incluye el gran total.
    /// </summary>
    public bool IncludeGrandTotal { get; private set; }

    /// <summary>
    /// Los miembros de pivoteo del reporte.
    /// </summary>
    public IEnumerable<PivotedMember> PivotedMembers => _pivotedMembers;
}