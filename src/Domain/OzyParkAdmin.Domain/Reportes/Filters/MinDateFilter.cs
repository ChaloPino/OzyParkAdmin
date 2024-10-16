namespace OzyParkAdmin.Domain.Reportes.Filters;
/// <summary>
/// Define el filtro de tipo fecha que sería el mínimo valor para otro filtro de tipo fecha.
/// </summary>
public sealed class MinDateFilter
{
    private MinDateFilter()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="MinDateFilter"/>.
    /// </summary>
    /// <param name="filter">El filtro tipo fecha que sería el mínimo valor del filtro.</param>
    public MinDateFilter(DateFilter filter)
    {
        Filter = filter;
    }

    /// <summary>
    /// El filtro tipo fecha que sería el mínimo valor del filtro.
    /// </summary>
    public DateFilter Filter { get; private set; } = default!;
}
