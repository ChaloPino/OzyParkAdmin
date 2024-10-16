namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// Define el filtro de tipo fecha que sería el máximo valor para otro filtro de tipo fecha.
/// </summary>
public sealed class MaxDateFilter
{
    private MaxDateFilter()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="MaxDateFilter"/>.
    /// </summary>
    /// <param name="filter">El filtro tipo fecha que sería el máximo valor del filtro.</param>
    public MaxDateFilter(DateFilter filter)
    {
        Filter = filter;
    }

    /// <summary>
    /// El filtro tipo fecha que sería el máximo valor del filtro.
    /// </summary>
    public DateFilter Filter { get; private set; } = default!;
}
