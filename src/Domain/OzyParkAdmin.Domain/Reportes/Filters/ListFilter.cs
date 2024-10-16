using OzyParkAdmin.Domain.Reportes.DataSources;
using System.Text;

namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// Filtro de tipo lista.
/// </summary>
public sealed class ListFilter : Filter, IRemoteFilter
{
    private readonly List<FilterData> _list = [];
    private ListFilter()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListFilter"/>.
    /// </summary>
    /// <param name="report">El reporte al que pertenece el filro.</param>
    /// <param name="id">El identificador del filtro.</param>
    public ListFilter(Report report, int id)
        : base(report, id)
    {
    }

    /// <summary>
    /// El valor opcional.
    /// </summary>
    public string? OptionalValue { get; private set; }

    /// <summary>
    /// Si puede tener múltiples valores.
    /// </summary>
    public bool IsMultiple { get; private set; }

    /// <summary>
    /// El tamaño máximo de elementos.
    /// </summary>
    public int? Size { get; private set; }

    /// <summary>
    /// Si el filtro consigue la lista de forma remota.
    /// </summary>
    public bool IsRemote { get; private set; }

    /// <inheritdoc/>
    public DataSource? RemoteDataSource { get; set; }

    /// <summary>
    /// La lista de datos de filtrado.
    /// </summary>
    public IEnumerable<ItemOption> List => _list.Select(x => new ItemOption { Valor = x.Value, Display = x.Display });

    /// <inheritdoc/>
    public override object? GetDefaultValue() =>
        List?.Any() == true ? List.First() : null;

    /// <inheritdoc/>
    public override object? GetValue(object? value)
    {
        if (value is Array array)
        {
            Dictionary<string, object?>[] dictionary = (Dictionary<string, object?>[])array;

            return !IsMultiple
                ? base.GetValue(dictionary[0]["value"])
                : dictionary.Select(k => k["value"]).ToArray();
        }

        return base.GetValue(value);
    }
}
