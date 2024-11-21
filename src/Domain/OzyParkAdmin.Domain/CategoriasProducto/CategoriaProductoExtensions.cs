using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

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

    /// <summary>
    /// Convierte el <paramref name="categoriaProducto"/> en <see cref="CategoriaProductoFullInfo"/>.
    /// </summary>
    /// <param name="categoriaProducto">La Categoria producto a convertir.</param>
    /// <returns>El <see cref="CategoriaProductoFullInfo"/> convertido desde <paramref name="categoriaProducto"/>.</returns>
    public static CategoriaProductoFullInfo ToFullInfo(this CategoriaProducto categoriaProducto) =>
        new()
        {
            Id = categoriaProducto.Id,
            FranquiciaId = categoriaProducto.FranquiciaId,
            Aka = categoriaProducto.Aka,
            Nombre = categoriaProducto.Nombre,
            EsActivo = categoriaProducto.EsActivo,
            Padre = categoriaProducto.Padre?.ToInfo(),
            EsFinal = categoriaProducto.EsFinal,
            Imagen = categoriaProducto.Imagen.ToInfo(),
            Orden = categoriaProducto.Orden,
            EsTop = categoriaProducto.EsTop,
            Nivel = categoriaProducto.Nivel,
            PrimeroProductos = categoriaProducto.PrimeroProductos,
            UsuarioCreacion = categoriaProducto.UsuarioCreacion.ToInfo(),
            FechaCreacion = categoriaProducto.FechaCreacion,
            UsuarioModificacion = categoriaProducto.UsuarioModificacion.ToInfo(),
            UltimaModificacion = categoriaProducto.UltimaModificacion,
            CajasAsignadas = categoriaProducto.CajasAsignadas,
            CanalesVenta = categoriaProducto.CanalesVenta,
            Hijos = categoriaProducto.Hijos.ToInfo(),
        };
}
