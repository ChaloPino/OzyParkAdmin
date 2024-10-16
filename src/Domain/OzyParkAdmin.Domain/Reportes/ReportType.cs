namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// Los tipos de reportes.
/// </summary>
public enum ReportType
{
    /// <summary>
    /// Reporte tipo listado.
    /// </summary>
    Listed = 1,

    /// <summary>
    /// Reporte tipo paginado.
    /// </summary>
    Paginated = 2,

    /// <summary>
    /// Reporte pivoteado.
    /// </summary>
    Pivoted = 3,

    /// <summary>
    /// Reporte maestro-detalle.
    /// </summary>
    MasterDetail = 4,

    /// <summary>
    /// Reporte tipo dashboards
    /// </summary>
    Chart = 5,
}
