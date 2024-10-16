using NReco.PivotData;
using OzyParkAdmin.Application.Reportes;
using OzyParkAdmin.Application.Reportes.Charts;
using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Application.Reportes.MasterDetails;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo de un reporte generado.
/// </summary>
public class ReportGeneratedModel
{
    /// <summary>
    /// Si se está cargando el reporte.
    /// </summary>
    public LoadingState Loading { get; set; } = LoadingState.None;
    /// <summary>
    /// Las columnas del reporte.
    /// Para reportes tipo lista y paginados.
    /// </summary>
    public IEnumerable<ColumnInfo> Columns { get; set; } = [];

    /// <summary>
    /// Los valores por columna del reporte.
    /// Para reportes tipo lista y paginados.
    /// </summary>
    public IEnumerable<DataInfo> Data { get; set; } = [];

    /// <summary>
    /// El total de registros.
    /// Para reportes tipo paginados.
    /// </summary>
    public long TotalRecords { get; set; }

    /// <summary>
    /// La tabla maestra.
    /// Para reportes tipo maestro detalle.
    /// </summary>
    public MasterTable? MasterTable { get; set; }

    /// <summary>
    /// Las tablas de detalle.
    /// Para reportes tipo maestro detalle.
    /// </summary>
    public IEnumerable<DetailTable> Details { get; set; } = [];

    /// <summary>
    /// La tabla pivote.
    /// Para reportes de tipo pivote.
    /// </summary>
    public IPivotTable? PivotTable { get; set; }

    /// <summary>
    /// Si se muestra los totales de las columnas.
    /// Para reportes de tipo pivote.
    /// </summary>
    public bool TotalsColum { get; set; }

    /// <summary>
    /// Si se muestra los totales de las filas.
    /// Para reportes de tipo pivote.
    /// </summary>
    public bool TotalsRow { get; set; }

    /// <summary>
    /// Si se muestra el gran total
    /// Para reportes de tipo pivote.
    /// </summary>
    public bool GrandTotal { get; set; }

    /// <summary>
    /// Si se muestra los subtotales de las columnas.
    /// Para reportes de tipo pivote.
    /// </summary>
    public bool SubtotalColumns { get; set; }

    /// <summary>
    /// Si se muestra los subtotales de las filas.
    /// Para reportes de tipo pivote.
    /// </summary>
    public bool SubtotalRows { get; set; }

    /// <summary>
    /// La lista de dimensiones que tendrán subtotales.
    /// </summary>
    public string[] SubtotalDimensions { get; set; } = [];

    /// <summary>
    /// La lista de gráficos y tablas.
    /// </summary>
    public IEnumerable<ChartMetaInfo> Charts { get; set; } = [];

    internal static ReportGeneratedModel Create(ReportGenerated reportGenerated) =>
        new()
        {
            Loading = LoadingState.Loaded,
            Columns = reportGenerated.Columns,
            Data = reportGenerated.Data,
            TotalRecords = reportGenerated.TotalRecords,
            MasterTable = reportGenerated.MasterTable,
            Details = reportGenerated.Details,
            PivotTable = reportGenerated.PivotTable,
            TotalsColum = reportGenerated.TotalsColum,
            TotalsRow = reportGenerated.TotalsRow, 
            GrandTotal = reportGenerated.GrandTotal,
            SubtotalColumns = reportGenerated.SubtotalColumns,
            SubtotalRows = reportGenerated.SubtotalRows,
            SubtotalDimensions = reportGenerated.SubtotalDimensions,
            Charts = reportGenerated.Charts,
        };
}
