namespace OzyParkAdmin.Domain.CategoriasProducto;

/// <summary>
/// Contiene métodos de extensión para el <see cref="CategoriaProducto"/>.
/// </summary>
public static class CategoriaProductoExtensions
{
    /// <summary>
    /// Convierte una colección de <see cref="CategoriaProducto"/> a una colección de <see cref="CategoriaProductoInfo"/>.
    /// </summary>
    /// <param name="source">La colección de <see cref="CategoriaProducto"/> a convertir.</param>
    /// <returns>La colección de <see cref="CategoriaProductoInfo"/> convertida desde <paramref name="source"/>.</returns>
    public static List<CategoriaProductoInfo> ToInfo(this IEnumerable<CategoriaProducto> source) =>
        [.. source.Select(ToInfo).OrderBy(x => x.NombreCompleto)];

    /// <summary>
    /// Convierte un <see cref="CategoriaProducto"/> en <see cref="CategoriaProductoInfo"/>.
    /// </summary>
    /// <param name="categoriaProducto">El <see cref="CategoriaProducto"/> a convertir.</param>
    /// <returns>El <see cref="CategoriaProductoInfo"/> convertido desde <paramref name="categoriaProducto"/>.</returns>
    public static CategoriaProductoInfo ToInfo(this CategoriaProducto categoriaProducto) =>
        new()
        {
            Id = categoriaProducto.Id,
            Aka = categoriaProducto.Aka,
            Nombre = categoriaProducto.Nombre,
            NombreCompleto = categoriaProducto.ToNombreCompleto(),
            EsActivo = categoriaProducto.EsActivo,
        };
}
