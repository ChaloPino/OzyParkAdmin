using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Models;

/// <summary>
/// El view model de la Categoria Servicio
/// </summary>
public sealed class CategoriaProductoViewModel
{
    /// <summary>
    /// El id de la categoría del producto.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// El id de la franquicia.
    /// </summary>
    public int FranquiciaId { get; init; }

    /// <summary>
    /// El aka de la categoria de producto.
    /// </summary>
    public string Aka { get; set; } = string.Empty;

    /// <summary>
    /// El nombre de la categoría de producto.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Si la categoría de producto está activa.
    /// </summary>
    public bool EsActivo { get; set; } = true;

    /// <summary>
    /// La categoría de producto padre.
    /// </summary>
    public CategoriaProductoInfo? Padre { get; set; }

    /// <summary>
    /// Si la categoría de producto es final y no tiene hijos.
    /// </summary>
    public bool EsFinal { get; set; }

    /// <summary>
    /// La imágen asociada a la categoría de producto.
    /// </summary>
    public CatalogoImagenInfo Imagen { get; set; } = default!;

    /// <summary>
    /// El orden de despliegue de la categoría de producto.
    /// </summary>
    public int Orden { get; set; }

    /// <summary>
    /// Si la categoría de producto se tiene que presentar en la sección principal.
    /// </summary>
    public bool EsTop { get; set; }

    /// <summary>
    /// El nivel de la categoría de producto.
    /// </summary>
    public short Nivel { get; set; }

    /// <summary>
    /// Define si es que se presentan primero los productos antes que las subcategorías de producto.
    /// </summary>
    public bool PrimeroProductos { get; set; }

    /// <summary>
    /// El usuario que creó la categoría de producto.
    /// </summary>
    public UsuarioInfo UsuarioCreacion { get; set; } = default!;

    /// <summary>
    /// La fecha en que se creó la categoría de producto.
    /// </summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// El usuario que realizó la última actualización de la categoría de producto.
    /// </summary>
    public UsuarioInfo UsuarioModificacion { get; set; } = default!;

    /// <summary>
    /// La última fecha de modificación de la categoría de producto.
    /// </summary>
    public DateTime UltimaModificacion { get; set; }

    /// <summary>
    /// Las cajas asignadas a la categoría de producto.
    /// </summary>
    public IEnumerable<CategoriaPorCaja> CajasAsignadas { get; set; } = [];

    /// <summary>
    /// Los canales de venta asociados a la categoría de producto.
    /// </summary>
    public IEnumerable<CanalVenta> CanalesVenta { get; set; } = [];

    /// <summary>
    /// Las subcategorias de la categoría de producto.
    /// </summary>
    public IEnumerable<CategoriaProductoInfo> Hijos { get; set; } = [];

    //>internal string ToNombreCompleto() =>
    //>    Padre is not null ? $"{Padre.ToNombreCompleto()} > {Nombre}" : Nombre;
}
