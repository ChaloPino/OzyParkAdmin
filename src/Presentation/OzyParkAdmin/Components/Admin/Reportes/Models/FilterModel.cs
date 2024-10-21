using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo del filtro.
/// </summary>
/// <typeparam name="TFilter">El tipo del filtro.</typeparam>
/// <typeparam name="T">El tipo del valor del filtro.</typeparam>
/// <remarks>
/// Crea una instancia de <see cref="FilterModel{TFilter, T}"/>.
/// </remarks>
/// <param name="parent">El padre del filtro.</param>
/// <param name="filter">El filtro asociado.</param>
/// <param name="row">La en fila en que se despliega el filtro.</param>
/// <param name="size">El tamaño para el layout.</param>
public abstract class FilterModel<TFilter, T>(FilterViewModel parent, TFilter filter, int row, SizeLayout size) : IFilterModel
    where TFilter : Filter
{
    private T? _value;
    private bool _valueInitialized;

    /// <summary>
    /// El padre del filtro.
    /// </summary>
    protected FilterViewModel Parent { get; } = parent;

    /// <summary>
    /// El filtrop asociado.
    /// </summary>
    protected TFilter Filter { get; } = filter;

    /// <summary>
    /// En qué fila se despliega el filtro.
    /// </summary>
    public int Row { get; } = row;

    /// <summary>
    /// El tamaño del layout del filtro.
    /// </summary>
    public SizeLayout SizeLayout { get; } = size;

    /// <summary>
    /// Consigue y asignar el valor del filtro.
    /// </summary>
    internal virtual T? Value
    {
        get
        {
            if (!_valueInitialized)
            {
                _value = (T?)Filter?.GetDefaultValue();
                _valueInitialized = true;
            }

            return _value;
        }
        set
        {
            _value = value;
            _valueInitialized = true;
        }
    }

    /// <summary>
    /// El id del reporte.
    /// </summary>
    public Guid ReportId => Filter.ReportId;

    /// <summary>
    /// El id del filtro.
    /// </summary>
    public int Id => Filter.Id;

    /// <inheritdoc/>
    public string Name => Filter.Name;

    /// <inheritdoc/>
    public string Label => Filter.Label ?? Filter.Name;

    /// <inheritdoc/>
    public bool IsRequired => Filter.IsRequired;

    /// <inheritdoc/>
    public string? InvalidMessage => Filter.InvalidMessage;

    /// <summary>
    /// Los filtros padres.
    /// </summary>
    public IEnumerable<int> ParentFilters => Filter.ParentFilters;

    /// <summary>
    /// El validador.
    /// </summary>
    public Func<T?, string?> Validation => Validate;

    /// <inheritdoc/>
    Filter IFilterModel.Filter => Filter;

    /// <summary>
    /// Consigue los valores de los filtros padres.
    /// </summary>
    /// <returns>Los valores de los filtros padres.</returns>
    public string?[] GetParentValues() =>
        Parent.GetParentValuesForFilter(this);

    /// <inheritdoc/>
    public virtual object? GetValue() =>
        Value;

    /// <summary>
    /// Valida el valor del filtro.
    /// </summary>
    /// <param name="value">El valor del filtro a validar.</param>
    /// <returns>Retorna el mensaje de error si es que no es válido; en caso contrario retorna <c>null</c>.</returns>
    public string? Validate(T? value)
    {
        if (IsRequired && EqualityComparer<T>.Default.Equals(value, default))
        {
            return InvalidMessage ?? $"{Name} es requerido.";
        }

        return ValidateValue(value!);
    }

    /// <summary>
    /// Valida el valor del filtro.
    /// </summary>
    /// <param name="value">El valor del filtro a validar.</param>
    /// <returns>Retorna el mensaje de error si es que no es válido; en caso contrario retorna <c>null</c>.</returns>
    protected virtual string? ValidateValue(T value)
    {
        return null;
    }
}
