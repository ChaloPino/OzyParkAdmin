using OzyParkAdmin.Domain.Reportes.DataSources;
using OzyParkAdmin.Domain.Reportes.Excel;
using System.Globalization;

namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// El filtro de un reporte que se mostrará visualmente.
/// </summary>
public abstract class Filter : SecureComponent<Filter>
{
    private readonly List<ParentFilter> _parentFilters = [];
    private ExcelFormat? _currentExcelFormat;

    /// <summary>
    /// Crea una nueva instancia de <see cref="Filter"/>.
    /// </summary>
    protected Filter()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="Filter"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertence el filtro.</param>
    /// <param name="id">El identificador del filtro.</param>
    protected Filter(Report report, int id)
    {
        ReportId = report.Id;
        Id = id;
    }

    /// <summary>
    /// El id del reporte.
    /// </summary>
    public Guid ReportId { get; private set; }

    /// <summary>
    /// El id del filtro.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// El nombre del filtro.
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    /// La etiqueta del filtro.
    /// </summary>
    public string? Label { get; private set; }

    /// <summary>
    /// Si el filtro es requerido para las validaciones.
    /// </summary>
    public bool IsRequired { get; private set; }

    /// <summary>
    /// El orden de despliegue del filtro.
    /// </summary>
    public int Order { get; private set; }

    /// <summary>
    /// El mensaje de error cuando no sea válido el valor..
    /// </summary>
    public string? InvalidMessage { get; private set; }

    /// <summary>
    /// El formato que tendrá el filtro cuando se presente en reportes exportados a Pdf.
    /// </summary>
    public string? Format { get; private set; }

    /// <summary>
    /// El formato Excel que tendrá el filtro cuando se presente en reportes exportados a Excel.
    /// </summary>
    public ExcelFormat? ExcelFormat { get; private set; }

    /// <summary>
    /// El id del formato Excel personalizado que tendrá el filtro cuando se presente en reportes exportados a Excel.
    /// </summary>
    public int? CustomExcelFormatId { get; private set; }

    /// <summary>
    /// El formato Excel personalizado que tendrá el filtro cuando se presente en reportes exportados a Excel.
    /// </summary>
    public string? CustomExcelFormat { get; private set; }

    /// <summary>
    /// El parámetro asociado al filtro para cuando se ejecuta el reporte.
    /// </summary>
    public Parameter Parameter { get; private set; } = default!;

    /// <summary>
    /// Si es que hay más parámetros asociados al filtro.
    /// </summary>
    public virtual bool HasMoreParameters => false;

    /// <inheritdoc/>
    public ExcelFormat? CurrentExcelFormat
    {
        get
        {
            _currentExcelFormat ??= ExcelFormat ?? (CustomExcelFormatId.HasValue && !string.IsNullOrEmpty(CustomExcelFormat) ? new ExcelFormat(CustomExcelFormatId.Value, CustomExcelFormat) : null);

            return _currentExcelFormat;
        }
    }

    /// <summary>
    /// Los padres de este filtro.
    /// </summary>
    public IEnumerable<int> ParentFilters => _parentFilters.Select(x => x.ListFilterId);

    /// <summary>
    /// Revisa si es que el <paramref name="parameter"/> es soportado también por el filtro.
    /// </summary>
    /// <param name="parameter">El <see cref="Parameter"/> a evaluar.</param>
    /// <returns><c>true</c> si el filtro si lo soporta; en caso contrario, <c>false</c>.</returns>
    public virtual bool SupportsParameter(Parameter? parameter) => false;

    /// <summary>
    /// Si se requiere validación.
    /// </summary>
    protected virtual bool NeedsValidation() => IsRequired;

    /// <summary>
    /// Adjusta el valor.
    /// </summary>
    /// <param name="value">El valor a ser ajustado.</param>
    /// <returns>El valor ajustado.</returns>
    public virtual object AdjustValue(object value) => value;

    /// <summary>
    /// Consigue el valor predeterminado del filtro.
    /// </summary>
    /// <returns>El valor predeterminado del filtro.</returns>
    public abstract object? GetDefaultValue();

    /// <summary>
    /// Consigue el valor del filtro.
    /// </summary>
    /// <param name="value">El valor del filtro.</param>
    /// <returns>El valor del filtro.</returns>
    public virtual object? GetValue(object? value)
    {
        return value is string sValue && string.IsNullOrEmpty(sValue) ? null : value;
    }

    /// <summary>
    /// Consigue el otro valor del filtro.
    /// </summary>
    /// <param name="value">El valor del filtro.</param>
    /// <returns>El otro valor del filtro.</returns>
    public virtual object? GetOtherValue(object? value)
    {
        return null;
    }

    /// <summary>
    /// Consigue el valor como texto para el filtro.
    /// </summary>
    /// <param name="value">El valor a conseguir.</param>
    /// <returns>El texto del valor para el filtro.</returns>
    public virtual object? GetText(object? value) =>
        value;

    /// <summary>
    /// Formatea el valor para el filtro.
    /// </summary>
    /// <param name="value">El valor a ser formateado.</param>
    /// <returns>El valor formateado para el filtro.</returns>
    public virtual string? GetFormattedText(object? value)
    {
        value = GetText(value);
        return !string.IsNullOrEmpty(Format)
            ? string.Format(CultureInfo.CurrentCulture, string.Concat("{0:", Format, "}"), value)
            : value!.ToString();
    }
}
