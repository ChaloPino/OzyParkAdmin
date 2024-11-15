using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.CategoriasProducto;
using System.Security.Claims;

namespace OzyParkAdmin.Application.CategoriasProducto.Update;

/// <summary>
/// Modifica una Categoria de productos
/// </summary>
/// <param name="Id"></param>
/// <param name="FranquiciaId"></param>
/// <param name="Aka"></param>
/// <param name="Nombre"></param>
/// <param name="PadreInfo"></param>
/// <param name="EsFinal"></param>
/// <param name="ImagenInfo"></param>
/// <param name="Orden"></param>
/// <param name="EsTop"></param>
/// <param name="Nivel"></param>
/// <param name="PrimeroProductos"></param>
/// <param name="UsuarioModificacion"></param>
/// <param name="UltimaModificacion"></param>
public sealed record UpdateCategoriaProducto(
        int Id,
        int FranquiciaId,
        string Aka,
        string Nombre,
        CategoriaProductoInfo PadreInfo,
        bool EsFinal,
        CatalogoImagenInfo ImagenInfo,
        int Orden,
        bool EsTop,
        short Nivel,
        bool PrimeroProductos,
        ClaimsPrincipal UsuarioModificacion,
        DateTime UltimaModificacion
    ) : ICommand<CategoriaProductoFullInfo>;
