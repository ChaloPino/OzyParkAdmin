using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Domain.CategoriasProducto;

/// <summary>
/// Define la categoría del producto.
/// </summary>
public sealed class CategoriaProducto
{
    private readonly List<CategoriaPorCaja> _cajasAsignadas = [];
    private readonly List<CanalVenta> _canalesVenta = [];
    private readonly List<CategoriaProducto> _hijos = [];

    /// <summary>
    /// El id de la categoría del producto.
    /// </summary>
    public int Id { get; private init; }

    /// <summary>
    /// El id de la franquicia.
    /// </summary>
    public int FranquiciaId { get; private init; }

    /// <summary>
    /// El aka de la categoria de producto.
    /// </summary>
    public string Aka { get; private set; } = string.Empty;

    /// <summary>
    /// El nombre de la categoría de producto.
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// Si la categoría de producto está activa.
    /// </summary>
    public bool EsActivo { get; private set; } = true;

    /// <summary>
    /// La categoría de producto padre.
    /// </summary>
    public CategoriaProducto? Padre { get; private set; }

    /// <summary>
    /// Si la categoría de producto es final y no tiene hijos.
    /// </summary>
    public bool EsFinal { get; private set; }

    /// <summary>
    /// La imágen asociada a la categoría de producto.
    /// </summary>
    public CatalogoImagen Imagen { get; set; } = default!;

    /// <summary>
    /// El orden de despliegue de la categoría de producto.
    /// </summary>
    public int Orden { get; private set; }

    /// <summary>
    /// Si la categoría de producto se tiene que presentar en la sección principal.
    /// </summary>
    public bool EsTop { get; private set; }

    /// <summary>
    /// El nivel de la categoría de producto.
    /// </summary>
    public short Nivel { get; private set; }

    /// <summary>
    /// Define si es que se presentan primero los productos antes que las subcategorías de producto.
    /// </summary>
    public bool PrimeroProductos { get; private set; }

    /// <summary>
    /// El usuario que creó la categoría de producto.
    /// </summary>
    public Usuario UsuarioCreacion { get; private set; } = default!;

    /// <summary>
    /// La fecha en que se creó la categoría de producto.
    /// </summary>
    public DateTime FechaCreacion { get; private set; } = DateTime.Now;

    /// <summary>
    /// El usuario que realizó la última actualización de la categoría de producto.
    /// </summary>
    public Usuario UsuarioModificacion { get; private set; } = default!;

    /// <summary>
    /// La última fecha de modificación de la categoría de producto.
    /// </summary>
    public DateTime UltimaModificacion { get; private set; } = DateTime.Now;

    /// <summary>
    /// Las cajas asignadas a la categoría de producto.
    /// </summary>
    public IEnumerable<CategoriaPorCaja> CajasAsignadas => _cajasAsignadas;

    /// <summary>
    /// Los canales de venta asociados a la categoría de producto.
    /// </summary>
    public IEnumerable<CanalVenta> CanalesVenta => _canalesVenta;

    /// <summary>
    /// Las subcategorias de la categoría de producto.
    /// </summary>
    public IEnumerable<CategoriaProducto> Hijos => _hijos;

    internal string ToNombreCompleto() =>
        Padre is not null ? $"{Padre.ToNombreCompleto()} > {Nombre}" : Nombre;
}