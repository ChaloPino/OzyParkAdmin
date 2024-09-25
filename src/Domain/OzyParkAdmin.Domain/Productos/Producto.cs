using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// La entidad producto.
/// </summary>
public sealed class Producto
{
    private ProductoAgrupacion? _productoAgrupacion;
    private readonly List<Caja> _cajas = [];
    private readonly List<ProductoComplementario> _complementos = [];
    private readonly List<ProductoRelacionado> _relacionados = [];
    private readonly List<ProductoParte> _partes = [];

    /// <summary>
    /// El id del producto.
    /// </summary>
    public int Id { get; private init; }

    /// <summary>
    /// El sku del producto.
    /// </summary>
    public string Sku { get; private set; } = string.Empty;

    /// <summary>
    /// El aka del producto.
    /// </summary>
    public string Aka { get; private set; } = string.Empty;

    /// <summary>
    /// El nombre del producto.
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// El centro de costo asociado al producto.
    /// </summary>
    public CentroCosto CentroCosto { get; private init; } = default!;

    /// <summary>
    /// El id de la franquicia.
    /// </summary>
    public int FranquiciaId { get; private init; }

    /// <summary>
    /// La categoría asociada al producto.
    /// </summary>
    public CategoriaProducto Categoria { get; private set; } = default!;

    /// <summary>
    /// La categoría de despliegue del producto.
    /// </summary>
    public CategoriaProducto CategoriaDespliegue { get; set; } = default!;

    /// <summary>
    /// El catálogo de imagen asociado al producto.
    /// </summary>
    public CatalogoImagen Imagen { get; private set; } = default!;

    /// <summary>
    /// El tipo del producto.
    /// </summary>
    public TipoProducto TipoProducto { get; private set; } = default!;

    /// <summary>
    /// El orden de despliegue del producto.
    /// </summary>
    public int Orden { get; private set; }

    /// <summary>
    /// La familia de agrupación contable del producto.
    /// </summary>
    public AgrupacionContable? Familia => _productoAgrupacion?.AgrupacionContable;

    /// <summary>
    /// Si el producto es complemento de otro producto.
    /// </summary>
    public bool EsComplemento { get; private set; }

    /// <summary>
    /// Si el producto está en inventario y se tiene que "congelar" su venta.
    /// </summary>
    public bool EnInventario { get; private set; }

    /// <summary>
    /// La fecha de alta del producto.
    /// </summary>
    public DateOnly FechaAlta { get; private set; } = DateOnly.FromDateTime(DateTime.Today);

    /// <summary>
    /// La fecha de creación del producto.
    /// </summary>
    public DateTime FechaSistema { get; private init; } = DateTime.Now;

    /// <summary>
    /// El usuario que creó del producto.
    /// </summary>
    public Usuario UsuarioCreacion { get; private init; } = default!;

    /// <summary>
    /// La última fecha de modificación del producto.
    /// </summary>
    public DateTime UltimaModificacion { get; private set; } = DateTime.Now;

    /// <summary>
    /// El usuario que realizó la última modificación del producto.
    /// </summary>
    public Usuario UsuarioModificacion { get; private set; } = default!;

    /// <summary>
    /// Si el producto está activo.
    /// </summary>
    public bool EsActivo { get; private set; } = true;

    /// <summary>
    /// Las cajas asociadas al producto.
    /// </summary>
    public IEnumerable<Caja> Cajas => _cajas;

    /// <summary>
    /// Los productos complementarios del producto.
    /// </summary>
    public IEnumerable<ProductoComplementario> Complementos => _complementos;

    /// <summary>
    /// Los productos relacionados al producto.
    /// </summary>
    public IEnumerable<ProductoRelacionado> Relacionados => _relacionados;

    /// <summary>
    /// Las partes del producto.
    /// </summary>
    public IEnumerable<ProductoParte> Partes => _partes;
}
