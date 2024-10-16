namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// Es un filtro de tipo checked o switch.
/// </summary>
public sealed class CheckFilter : Filter
{
    private CheckFilter()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="CheckFilter"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertenece este filtro.</param>
    /// <param name="id">El identificador de este filtro.</param>
    public CheckFilter(Report report, int id)
        : base(report, id)
    {
    }

    /// <summary>
    /// El valor predefinido del filtro.
    /// </summary>
    public bool Checked { get; private set; }

    /// <inheritdoc/>
    public override object? GetDefaultValue() =>
        Checked;
}
