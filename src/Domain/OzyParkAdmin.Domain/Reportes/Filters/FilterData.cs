namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// El dato para un <see cref="ListFilter"/>.
/// </summary>
public sealed class FilterData
{
    private FilterData()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="FilterData"/>.
    /// </summary>
    /// <param name="listFilter">El <see cref="ListFilter"/> al que pertenece el dato.</param>
    /// <param name="value">El valor del dato.</param>
    /// <param name="display">El texto del dato.</param>
    /// <param name="order">El orden de despliegue del dato.</param>
    public FilterData(ListFilter listFilter, string value, string display, int order)
    {
        ReportId = listFilter.ReportId;
        FilterId = listFilter.Id;
        Value = value;
        Display = display;
        Order = order;
    }

    /// <summary>
    /// El id del reporte.
    /// </summary>
    public Guid ReportId { get; private set; }

    /// <summary>
    /// El id del filtro.
    /// </summary>
    public int FilterId { get; private set; }

    /// <summary>
    /// El valor del dato.
    /// </summary>
    public string Value { get; private set; } = default!;

    /// <summary>
    /// El texto del dato.
    /// </summary>
    public string Display { get; private set; } = default!;

    /// <summary>
    /// El orden de despliegue del dato.
    /// </summary>
    public int Order { get; private set; }
}
