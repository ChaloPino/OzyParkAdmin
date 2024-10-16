using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo de <see cref="ListFilter"/> con multiples selecciones.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="ListFilterModel"/>.
/// </remarks>
/// <param name="parent">El padre del filtro.</param>
/// <param name="filter">El <see cref="ListFilter"/> asociado.</param>
/// <param name="row">Fila en que se presentará el filtro.</param>
/// <param name="size">El tamaño del layout.</param>
public sealed class MultiListFilterModel(FilterViewModel parent, ListFilter filter, int row, SizeLayout size) : FilterModel<ListFilter, IEnumerable<ItemOption>>(parent, filter, row, size)
{
    /// <summary>
    /// Los elementos seleccionados.
    /// </summary>
    public IEnumerable<ItemOption> SelectedItems
    {
        get => Value ?? [];
        set => Value = value;
    }

    /// <summary>
    /// Si la lista del filtro es remota.
    /// </summary>
    public bool IsRemote => Filter.IsRemote;

    /// <summary>
    /// La lista local del filtro.
    /// </summary>
    public List<ItemOption> List => [.. Filter.List];

    /// <summary>
    /// El valor opcional.
    /// </summary>
    public string? OptionalValue => Filter.OptionalValue;

    /// <summary>
    /// El tamaño máximo de elementos que se pueden seleccionar.
    /// </summary>
    internal int? Size => Filter.Size;

    /// <inheritdoc/>
    public override object? GetValue()
    {
        return SelectedItems.Select(x => x.Valor);
    }

    /// <inheritdoc/>
    protected override string? ValidateValue(IEnumerable<ItemOption> value)
    {
        if (Size is not null && value.Count() > Size)
        {
            return $"{Label} no puede ser mayor a {Size}.";
        }

        return null;
    }
}
