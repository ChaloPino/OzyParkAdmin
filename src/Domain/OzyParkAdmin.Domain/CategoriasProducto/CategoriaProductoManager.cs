
using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;
using static System.Net.Mime.MediaTypeNames;

namespace OzyParkAdmin.Domain.CategoriasProducto;

/// <summary>
/// Administra toda la lógica de negocio de <see cref="CategoriaProducto"/>
/// </summary>
public sealed class CategoriaProductoManager : IBusinessLogic
{
    private readonly ICategoriaProductoRepository _categoriaProductoRepository;
    private readonly CatalogoImagenService _catalogoImagenService;
    private readonly IUsuarioRepository _usuarioRepository;

    /// <summary>
    /// Crea una instancia de <see cref="CategoriaProductoManager"/>
    /// </summary>
    /// <param name="categoriaProductoRepository"></param>
    /// <param name="catalogoImagenService"></param>
    /// <param name="usuarioRepository"></param>
    public CategoriaProductoManager(
        ICategoriaProductoRepository categoriaProductoRepository,
        CatalogoImagenService catalogoImagenService,
        IUsuarioRepository usuarioRepository
        )
    {
        ArgumentNullException.ThrowIfNull(categoriaProductoRepository);
        ArgumentNullException.ThrowIfNull(catalogoImagenService);
        ArgumentNullException.ThrowIfNull(usuarioRepository);

        _categoriaProductoRepository = categoriaProductoRepository;
        _catalogoImagenService = catalogoImagenService;
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Crea una nueva Categoria de Producto
    /// </summary>
    /// <param name="franquiciaId"></param>
    /// <param name="aka"></param>
    /// <param name="nombre"></param>
    /// <param name="padreInfo"></param>
    /// <param name="esFinal"></param>
    /// <param name="imagenInfo"></param>
    /// <param name="orden"></param>
    /// <param name="esTop"></param>
    /// <param name="nivel"></param>
    /// <param name="primeroProductos"></param>
    /// <param name="usuarioCreacionInfo"></param>
    /// <param name="fechaCreacion"></param>
    /// <param name="usuarioModificacionInfo"></param>
    /// <param name="ultimaModificacion"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ResultOf<CategoriaProducto>> CreateAsync(
        int franquiciaId,
        string aka,
        string nombre,
        CategoriaProductoInfo padreInfo,
        bool esFinal,
        CatalogoImagenInfo imagenInfo,
        int orden,
        bool esTop,
        short nivel,
        bool primeroProductos,
        UsuarioInfo usuarioCreacionInfo,
        DateTime fechaCreacion,
        UsuarioInfo usuarioModificacionInfo,
        DateTime ultimaModificacion,
        CancellationToken cancellationToken)
    {
        CategoriaProducto? categoriaProductoPadre = await _categoriaProductoRepository.FindByIdAsync(padreInfo.Id, cancellationToken);
        if (categoriaProductoPadre is null)
        {
            return new ValidationError(nameof(Producto.Categoria), "No existe la categoría padre");
        }

        CatalogoImagen catalogoImagen = await _catalogoImagenService.FindOrCreateAsync(imagenInfo, cancellationToken);

        Usuario? usuarioCreacion = await _usuarioRepository.FindByIdAsync(usuarioCreacionInfo.Id, cancellationToken);

        if (usuarioCreacion is null)
        {
            return new ValidationError(nameof(Producto.UsuarioCreacion), "No existe el usuario.");
        }

        //Al crear usuario de creacion y modificacion es el mismo. UusuarioModificacion en BD no permite nulos.
        Usuario? usuarioModificacion = usuarioCreacion;

        int newId = await GenerateIdAysnc(cancellationToken);

        return CategoriaProducto.Create(
                newId,
                franquiciaId,
                aka,
                nombre,
                categoriaProductoPadre,
                esFinal,
                catalogoImagen,
                orden,
                esTop,
                nivel,
                primeroProductos,
                usuarioCreacion,
                fechaCreacion,
                usuarioModificacion,
                ultimaModificacion
            );

    }

    private async Task<int> GenerateIdAysnc(CancellationToken cancellationToken)
    {
        int id = await _categoriaProductoRepository.MaxIdAsync(cancellationToken);
        return id + 1;
    }

}
