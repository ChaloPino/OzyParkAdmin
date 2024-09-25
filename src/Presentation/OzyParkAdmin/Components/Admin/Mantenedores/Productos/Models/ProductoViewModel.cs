using Microsoft.AspNetCore.Components.Forms;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;

/// <summary>
/// View model de producto.
/// </summary>
public sealed record ProductoViewModel
{
    /// <summary>
    /// El id del producto.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// El aka del producto.
    /// </summary>
    public required string Aka { get; set; } = string.Empty;

    /// <summary>
    /// El sku del producto.
    /// </summary>
    public required string Sku { get; set; } = string.Empty;

    /// <summary>
    /// El nombre del producto.
    /// </summary>
    public required string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// El id de la franquicia.
    /// </summary>
    public required int FranquiciaId { get; set; }

    /// <summary>
    /// El centro de costo asociado al producto.
    /// </summary>
    public required CentroCostoInfo CentroCosto { get; set; }

    /// <summary>
    /// La categoría asociada al producto.
    /// </summary>
    public required CategoriaProductoInfo Categoria { get; set; }

    /// <summary>
    /// La categoría de despliegue asociada al producto.
    /// </summary>
    public required CategoriaProductoInfo CategoriaDespliegue { get; set; }

    /// <summary>
    /// El catálogo de imagen asociado al producto.
    /// </summary>
    public CatalogoImagenModel Imagen { get; set; } = new();

    /// <summary>
    /// El tipo del producto.
    /// </summary>
    public required TipoProducto TipoProducto { get; set; }

    /// <summary>
    /// El orden de despliegue del producto.
    /// </summary>
    public required int Orden { get; set; }

    /// <summary>
    /// La agrupación contable de la familia.
    /// </summary>
    public AgrupacionContable? Familia { get; set; }

    /// <summary>
    /// Si el producto es complemento.
    /// </summary>
    public required bool EsComplemento { get; set; }

    /// <summary>
    /// Si el producto está en inventario.
    /// </summary>
    public required bool EnInventario { get; set; }

    /// <summary>
    /// La fecha de alta del producto.
    /// </summary>
    public required DateOnly FechaAlta { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    /// <summary>
    /// El usuario que creó el producto.
    /// </summary>
    public required UsuarioInfo UsuarioCreacion { get; set; }

    /// <summary>
    /// La fecha en que se creó el producto.
    /// </summary>
    public required DateTime FechaSistema { get; set; } = DateTime.Now;

    /// <summary>
    /// El usuario que realizó la última modificación del producto.
    /// </summary>
    public required UsuarioInfo UsuarioModificacion { get; set; }

    /// <summary>
    /// La fecha en que se realizó la última modificación del producto.
    /// </summary>
    public required DateTime UltimaModificacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Si el producto está activo.
    /// </summary>
    public required bool EsActivo { get; set; } = true;

    /// <summary>
    /// Las cajas asignadas al producto.
    /// </summary>
    public List<CajaInfo> Cajas { get; set; } = [];

    /// <summary>
    /// Los complementos del producto.
    /// </summary>
    public List<ProductoComplementarioModel> Complementos { get; set; } = [];

    /// <summary>
    /// Los productos relacionados.
    /// </summary>
    public List<ProductoRelacionadoModel> Relacionados { get; set; } = [];

    /// <summary>
    /// Las partes del producto.
    /// </summary>
    public List<ProductoParteModel> Partes { get; set; } = [];

    /// <summary>
    /// Si el producto es nuevo.
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// La fecha de alta como <see cref="DateTime"/>.
    /// </summary>
    public DateTime? FechaAltaDate
    {
        get => FechaAlta.ToDateTime(TimeOnly.MinValue);
        set => FechaAlta = value is null
                ? DateOnly.MinValue
                : DateOnly.FromDateTime(value.Value);
    }
}
