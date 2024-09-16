namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa una lista paginada.
/// </summary>
/// <typeparam name="T">El tipo del elemento.</typeparam>
public sealed class PagedList<T>
{
    /// <summary>
    /// El página actual.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// El tamaño de la página.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// El total de elementos que hay.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// La lista de elementos.
    /// </summary>
    public IEnumerable<T> Items { get; set; } = [];
}
