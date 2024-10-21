namespace OzyParkAdmin.Domain.Franquicias;

/// <summary>
/// Contiene métodos de extensión para <see cref="Franquicia"/>.
/// </summary>
public static class FranquiciaExtensions
{
    /// <summary>
    /// Convierte una lista de <see cref="Franquicia"/> en una lista de <see cref="FranquiciaInfo"/>.
    /// </summary>
    /// <param name="source">La lista de <see cref="Franquicia"/> a convertir.</param>
    /// <returns>La lista de <see cref="FranquiciaInfo"/> convertida de <paramref name="source"/>.</returns>
    public static List<FranquiciaInfo> ToInfo(this IEnumerable<Franquicia> source) =>
        [.. source.Select(ToInfo)];

    /// <summary>
    /// Convierte una <see cref="Franquicia"/> en <see cref="FranquiciaInfo"/>.
    /// </summary>
    /// <param name="franquicia">La <see cref="Franquicia"/> a convertir.</param>
    /// <returns>La <see cref="FranquiciaInfo"/> convertida de <paramref name="franquicia"/>.</returns>
    public static FranquiciaInfo ToInfo(this Franquicia franquicia) =>
        new() { Id = franquicia.Id, Nombre = franquicia.Nombre, EsActivo = franquicia.EsActivo };
}
