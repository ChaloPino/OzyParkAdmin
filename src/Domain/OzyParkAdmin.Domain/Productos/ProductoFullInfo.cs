using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Contabilidad;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Contiene la información completa de un producto.
/// </summary>
public sealed record ProductoFullInfo
{
    /// <summary>
    /// El id del producto.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// El sku del producto.
    /// </summary>
    public string Sku { get; init; } = string.Empty;

    /// <summary>
    /// El aka del producto.
    /// </summary>
    public string Aka { get; init; } = string.Empty;

    /// <summary>
    /// El nombre del producto.
    /// </summary>
    public string Nombre { get; init; } = string.Empty;

    /// <summary>
    /// El id de la franquicia.
    /// </summary>
    public int FranquiciaId { get; init; }

    /// <summary>
    /// El centro de costo asociado al producto.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; init; } = default!;

    /// <summary>
    /// La categoría asociada al producto.
    /// </summary>
    public CategoriaProductoInfo Categoria { get; init; } = default!;

    /// <summary>
    /// La categoría de despliegue asociada al producto.
    /// </summary>
    public CategoriaProductoInfo CategoriaDespliegue { get; init; } = default!;

    /// <summary>
    /// La imagen asociada al producto.
    /// </summary>
    public CatalogoImagenInfo Imagen { get; init; } = default!;

    /// <summary>
    /// El tipo del producto.
    /// </summary>
    public TipoProducto TipoProducto { get; init; } = default!;

    /// <summary>
    /// El orden de despliegue del producto.
    /// </summary>
    public int Orden { get; init; }

    /// <summary>
    /// La agrupación contable del producto.
    /// </summary>
    public AgrupacionContable Familia { get; init; } = default!;

    /// <summary>
    /// Si el producto es un complemento.
    /// </summary>
    public bool EsComplemento { get; init; }

    /// <summary>
    /// Si el producto está en inventario.
    /// </summary>
    public bool EnInventario { get; init; }

    /// <summary>
    /// La fecha de alta del producto.
    /// </summary>
    public DateOnly FechaAlta { get; init; }

    /// <summary>
    /// La fecha de creación del producto.
    /// </summary>
    public DateTime FechaSistema { get; init; }

    /// <summary>
    /// El usuario que creó el producto.
    /// </summary>
    public UsuarioInfo UsuarioCreacion { get; init; } = default!;

    /// <summary>
    /// La última fecha de actualización del producto.
    /// </summary>
    public DateTime UltimaModificacion { get; init; }

    /// <summary>
    /// El usuario que actualizó el producto.
    /// </summary>
    public UsuarioInfo UsuarioModificacion { get; init; } = default!;

    /// <summary>
    /// Si el producto está activo.
    /// </summary>
    public bool EsActivo { get; init; }

    /// <summary>
    /// Las complementos del producto.
    /// </summary>
    public IEnumerable<ProductoComplementarioInfo> Complementos { get; set; } = [];

    /// <summary>
    /// Las cajas asociadas al producto.
    /// </summary>
    public ImmutableArray<CajaInfo> Cajas { get; set; } = [];

    /// <summary>
    /// Las partes del producto.
    /// </summary>
    public ImmutableArray<ProductoParteInfo> Partes { get; set; } = [];

    /// <summary>
    /// Los productos relacionados.
    /// </summary>
    public ImmutableArray<ProductoRelacionadoInfo> Relacionados { get; set; } = [];
}
