using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Domain.Reportes.Pdf;

/// <summary>
/// La plantilla usada para cuando el reporte se exporte a Pdf.
/// </summary>
public sealed class PdfReportTemplate : ReportTemplate
{
    private PdfReportTemplate()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="PdfReportTemplate"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertenece la plantilla.</param>
    public PdfReportTemplate(Report report)
        : base(report, ActionType.Pdf)
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

    /// <summary>
    /// La orientación del Pdf.
    /// </summary>
    public PdfOrientation Orientation { get; private set; }

    /// <summary>
    /// El tamaño de la fuente para el título.
    /// </summary>
    public float? TitleFontSize { get; private set; }

    /// <summary>
    /// El tamaño de la fuente para las etiquetas de los filtros.
    /// </summary>
    public float? FilterHeaderFontSize { get; private set; }

    /// <summary>
    /// El tamaño de la fuente para los valores de los filtros.
    /// </summary>
    public float? FilterFontSize { get; private set; }

    /// <summary>
    /// El tamaño de la fuente para los títulos de las tablas del reporte.
    /// </summary>
    public float? HeaderFontSize { get; private set; }

    /// <summary>
    /// El tamaño de la fuente para cada fila de las tablas del reporte.
    /// </summary>
    public float? RowFontSize { get; private set; }

    /// <summary>
    /// El tamaño de la fuente para el pie de las tablas del reporte.
    /// </summary>
    public float? FooterFontSize { get; private set; }

    /// <summary>
    /// Si se repite la cabecera en cada página.
    /// </summary>
    public bool RepeatHeaderInEachPage { get; private set; }

    /// <summary>
    /// Si se reporte el pie en cada página.
    /// </summary>
    public bool RepeatFooterInEachPage { get; private set; }

    /// <summary>
    /// El margen izquierdo.
    /// </summary>
    public Margin Margin { get; private set; } = Margin.Empty;
}
