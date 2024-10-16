using NReco.PivotData;
using OzyParkAdmin.Application.Reportes.Charts;
using OzyParkAdmin.Application.Reportes.MasterDetails;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Charts;
using System.Diagnostics.CodeAnalysis;

namespace OzyParkAdmin.Application.Reportes.Generate;

/// <summary>
/// La información del reporte generado.
/// </summary>
public sealed class ReportGenerated
{
    /// <summary>
    /// El tipo del reporte.
    /// </summary>
    public ReportType Type { get; set; }

    /// <summary>
    /// El formato generado.
    /// </summary>
    public ActionType Format { get; set; }

    /// <summary>
    /// Si el reporte está generado para html.
    /// </summary>
    public bool IsHtml => Format == ActionType.Html;

    /// <summary>
    /// Si el reporte está generado para Excel.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Content))]
    public bool IsExcel => Format == ActionType.Excel;

    /// <summary>
    /// Si el reporte está generado para Pdf.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Content))]
    public bool IsPdf => Format == ActionType.Pdf;

    /// <summary>
    /// Si es un reporte tipo lista.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Columns), nameof(Data))]
    public bool IsListed => Type == ReportType.Listed;

    /// <summary>
    /// Si es un reporte tipo paginado.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Columns), nameof(Data))]
    public bool IsPaginated => Type == ReportType.Paginated;

    /// <summary>
    /// Si es un reporte tipo maestro - detalle.
    /// </summary>
    [MemberNotNullWhen(true, nameof(MasterTable), nameof(Details))]
    public bool IsMasterDetail => Type == ReportType.MasterDetail;

    /// <summary>
    /// Si es un reporte tipo pivote.
    /// </summary>
    [MemberNotNullWhen(true, nameof(PivotTable))]
    public bool IsPivoted => Type == ReportType.Pivoted;

    /// <summary>
    /// Si es un reporte tipo pivote.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Charts))]
    public bool IsChart => Type == ReportType.Chart;

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

    /// <summary>
    /// El contenido del reporte generado.
    /// Para todos los reportes con formato Html y Pdf.
    /// </summary>
    public byte[]? Content { get; set; }
}
