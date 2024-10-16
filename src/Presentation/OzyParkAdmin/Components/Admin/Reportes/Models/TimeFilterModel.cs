using MassTransit;
using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo de <see cref="TimeFilter"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="TimeFilterModel"/>.
/// </remarks>
/// <param name="parent">El padre del filtro.</param>
/// <param name="filter">El <see cref="TimeFilter"/> asociado.</param>
/// <param name="row">Fila en que se presentará el filtro.</param>
/// <param name="size">El tamaño del layout.</param>
public class TimeFilterModel(FilterViewModel parent, TimeFilter filter, int row, SizeLayout size) : FilterModel<TimeFilter, TimeSpan?>(parent, filter, row, size)
{
    /// <summary>
    /// La hora.
    /// </summary>
    public TimeSpan? Time
    {
        get => Value;
        set => Value = value;
    }

    /// <summary>
    /// La hora mínima que se puede mostrar.
    /// </summary>
    public TimeSpan? MinTime => Filter.MinTime;

    /// <summary>
    /// La hora máxima que se puede mostrar.
    /// </summary>
    public TimeSpan? MaxTime => Filter.MaxTime;

    internal TimeFilterModel? MinFilter => Parent.FindFilter(Filter.MinFilter) as TimeFilterModel;

    internal TimeFilterModel? MaxFilter => Parent.FindFilter(Filter.MaxFilter) as TimeFilterModel;

    /// <summary>
    /// El marcador.
    /// </summary>
    public string? Placeholder => Filter.Placeholder;

    /// <summary>
    /// Los minutos de cuánto en cuánto se avanca.
    /// </summary>
    public int MinuteStep => Filter.StepMinutes ?? 0;

    /// <inheritdoc/>
    protected override string? ValidateValue(TimeSpan? value)
    {
        if (MinTime is not null && value < MinTime)
        {
            return $"{Label} no puede ser menor a {MinTime}.";
        }

        if (MaxTime is not null && value > MaxTime)
        {
            return $"{Label} no puede ser mayor a {MaxTime}.";
        }

        if (MinFilter is not null && value < MinFilter.Time)
        {
            return $"{Label} no puede ser menor a {MinFilter.Label}.";
        }

        if (MaxFilter is not null && value > MaxFilter.Time)
        {
            return $"{Label} no puede ser mayor a {MaxFilter.Label}.";
        }

        return null;
    }
}
