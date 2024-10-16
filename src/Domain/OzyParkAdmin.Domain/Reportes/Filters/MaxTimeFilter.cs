namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// Define el filtro de tipo hora que sería el máximo valor para otro filtro de tipo hora.
/// </summary>
public sealed class MaxTimeFilter
{
    private MaxTimeFilter()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="MaxTimeFilter"/>.
    /// </summary>
    /// <param name="filter">El filtro tipo hora que sería el máximo valor del filtro.</param>
    public MaxTimeFilter(TimeFilter filter)
    {
        Filter = filter;
    }

    /// <summary>
    /// El filtro tipo hora que sería el máximo valor del filtro.
    /// </summary>
    public TimeFilter Filter { get; private set; } = default!;
}
