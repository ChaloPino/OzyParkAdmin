namespace OzyParkAdmin.Domain.Reportes.MasterDetails;

/// <summary>
/// Un reporte de tipo maestro detalle.
/// </summary>
public sealed class MasterDetailReport : Report
{
    private readonly List<ReportDetail> _details = [];

    private MasterDetailReport()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="MasterDetailReport"/>.
    /// </summary>
    /// <param name="aka">El aka del reporte.</param>
    public MasterDetailReport(string aka)
        : base(ReportType.MasterDetail, aka)
    {
    }

    /// <summary>
    /// Si tiene detalles.
    /// </summary>
    public bool HasDetail { get; private set; }

    /// <summary>
    /// El índice para conseguir la información del result set.
    /// </summary>
    public int? MasterResultIndex { get; private set; }

    /// <summary>
    /// Si es tabular.
    /// </summary>
    public bool IsTabular { get; private set; }

    /// <summary>
    /// El nombre del reporte.
    /// </summary>
    public string? TitleInReport { get; private set; }

    /// <summary>
    /// Tiene detalles dinámicos.
    /// </summary>
    public bool HasDynamicDetails { get; private set; }

    /// <summary>
    /// Los detalles del reporte.
    /// </summary>
    public IEnumerable<ReportDetail> Details => _details;
}
