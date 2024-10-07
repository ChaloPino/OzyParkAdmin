using System.Diagnostics.CodeAnalysis;

namespace OzyParkAdmin.Infrastructure.Plantillas;

/// <summary>
/// Contiene información que se usará como datos adicionales de tipo colección.
/// </summary>
/// <typeparam name="T">El tipo de elemento.</typeparam>
public class DataContainer<T>
{
    private readonly IDictionary<string, T> data;
    private readonly Func<T, string>? _stringResolver;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DataContainer{T}"/>.
    /// </summary>
    /// <param name="data">La lista de elementos.</param>
    /// <param name="keyResolver">Una función que resuelve la clave del elemento.</param>
    /// <param name="stringResolver">Una funcíón que resuelve cómo se renderizará el elemento en el html.</param>
    public DataContainer(IEnumerable<T> data, Func<T, string> keyResolver, Func<T, string>? stringResolver)
    {
        this.data = data.ToDictionary(keyResolver, x => x);
        _stringResolver = stringResolver;
    }

    /// <summary>
    /// Si existe la clave del elemento.
    /// </summary>
    /// <typeparam name="TKey">El tipo de la clave.</typeparam>
    /// <param name="key">La clave del elemento a buscar.</param>
    /// <param name="format">El formato adicional para revisar si existe.</param>
    /// <returns>Si existe el elemento.</returns>
    public bool Exists<TKey>(TKey key, string format) =>
        TryGetItem(key, format, out _);

    /// <summary>
    /// Resuelve si es que se renderiza o no el elemento.
    /// </summary>
    /// <typeparam name="TKey">El tipo de la clave.</typeparam>
    /// <param name="key">La clave del elemento a buscar.</param>
    /// <param name="format">El formato del elemento.</param>
    /// <returns>El valor renderizado del elemento.</returns>
    public string? Resolve<TKey>(TKey key, string format)
    {
        if (TryGetItem(key, format, out T? item))
        {
            if (_stringResolver is not null)
            {
                return _stringResolver(item);
            }

            return item.ToString();
        }

        return null;
    }

    private bool TryGetItem<TKey>(TKey key, string format, [NotNullWhen(true)]out T? item)
    {
        string keyToSearch = string.Format(format, key);
        return data.TryGetValue(keyToSearch, out item);
    }
}