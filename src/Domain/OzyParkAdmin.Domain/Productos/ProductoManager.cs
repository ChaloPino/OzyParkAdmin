using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Contabilidad;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Administra toda la lógica de negocio del <see cref="Producto"/>
/// </summary>
public sealed class ProductoManager
{
    private readonly IProductoRepository _repository;
    private readonly ICategoriaProductoRepository _categoriaRepository;
    private readonly ICentroCostoRepository _centroCostoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ICajaRepository _cajaRepository;
    private readonly CatalogoImagenService _catalogoImagenService;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ProductoManager"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IProductoRepository"/>.</param>
    /// <param name="categoriaRepository">El <see cref="ICategoriaProductoRepository"/>.</param>
    /// <param name="centroCostoRepository">El <see cref="ICentroCostoRepository"/>.</param>
    /// <param name="usuarioRepository">El <see cref="IUsuarioRepository"/>.</param>
    /// <param name="cajaRepository">El <see cref="ICajaRepository"/>.</param>
    /// <param name="catalogoImagenService">El <see cref="CatalogoImagenService"/>.</param>
    public ProductoManager(
        IProductoRepository repository,
        ICategoriaProductoRepository categoriaRepository,
        ICentroCostoRepository centroCostoRepository,
        IUsuarioRepository usuarioRepository,
        ICajaRepository cajaRepository,
        CatalogoImagenService catalogoImagenService)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(categoriaRepository);
        ArgumentNullException.ThrowIfNull(centroCostoRepository);
        ArgumentNullException.ThrowIfNull(usuarioRepository);
        ArgumentNullException.ThrowIfNull(cajaRepository);
        ArgumentNullException.ThrowIfNull(catalogoImagenService);
        _repository = repository;
        _categoriaRepository = categoriaRepository;
        _centroCostoRepository = centroCostoRepository;
        _usuarioRepository = usuarioRepository;
        _cajaRepository = cajaRepository;
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

    /// <summary>
    /// Realiza la activación de un producto.
    /// </summary>
    /// <param name="productoId">El id del producto a activar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de la activación del producto.</returns>
    public async Task<ResultOf<Producto>> ActivarProductoAsync(int productoId, CancellationToken cancellationToken)
    {
        Producto? producto = await _repository.FindByIdAsync(productoId, cancellationToken);

        if (producto is null)
        {
            return new NotFound();
        }

        producto.Activar();
        return producto;
    }

    /// <summary>
    /// Realiza la desactivación de un producto.
    /// </summary>
    /// <param name="productoId">El id del producto a desactivar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de la desactivación del producto.</returns>
    public async Task<ResultOf<Producto>> DesactivarProductoAsync(int productoId, CancellationToken cancellationToken)
    {
        Producto? producto = await _repository.FindByIdAsync(productoId, cancellationToken);

        if (producto is null)
        {
            return new NotFound();
        }

        producto.Desactivar();
        return producto;
    }

    /// <summary>
    /// Realiza el bloqueo de un producto.
    /// </summary>
    /// <param name="productoId">El id del producto a bloquear.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado del bloqueo del producto.</returns>
    public async Task<ResultOf<Producto>> BloquearProductoAsync(int productoId, CancellationToken cancellationToken)
    {
        Producto? producto = await _repository.FindByIdAsync(productoId, cancellationToken);

        if (producto is null)
        {
            return new NotFound();
        }

        producto.Bloquear();
        return producto;
    }

    /// <summary>
    /// Realiza el desbloqueo de un producto.
    /// </summary>
    /// <param name="productoId">El id del producto a desbloquear.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado del desbloqueo del producto.</returns>
    public async Task<ResultOf<Producto>> DesbloquearProductoAsync(int productoId, CancellationToken cancellationToken)
    {
        Producto? producto = await _repository.FindByIdAsync(productoId, cancellationToken);

        if (producto is null)
        {
            return new NotFound();
        }

        producto.Desbloquear();
        return producto;
    }

    /// <summary>
    /// Asigna o desasigna cajas de un producto.
    /// </summary>
    /// <param name="productoId">El id del producto a asignar las cajas.</param>
    /// <param name="cajasAsociar">Las cajas a asignar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de asignar las cajas.</returns>
    public async Task<ResultOf<Producto>> AssignCajasAsync(int productoId, ImmutableArray<CajaInfo> cajasAsociar, CancellationToken cancellationToken)
    {
        Producto? producto = await _repository.FindByIdAsync(productoId, ProductoDetail.Todo, cancellationToken);

        if (producto is null)
        {
            return new NotFound();
        }

        IEnumerable<Caja> cajas = await _cajaRepository.FindByIdsAsync(cajasAsociar.Select(x => x.Id).ToArray(), cancellationToken);

        return producto.AssignCajas(cajas);
    }

    /// <summary>
    /// Asigna o desasigna partes de un producto.
    /// </summary>
    /// <param name="productoId">El id del producto al que se le asignarán las partes..</param>
    /// <param name="partesAsignar">Las partes a asignar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de asignar las partes.</returns>
    public async Task<ResultOf<Producto>> AssignPartesAsync(int productoId, ImmutableArray<ProductoParteInfo> partesAsignar, CancellationToken cancellationToken)
    {
        Producto? producto = await _repository.FindByIdAsync(productoId, ProductoDetail.Todo, cancellationToken);

        if (producto is null)
        {
            return new NotFound();
        }

        IEnumerable<Producto> productos = await _repository.FindByIdsAsync(partesAsignar.Select(x => x.Parte.Id).ToArray(), cancellationToken);

        IEnumerable<(ProductoParteInfo Info, Producto Producto)> partes = (from parte in partesAsignar
                                                                           join productoParte in productos on parte.Parte.Id equals productoParte.Id
                                                                           select (parte, productoParte)).ToList();

        return producto.AssignPartes(partes);
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
