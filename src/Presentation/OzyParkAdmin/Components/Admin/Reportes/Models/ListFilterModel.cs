using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo de <see cref="ListFilter"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="ListFilterModel"/>.
/// </remarks>
/// <param name="parent">El padre del filtro.</param>
/// <param name="filter">El <see cref="ListFilter"/> asociado.</param>
/// <param name="row">Fila en que se presentará el filtro.</param>
/// <param name="size">El tamaño del layout.</param>
public sealed class ListFilterModel(FilterViewModel parent, ListFilter filter, int row, SizeLayout size) : FilterModel<ListFilter, ItemOption>(parent, filter, row, size)
{
    /// <summary>
    /// El valor seleccionado.
    /// </summary>
    public ItemOption? SelectedItem
    {
        get => Value;
        set => Value = value;
    }

    /// <summary>
    /// El valor opcional.
    /// </summary>
    public string? OptionalValue => Filter.OptionalValue;

    /// <summary>
    /// Si el filtro tiene la información de modo remota.
    /// </summary>
    public bool IsRemote => Filter.IsRemote;


    /// <summary>
    /// La lista de elementos a seleccionar.
    /// </summary>
    public List<ItemOption> List => [..Filter.List];

    /// <inheritdoc/>
    public override object? GetValue()
    {
        return SelectedItem?.Valor;
    }
}
