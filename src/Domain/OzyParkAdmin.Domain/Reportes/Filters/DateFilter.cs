namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// El filtro tipo fecha.
/// </summary>
public sealed class DateFilter : Filter
{
    private MinDateFilter? _minDateFilter;
    private MaxDateFilter? _maxDateFilter;
    private DateFilter()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="DateFilter"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertenece el filtro.</param>
    /// <param name="id">El identificador del filtro.</param>
    public DateFilter(Report report, int id)
        : base(report, id)
    {
    }

    /// <summary>
    /// El marcador.
    /// </summary>
    public string? Placeholder { get; private set; }

    /// <summary>
    /// Si se limita la fecha hasta el día actual.
    /// </summary>
    public bool LimitToday { get; private set; }

    /// <summary>
    /// La fecha mínima para realizar una validación.
    /// </summary>
    public DateTime? MinDate { get; private set; }

    /// <summary>
    /// La fecha máxima para realizar una validación.
    /// </summary>
    public DateTime? MaxDate { get; private set; }

    /// <summary>
    /// El filtro de fecha mínima para realizar una validación.
    /// </summary>
    public DateFilter? MinFilter => _minDateFilter?.Filter;

    /// <summary>
    /// El filtro de fecha máxima para realizar una validación.
    /// </summary>
    public DateFilter? MaxFilter => _maxDateFilter?.Filter;

    /// <summary>
    /// Se complementa con la última hora del día para cuando se haga el filtrado.
    /// </summary>
    public bool UseLastTimeOfDay { get; private set; }

    /// <summary>
    /// Si se usa como valor iniciar la fecha actual.
    /// </summary>
    public bool UseToday { get; private set; }

    /// <inheritdoc/>
    protected override bool NeedsValidation()
        => base.NeedsValidation()
            || MinDate.HasValue
            || MaxDate.HasValue
            || MinFilter != null
            || MaxFilter != null;

    /// <inheritdoc/>
    public override object AdjustValue(object value)
    {
        return UseLastTimeOfDay && value is DateTime date ? date.Date.Add(new TimeSpan(23, 59, 59)) : value;
    }

    /// <inheritdoc/>
    public override object? GetDefaultValue() =>
        UseToday ? DateTime.Today : null;
}
