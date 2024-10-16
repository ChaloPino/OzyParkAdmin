namespace OzyParkAdmin.Domain.Reportes.Filters;
/// <summary>
/// Define el filtro de tipo hora que sería el mínimo valor para otro filtro de tipo hora.
/// </summary>
public sealed class MinTimeFilter
{
    private MinTimeFilter()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="MinTimeFilter"/>.
    /// </summary>
    /// <param name="filter">El filtro tipo hora que sería el mínimo valor del filtro.</param>
    public MinTimeFilter(TimeFilter filter)
    {
        Filter = filter;
    }

    /// <summary>
    /// El filtro tipo hora que sería el mínimo valor del filtro.
    /// </summary>
    public TimeFilter Filter { get; private set; } = default!;
}
