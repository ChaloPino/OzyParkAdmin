using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Contabilidad;
using OzyParkAdmin.Domain.Productos;
using System.Collections.Immutable;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Productos.Update;

/// <summary>
/// Actualiza la información de un producto.
/// </summary>
/// <param name="Id">El id del producto a actualizar.</param>
/// <param name="Aka">El aka del producto.</param>
/// <param name="Sku">El sku del producto.</param>
/// <param name="Nombre">El nombre del producto.</param>
/// <param name="FranquiciaId">El id de la franquicia.</param>
/// <param name="CentroCosto">El centro de costo del producto.</param>
/// <param name="Categoria">La categoría del producto.</param>
/// <param name="CategoriaDespliegue">La categoría de despliegue del producto.</param>
/// <param name="Imagen">La imagen del producto.</param>
/// <param name="TipoProducto">El tipo de producto.</param>
/// <param name="Orden">El orden de despliegue.</param>
/// <param name="Familia">La familia contable.</param>
/// <param name="EsComplemento">Si el producto es complemento.</param>
/// <param name="FechaAlta">La fecha de alta del producto.</param>
/// <param name="UsuarioModificacion">El usuario que está haciendo la modificación.</param>
/// <param name="Complementos">Los complementos del producto.</param>
public sealed record UpdateProducto(
    int Id,
    string Aka,
    string Sku,
    string Nombre,
    int FranquiciaId,
    CentroCostoInfo CentroCosto,
    CategoriaProductoInfo Categoria,
    CategoriaProductoInfo CategoriaDespliegue,
    CatalogoImagenInfo Imagen,
    TipoProducto TipoProducto,
    int Orden,
    AgrupacionContable Familia,
    bool EsComplemento,
    DateOnly FechaAlta,
    ClaimsPrincipal UsuarioModificacion,
    ImmutableArray<ProductoComplementarioInfo> Complementos) : IProductoStateChangeable;
