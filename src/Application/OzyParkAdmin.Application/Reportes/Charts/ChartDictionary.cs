using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace OzyParkAdmin.Application.Reportes.Charts;

/// <summary>
/// El diccionario base para todos los elementos que conforman un gráfico.
/// </summary>
public abstract class ChartDictionary : IDictionary<string, object?>
{
    private readonly Dictionary<string, object?> _innerDictionary = new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public object? this[string key] { get => _innerDictionary[key]; set => _innerDictionary[key] = value; }

    /// <inheritdoc/>
    ICollection<string> IDictionary<string, object?>.Keys => _innerDictionary.Keys;

    /// <inheritdoc/>
    ICollection<object?> IDictionary<string, object?>.Values => _innerDictionary.Values;

    /// <inheritdoc/>
    public int Count => _innerDictionary.Count;

    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, object?>>.IsReadOnly => false;

    /// <summary>
    /// Agrega un elemento con su respectiva clave al diccionario.
    /// </summary>
    /// <param name="key">La clave elemento.</param>
    /// <param name="value">El valor del elemento.</param>
    protected void Add(string key, object? value) =>
        _innerDictionary.Add(key, value);

    /// <inheritdoc/>
    void IDictionary<string, object?>.Add(string key, object? value) =>
        Add(key, value);

    /// <inheritdoc/>
    void ICollection<KeyValuePair<string, object?>>.Add(KeyValuePair<string, object?> item) =>
        Add(item.Key, item.Value);

    /// <inheritdoc/>
    public void Clear() =>
        _innerDictionary.Clear();

    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, object?>>.Contains(KeyValuePair<string, object?> item) =>
        _innerDictionary.Contains(item);

    /// <summary>
    /// Determina si el <see cref="ChartDictionary"/> contiene la clave especificada.
    /// </summary>
    /// <param name="key">La clave a ubicar en <see cref="ChartDictionary"/>.</param>
    /// <returns>
    /// <c>true</c> si <see cref="ChartDictionary"/> contiene un elemento con la clave especificada;
    /// en caso contrario, <c>false</c>.
    /// </returns>
    protected bool ContainsKey(string key) =>
        _innerDictionary.ContainsKey(key);

    /// <inheritdoc/>
    bool IDictionary<string, object?>.ContainsKey(string key) =>
        ContainsKey(key);

    /// <inheritdoc/>
    void ICollection<KeyValuePair<string, object?>>.CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex) =>
        ((ICollection<KeyValuePair<string, object?>>)_innerDictionary).CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object?>>.GetEnumerator() =>
        _innerDictionary.GetEnumerator();

    /// <summary>
    /// Elimina el elemento con la clave especificada del <see cref="ChartDictionary"/>.
    /// </summary>
    /// <param name="key">La clave del elemento a eliminar.</param>
    /// <returns>
    /// <c>true</c> si el elemento se encontró y eliminó satisfactoriamente; en caso contrario, <c>false</c>.
    /// Este método retorna <c>false</c> si la clave no se encuentra en <see cref="ChartDictionary"/>.
    /// </returns>
    protected bool Remove(string key) =>
        _innerDictionary.Remove(key);

    /// <inheritdoc/>
    bool IDictionary<string, object?>.Remove(string key) =>
        Remove(key);

    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, object?>>.Remove(KeyValuePair<string, object?> item) =>
        Remove(item.Key);

    /// <summary>
    /// Devuelve el elemento asociado a la clave especificada.
    /// </summary>
    /// <param name="key">La clave del valor a conseguir.</param>
    /// <param name="value">
    /// Cuando este método retorna, contiene el valor asociado con la clave especificada, si <paramref name="key"/> se encontró;
    /// en caso contrario, el valor predeterminado para el tipo del parámetro <paramref name="value"/>. Este parámetro se pasa sin inicializar.
    /// </param>
    /// <returns>
    /// <c>true</c> si el <see cref="ChartDictionary"/> contiene un elemento con la clave especificada; en caso contrario, <c>false</c>.
    /// </returns>
    protected bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value) =>
        _innerDictionary.TryGetValue(key, out value);

    /// <inheritdoc/>
    bool IDictionary<string, object?>.TryGetValue(string key, [MaybeNullWhen(false)] out object? value) =>
       TryGetValue(key, out value);

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() =>
        _innerDictionary.GetEnumerator();
}
