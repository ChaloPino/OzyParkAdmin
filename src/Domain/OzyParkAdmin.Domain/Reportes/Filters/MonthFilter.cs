using OzyParkAdmin.Domain.Reportes.DataSources;
using System.Globalization;

namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// El filtro tipo mes.
/// </summary>
public sealed class MonthFilter : Filter
{
    private static readonly TimeSpan LastTimeOfDay = new TimeSpan(23, 59, 59);
    private MonthFilter()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="MonthFilter"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertenece el filtro.</param>
    /// <param name="id">El identificador del filtro.</param>
    public MonthFilter(Report report, int id)
        : base(report, id)
    {
    }

    /// <summary>
    /// Si se usa el día actual como valor predeterminado.
    /// </summary>
    public bool UseToday { get; private set; }

    /// <summary>
    /// El marcador.
    /// </summary>
    public string? Placeholder { get; private set; }

    /// <summary>
    /// El parámetro asociado para definir el rango de fechas de un mes.
    /// Sería el parámetro hasta.
    /// </summary>
    public Parameter? ToParameter { get; private set; }

    /// <inheritdoc/>
    public override bool HasMoreParameters => ToParameter != null;

    /// <inheritdoc/>
    public override bool SupportsParameter(Parameter? parameter) =>
        ToParameter is not null && ToParameter == parameter;

    /// <inheritdoc/>
    public override object? GetDefaultValue() =>
        UseToday ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) : null;

    /// <inheritdoc/>
    public override object? GetValue(object? value) =>
        GetDateTimeValue(value);

    /// <inheritdoc/>
    public override object? GetOtherValue(object? value)
    {
        if (ToParameter is null)
        {
            return null;
        }

        DateTime? dateTime = GetDateTimeValue(value);

        if (dateTime is not null)
        {
            return dateTime.Value.Add(LastTimeOfDay);
        }

        return null;
    }

    private static DateTime? GetDateTimeValue(object? value)
    {
        if (value is DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0, DateTimeKind.Local);
        }
        
        if (value is not null && DateTime.TryParseExact(value.ToString(), "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime date))
        {
            return date;
        }

        return null;
    }
}