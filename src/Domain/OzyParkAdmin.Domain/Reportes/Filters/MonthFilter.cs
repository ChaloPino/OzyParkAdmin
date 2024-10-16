using OzyParkAdmin.Domain.Reportes.DataSources;
using System.Globalization;
using System.Text;

namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// El filtro tipo mes.
/// </summary>
public sealed class MonthFilter : Filter
{
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
    public override object? GetValue(object? value)
    {
        if (value is not null && DateTime.TryParseExact(value.ToString(), "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime date))
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        }

        return null;
    }

    /// <inheritdoc/>
    public override object? GetOtherValue(object? value)
    {
        if (ToParameter is null)
        {
            return null;
        }

        if (value is not null && DateTime.TryParseExact(value.ToString(), "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime date))
        {
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            date = new DateTime(date.Year, date.Month, daysInMonth, 23, 59, 59, 0, DateTimeKind.Unspecified);
            return date.ToString("O", CultureInfo.InvariantCulture);

        }

        return null;
    }
}