using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo de <see cref="DateFilter"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="DateFilterModel"/>.
/// </remarks>
/// <param name="parent">El padre del filtro.</param>
/// <param name="filter">El <see cref="DateFilter"/> asociado.</param>
/// <param name="row">Fila en que se presentará el filtro.</param>
/// <param name="size">El tamaño del layout.</param>
public sealed class DateFilterModel(FilterViewModel parent, DateFilter filter, int row, SizeLayout size) : FilterModel<DateFilter, DateTime?>(parent, filter, row, size)
{
    /// <summary>
    /// El valor tipo fecha.
    /// </summary>
    public DateTime? Date
    {
        get => Value;
        set => Value = value;
    }

    /// <summary>
    /// La fecha mínima que se puede mostrar.
    /// </summary>
    public DateTime? MinDate => Filter.LimitToday ? DateTime.Today : Filter.MinDate;

    /// <summary>
    /// La fecha máxima que se puede mostrar.
    /// </summary>
    public DateTime? MaxDate => Filter.MaxDate;

    internal DateFilterModel? MinFilter => Parent.FindFilter(Filter.MinFilter) as DateFilterModel;

    internal DateFilterModel? MaxFilter => Parent.FindFilter(Filter.MaxFilter) as DateFilterModel;

    /// <summary>
    /// El marcador.
    /// </summary>
    public string? Placeholder => Filter.Placeholder;

    /// <inheritdoc/>
    protected override string? ValidateValue(DateTime? value)
    {
        if (MinDate is not null && value < MinDate)
        {
            return $"{Label} no puede ser menor a {MinDate}.";
        }

        if (MaxDate is not null && value > MaxDate)
        {
            return $"{Label} no puede ser mayor a {MaxDate}.";
        }

        if (MinFilter is not null && value < MinFilter.Date)
        {
            return $"{Label} no puede ser menor a {MinFilter.Label}.";
        }

        if (MaxFilter is not null && value > MaxFilter.Date)
        {
            return $"{Label} no puede ser mayor a {MaxFilter.Label}.";
        }

        return null;
    }
}
