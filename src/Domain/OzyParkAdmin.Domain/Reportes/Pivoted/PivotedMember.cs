using OzyParkAdmin.Domain.Reportes.Excel;
using OzyParkAdmin.Domain.Reportes.Listed;
using System.Data;

namespace OzyParkAdmin.Domain.Reportes.Pivoted;

/// <summary>
/// Representa un miembro del reporte pivoteado.
/// </summary>
public class PivotedMember : SecureComponent<PivotedMember>, IExcelColumnFormattable, IConditionable
{
    private ExcelFormat? _currentExcelFormat;

    private PivotedMember()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="PivotedMember"/>.
    /// </summary>
    /// <param name="report">El reporte pivoteado al que pertenece este miembro.</param>
    /// <param name="column">La columna asociada a este miembro.</param>
    /// <param name="id">El identificador de este miembro.</param>
    public PivotedMember(PivotedReport report, Column column, int id)
    {
        ReportId = report.Id;
        Column = column;
        ColumnId = column.Id;
        Id = id;
    }

    /// <summary>
    /// El id del reporte.
    /// </summary>
    public Guid ReportId { get; private set; }

    /// <summary>
    /// El id del miembro.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// La cabecera del miembro.
    /// </summary>
    public string? Header { get; private set; }

    /// <summary>
    /// El id de la columna asociada.
    /// </summary>
    public int ColumnId { get; private set; }

    /// <summary>
    /// La columna asociada.
    /// </summary>
    public Column Column { get; private set; } = default!;

    /// <summary>
    /// El tipo de pivotero.
    /// </summary>
    public PivotType PivotType { get; private set; }

    /// <summary>
    /// El tipo de agregación.
    /// </summary>
    public AggregationType? AggregationType { get; private set; }

    /// <summary>
    /// Si se muestra el total.
    /// </summary>
    public bool? ShowTotal { get; private set; }

    /// <summary>
    /// El orden de despliegue del miembro.
    /// </summary>
    public int Order { get; private set; }

    /// <summary>
    /// El nombre de la propiedad si el valor es un objeto complejo como una fecha.
    /// </summary>
    public string? Property { get; private set; }

    /// <summary>
    /// El nombre a desplegar de la propiedad.
    /// </summary>
    public string? PropertyDisplay { get; private set; }

    /// <summary>
    /// El formato que tendrá este miembro cuando se genere el reporte en html y en pdf.
    /// </summary>
    public string? Format { get; private set; }

    /// <summary>
    /// La dirección de ordenamiento de los valores del miembro.
    /// </summary>
    public PivotSortDirection? SortDirection { get; set; }

    /// <summary>
    /// El id de la columna de ordenamiento.
    /// </summary>
    public int? SortColumnId { get; private set; }

    /// <summary>
    /// La columna de ordenamiento.
    /// </summary>
    public Column? SortColumn { get; private set; }

    /// <summary>
    /// La lista de valores personalizados de ordenamiento.
    /// </summary>
    public string? CustomSortList { get; private set; }

    /// <summary>
    /// El formato Excel del miembro.
    /// </summary>
    public ExcelFormat? ExcelFormat { get; private set; }

    /// <summary>
    /// El id del formato Excel personalizado del miembro.
    /// </summary>
    public int? CustomExcelFormatId { get; private set; }

    /// <summary>
    /// El formato Excel personalizado del miembro.
    /// </summary>
    public string? CustomExcelFormat { get; private set; }

    /// <inheritdoc/>
    public bool HasConditionalStyle { get; private set; }

    /// <inheritdoc/>
    public ConditionalStyle? SuccessStyle { get; private set; }

    /// <inheritdoc/>
    public OperatorStyleType? SuccessOperator { get; private set; }

    /// <inheritdoc/>
    public string? SuccessConditionalValue { get; private set; }

    /// <inheritdoc/>
    public string? SuccessAlternateConditionalValue { get; private set; }

    /// <inheritdoc/>
    public ConditionalStyle? WarningStyle { get; private set; }

    /// <inheritdoc/>
    public OperatorStyleType? WarningOperator { get; private set; }

    /// <inheritdoc/>
    public string? WarningConditionalValue { get; private set; }

    /// <inheritdoc/>
    public string? WarningAlternateConditionalValue { get; private set; }

    /// <inheritdoc/>
    public ConditionalStyle? ErrorStyle { get; private set; }

    string IExcelColumnFormattable.Name => Property ?? Column.Name;
    string? IExcelColumnFormattable.Header => PropertyDisplay ?? Property ?? Header;

    DbType IExcelColumnFormattable.Type => Column.Type;

    ExcelFormat? IExcelColumnFormattable.CurrentExcelFormat
    {
        get
        {
            _currentExcelFormat ??= ExcelFormat ?? (CustomExcelFormatId.HasValue && !string.IsNullOrEmpty(CustomExcelFormat)
                        ? new ExcelFormat(CustomExcelFormatId.Value, CustomExcelFormat)
                        : Column.CurrentExcelFormat);

            return _currentExcelFormat;
        }
    }

    bool IExcelColumnFormattable.IsStringAndGuidType() => Column.IsStringAndGuidType();
}
