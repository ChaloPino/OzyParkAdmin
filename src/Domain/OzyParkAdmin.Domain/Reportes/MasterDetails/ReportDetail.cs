using OzyParkAdmin.Domain.Reportes.DataSources;

namespace OzyParkAdmin.Domain.Reportes.MasterDetails;

/// <summary>
/// Representa el detalle de un reporte tipo maestro detalle.
/// </summary>
public sealed class ReportDetail
{
    private readonly List<DetailColumn> _detailColumns = [];

    private ReportDetail()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ReportDetail"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertence el detalle.</param>
    /// <param name="id">El identificador del detalle.</param>
    public ReportDetail(MasterDetailReport report, int id)
    {
        ReportId = report.Id;
        DetailId = id;
    }

    /// <summary>
    /// El id del reporte.
    /// </summary>
    public Guid ReportId { get; private set; }

    /// <summary>
    /// El id del detalle.
    /// </summary>
    public int DetailId { get; private set; }

    /// <summary>
    /// El índice del result set de la fuente de datos para conseguir la información del detalle.
    /// </summary>
    public int? DetailResultSetIndex { get; private set; }

    /// <summary>
    /// Si el detalle es tabular.
    /// </summary>
    public bool IsTabular { get; private set; }

    /// <summary>
    /// El título del detalle.
    /// </summary>
    public string? Title { get; private set; }

    /// <summary>
    /// La fuente de datos del detalle si es separada del maestro.
    /// </summary>
    public DataSource? DetailDataSource { get; private set; }

    /// <summary>
    /// La lista de columnas del detalle.
    /// </summary>
    public IEnumerable<DetailColumn> DetailColumns => _detailColumns;
}
