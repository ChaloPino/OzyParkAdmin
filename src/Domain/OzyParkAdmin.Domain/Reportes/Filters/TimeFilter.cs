using System.Globalization;

namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// El filtro de tipo hora.
/// </summary>
public sealed class TimeFilter : Filter
{
    private MinTimeFilter? _minTimeFilter;
    private MaxTimeFilter? _maxTimeFilter;

    private TimeFilter()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="TimeFilter"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertenece el filtro.</param>
    /// <param name="id">El identificador del filtro.</param>
    public TimeFilter(Report report, int id)
        : base(report, id)
    {
    }

    /// <summary>
    /// El marcador.
    /// </summary>
    public string? Placeholder { get; private set; }

    /// <summary>
    /// La hora mínima, para validaciones.
    /// </summary>
    public TimeSpan? MinTime { get; private set; }

    /// <summary>
    /// La hora máxima, para validaciones.
    /// </summary>
    public TimeSpan? MaxTime { get; private set; }

    /// <summary>
    /// El cambio de minutos que se puede hacer.
    /// </summary>
    public int? StepMinutes { get; private set; }

    /// <summary>
    /// El filtro de tipo hora que será el valor mínimo, para validaciones.
    /// </summary>
    public TimeFilter? MinFilter => _minTimeFilter?.Filter;

    /// <summary>
    /// El filtro de tipo hora que será el valor máximo, para validaciones.
    /// </summary>
    public TimeFilter? MaxFilter => _maxTimeFilter?.Filter;

    /// <summary>
    /// Si se usará la hora actual como valor predeterminado.
    /// </summary>
    public bool UseNow { get; private set; }

    /// <summary>
    /// El valor predeterminado.
    /// </summary>
    public TimeSpan? DefaultValue { get; private set; }

    /// <inheritdoc/>
    protected override bool NeedsValidation()
        => base.NeedsValidation()
        || MinTime.HasValue
        || MaxTime.HasValue
        || MinFilter != null
        || MaxFilter != null;

    /// <inheritdoc/>
    public override object? GetDefaultValue() =>
        UseNow ? DateTime.Today.TimeOfDay : DefaultValue;

    /// <inheritdoc/>
    public override object? GetText(object? value) =>
        value is not null ? TimeSpan.Parse(value.ToString()!, CultureInfo.InvariantCulture) : null;
}
