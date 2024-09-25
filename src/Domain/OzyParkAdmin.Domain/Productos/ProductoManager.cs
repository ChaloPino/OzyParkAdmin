using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Contabilidad;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Contiene todos los servicios transaccionales del <see cref="Producto"/>
/// </summary>
public sealed class ProductoManager
{
    private readonly IProductoRepository _repository;
    private readonly ICategoriaProductoRepository _categoriaRepository;
    private readonly ICentroCostoRepository _centroCostoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly CatalogoImagenService _catalogoImagenService;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ProductoManager"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IProductoRepository"/>.</param>
    /// <param name="categoriaRepository">El <see cref="ICategoriaProductoRepository"/>.</param>
    /// <param name="centroCostoRepository">El <see cref="ICentroCostoRepository"/>.</param>
    /// <param name="usuarioRepository">El <see cref="IUsuarioRepository"/>.</param>
    /// <param name="catalogoImagenService">El <see cref="CatalogoImagenService"/>.</param>
    public ProductoManager(
        IProductoRepository repository,
        ICategoriaProductoRepository categoriaRepository,
        ICentroCostoRepository centroCostoRepository,
        IUsuarioRepository usuarioRepository,
        CatalogoImagenService catalogoImagenService)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(categoriaRepository);
        ArgumentNullException.ThrowIfNull(centroCostoRepository);
        ArgumentNullException.ThrowIfNull(usuarioRepository);
        ArgumentNullException.ThrowIfNull(catalogoImagenService);
        _repository = repository;
        _categoriaRepository = categoriaRepository;
        _centroCostoRepository = centroCostoRepository;
        _usuarioRepository = usuarioRepository;
        _catalogoImagenService = catalogoImagenService;
    }

    /// <summary>
    /// Crea un nuevo producto
    /// </summary>
    /// <param name="aka">El aka del producto.</param>
    /// <param name="sku">El sku del producto.</param>
    /// <param name="nombre">El nombre del producto.</param>
    /// <param name="franquiciaId">El id de la franquicia.</param>
    /// <param name="centroCostoInfo">El centro de costo del producto.</param>
    /// <param name="categoriaInfo">La categoría del producto.</param>
    /// <param name="categoriaDespliegueInfo">La categoría del producto.</param>
    /// <param name="imagen">La imagen asociada al producto.</param>
    /// <param name="tipoProducto">El tipo de producto.</param>
    /// <param name="familia">La agrupación contable.</param>
    /// <param name="orden">El orden de despliegue.</param>
    /// <param name="esComplemento">Si el producto es un complemento.</param>
    /// <param name="fechaAlta">La fecha de alta del producto.</param>
    /// <param name="usuarioCreacionInfo">El usuario de creación.</param>
    /// <param name="complementos">Los complementos del producto.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de la creación del producto.</returns>
    public async Task<ResultOf<Producto>> CreateProductoAsync(
        string aka,
        string sku,
        string nombre,
        int franquiciaId,
        CentroCostoInfo centroCostoInfo,
        CategoriaProductoInfo categoriaInfo,
        CategoriaProductoInfo categoriaDespliegueInfo,
        CatalogoImagenInfo imagen,
        TipoProducto tipoProducto,
        AgrupacionContable familia,
        int orden,
        bool esComplemento,
        DateOnly fechaAlta,
        UsuarioInfo usuarioCreacionInfo,
        ImmutableArray<ProductoComplementarioInfo> complementos,
        CancellationToken cancellationToken)
    {
        CentroCosto? centroCosto = await _centroCostoRepository.FindByIdAsync(centroCostoInfo.Id, cancellationToken);

        if (centroCosto is null)
        {
            return new ValidationError(nameof(Producto.CentroCosto), "No existe el centro de costo.");
        }

        CategoriaProducto? categoria = await _categoriaRepository.FindByIdAsync(categoriaInfo.Id, cancellationToken);

        if (categoria is null)
        {
            return new ValidationError(nameof(Producto.Categoria), "No existe la categoría.");
        }

        CategoriaProducto? categoriaDespliegue = await _categoriaRepository.FindByIdAsync(categoriaDespliegueInfo.Id, cancellationToken);

        if (categoriaDespliegue is null)
        {
            return new ValidationError(nameof(Producto.CategoriaDespliegue), "No existe la categoría de despliegue.");
        }

        Usuario? usuarioCreacion = await _usuarioRepository.FindByIdAsync(usuarioCreacionInfo.Id, cancellationToken);

        if (usuarioCreacion is null)
        {
            return new ValidationError(nameof(Producto.UsuarioCreacion), "No existe el usuario.");
        }

        int newId = await GenerateIdAysnc(cancellationToken);

        var productoComplementos = await FindComplementariosAsync(complementos, cancellationToken);

        CatalogoImagen catalogoImagen = await _catalogoImagenService.FindOrCreateAsync(imagen, cancellationToken);

        return Producto.Create(
            newId,
            aka,
            sku,
            nombre,
            franquiciaId,
            centroCosto,
            categoria,
            categoriaDespliegue,
            catalogoImagen,
            tipoProducto,
            familia,
            orden,
            esComplemento,
            fechaAlta,
            usuarioCreacion,
            productoComplementos);
    }

    /// <summary>
    /// Actualiza un producto.
    /// </summary>
    /// <param name="id">El id del producto.</param>
    /// <param name="aka">El aka del producto.</param>
    /// <param name="sku">El sku del producto.</param>
    /// <param name="nombre">El nombre del producto.</param>
    /// <param name="centroCostoInfo">El centro de costo del producto.</param>
    /// <param name="categoriaInfo">La categoría del producto.</param>
    /// <param name="categoriaDespliegueInfo">La categoría del producto.</param>
    /// <param name="imagen">La imagen asociada al producto.</param>
    /// <param name="tipoProducto">El tipo de producto.</param>
    /// <param name="familia">La agrupación contable.</param>
    /// <param name="orden">El orden de despliegue.</param>
    /// <param name="esComplemento">Si el producto es un complemento.</param>
    /// <param name="fechaAlta">La fecha de alta del producto.</param>
    /// <param name="usuarioCreacionInfo">El usuario de creación.</param>
    /// <param name="complementos">Los complementos del producto.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de la creación del producto.</returns>
    public async Task<ResultOf<Producto>> UpdateProductoAsync(
        int id,
        string aka,
        string sku,
        string nombre,
        CentroCostoInfo centroCostoInfo,
        CategoriaProductoInfo categoriaInfo,
        CategoriaProductoInfo categoriaDespliegueInfo,
        CatalogoImagenInfo imagen,
        TipoProducto tipoProducto,
        AgrupacionContable familia,
        int orden,
        bool esComplemento,
        DateOnly fechaAlta,
        UsuarioInfo usuarioCreacionInfo,
        ImmutableArray<ProductoComplementarioInfo> complementos,
        CancellationToken cancellationToken)
    {
        Producto? producto = await _repository.FindByIdAsync(id, cancellationToken);

        if (producto is null)
        {
            return new NotFound();
        }

        CentroCosto? centroCosto = await _centroCostoRepository.FindByIdAsync(centroCostoInfo.Id, cancellationToken);

        if (centroCosto is null)
        {
            return new ValidationError(nameof(Producto.CentroCosto), "No existe el centro de costo.");
        }

        CategoriaProducto? categoria = await _categoriaRepository.FindByIdAsync(categoriaInfo.Id, cancellationToken);

        if (categoria is null)
        {
            return new ValidationError(nameof(Producto.Categoria), "No existe la categoría.");
        }

        CategoriaProducto? categoriaDespliegue = await _categoriaRepository.FindByIdAsync(categoriaDespliegueInfo.Id, cancellationToken);

        if (categoriaDespliegue is null)
        {
            return new ValidationError(nameof(Producto.CategoriaDespliegue), "No existe la categoría de despliegue.");
        }

        Usuario? usuarioCreacion = await _usuarioRepository.FindByIdAsync(usuarioCreacionInfo.Id, cancellationToken);

        if (usuarioCreacion is null)
        {
            return new ValidationError(nameof(Producto.UsuarioCreacion), "No existe el usuario.");
        }

        var productoComplementos = await FindComplementariosAsync(complementos, cancellationToken);

        CatalogoImagen catalogoImagen = await _catalogoImagenService.FindOrCreateAsync(imagen, cancellationToken);

        return producto.Update(
            aka,
            sku,
            nombre,
            categoria,
            categoriaDespliegue,
            catalogoImagen,
            tipoProducto,
            familia,
            orden,
            esComplemento,
            fechaAlta,
            usuarioCreacion,
            productoComplementos);
    }

    private async Task<int> GenerateIdAysnc(CancellationToken cancellationToken)
    {
        int id = await _repository.MaxIdAsync(cancellationToken);
        return id + 1;
    }

    private async Task<IEnumerable<(ProductoComplementarioInfo Info, Producto Producto)>> FindComplementariosAsync(IEnumerable<ProductoComplementarioInfo> complementos, CancellationToken cancellationToken)
    {
        IEnumerable<Producto> productos = await _repository.FindByIdsAsync(complementos.Select(x => x.Complemento.Id).ToArray(), cancellationToken);

        return (from complemento in complementos
                join producto in productos on complemento.Complemento.Id equals producto.Id
                select (complemento, producto)).ToList();
    }
}
