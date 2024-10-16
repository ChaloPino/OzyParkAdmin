using System.Text;

namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// Filtro de tipo texto.
/// </summary>
public sealed class TextFilter : Filter
{
    private TextFilter()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="TextFilter"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertenece el filtro.</param>
    /// <param name="id">El identificador del filtro.</param>
    public TextFilter(Report report, int id)
        : base(report, id)
    {
    }

    /// <summary>
    /// El marcador.
    /// </summary>
    public string? Placeholder { get; private set; }

    /// <summary>
    /// El largo máximo del texto, para validaciones.
    /// </summary>
    public int? MaxLength { get; private set; }

    /// <summary>
    /// Un patrón de expresión regular, para validaciones.
    /// </summary>
    public string? Pattern { get; private set; }

    /// <summary>
    /// El valor predeterminado del filtro.
    /// </summary>
    public string? DefaultValue { get; private set; }

    /// <inheritdoc/>
    protected override bool NeedsValidation() =>
        base.NeedsValidation()
            || MaxLength.HasValue
            || Pattern != null;

    /// <inheritdoc/>
    public override object? GetDefaultValue() =>
        DefaultValue;
}
