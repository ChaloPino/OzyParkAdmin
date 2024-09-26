using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Contiene los metodos de extensión para <see cref="Producto"/>.
/// </summary>
public static class ProductoExtensions
{
    /// <summary>
    /// Convierte el <paramref name="producto"/> en <see cref="ProductoFullInfo"/>.
    /// </summary>
    /// <param name="producto">El producto a convertir.</param>
    /// <returns>El <see cref="ProductoFullInfo"/> convertido desde <paramref name="producto"/>.</returns>
    public static ProductoFullInfo ToFullInfo(this Producto producto) =>
        new()
        {
            Id = producto.Id,
            Aka = producto.Aka,
            Sku = producto.Sku,
            Nombre = producto.Nombre,
            FranquiciaId = producto.FranquiciaId,
            CentroCosto = producto.CentroCosto?.ToInfo() ?? null!,
            Categoria = producto.Categoria?.ToInfo() ?? null!,
            CategoriaDespliegue = producto.CategoriaDespliegue?.ToInfo() ?? null!,
            Imagen = producto.Imagen?.ToInfo() ?? null!,
            Familia = producto.Familia,
            TipoProducto = producto.TipoProducto,
            Orden = producto.Orden,
            FechaAlta = producto.FechaAlta,
            EsComplemento = producto.EsComplemento,
            EnInventario = producto.EnInventario,
            FechaSistema = producto.FechaSistema,
            UsuarioCreacion = producto.UsuarioCreacion?.ToInfo() ?? null!,
            UltimaModificacion = producto.UltimaModificacion,
            UsuarioModificacion = producto.UsuarioModificacion?.ToInfo() ?? null!,
            EsActivo = producto.EsActivo,
            Complementos = producto.Complementos.ToInfo(),
            Cajas = producto.Cajas.ToInfo(),
            Partes = producto.Partes.ToInfo(),
            Relacionados = producto.Relacionados.ToInfo(),
        };

    private static ImmutableArray<ProductoComplementarioInfo> ToInfo(this IEnumerable<ProductoComplementario> source) =>
        [.. source.Select(ToInfo)];

    private static ProductoComplementarioInfo ToInfo(ProductoComplementario complementario) =>
        new() { Complemento = complementario.Complemento.ToInfo(), Orden = complementario.Orden };

    private static ImmutableArray<ProductoRelacionadoInfo> ToInfo(this IEnumerable<ProductoRelacionado> source) =>
        [.. source.Select(ToInfo)];

    private static ProductoRelacionadoInfo ToInfo(ProductoRelacionado relacionado) =>
        new() { Relacionado = relacionado.Relacionado.ToInfo(), Orden = relacionado.Orden };

    private static ImmutableArray<ProductoParteInfo> ToInfo(this IEnumerable<ProductoParte> source) =>
        [.. source.Select(ToInfo)];

    private static ProductoParteInfo ToInfo(this ProductoParte parte) =>
        new() { Parte = parte.Parte.ToInfo(), Cantidad = parte.Cantidad, EsOpcional = parte.EsOpcional };

    /// <summary>
    /// Convierte el <paramref name="producto"/> en <see cref="ProductoInfo"/>.
    /// </summary>
    /// <param name="producto">El producto a convertir.</param>
    /// <returns>El <see cref="ProductoInfo"/> convertido desde <paramref name="producto"/>.</returns>
    public static ProductoInfo ToInfo(this Producto producto) =>
        new() {  Id = producto.Id, Aka = producto.Aka, Sku = producto.Sku, Nombre = producto.Nombre, EsActivo = producto.EsActivo };
}
