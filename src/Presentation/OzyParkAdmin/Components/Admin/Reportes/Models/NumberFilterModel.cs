using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo de <see cref="TextFilter"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="TextFilterModel"/>.
/// </remarks>
/// <param name="parent">El padre del filtro.</param>
/// <param name="filter">El <see cref="TextFilter"/> asociado.</param>
/// <param name="row">Fila en que se presentará el filtro.</param>
/// <param name="size">El tamaño del layout.</param>
public sealed class NumberFilterModel(FilterViewModel parent, TextFilter filter, int row, SizeLayout size) : FilterModel<TextFilter, double?>(parent, filter, row, size)
{
    /// <summary>
    /// El valor numérico.
    /// </summary>
    public double? Number
    {
        get => Value;
        set => Value = value;
    }

    /// <summary>
    /// El marcador.
    /// </summary>
    public string? Placeholder => Filter.Placeholder;
}
