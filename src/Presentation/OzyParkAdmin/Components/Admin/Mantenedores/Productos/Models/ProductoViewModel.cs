using Microsoft.CodeAnalysis;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Contabilidad;
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
    public int Id { get; set; }

    /// <summary>
    /// El aka del producto.
    /// </summary>
    public string Aka { get; set; } = string.Empty;

    /// <summary>
    /// El sku del producto.
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// El nombre del producto.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// El id de la franquicia.
    /// </summary>
    public int FranquiciaId { get; set; }

    /// <summary>
    /// El centro de costo asociado al producto.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; set; } = default!;

    /// <summary>
    /// La categoría asociada al producto.
    /// </summary>
    public CategoriaProductoInfo Categoria { get; set; } = default!;

    /// <summary>
    /// La categoría de despliegue asociada al producto.
    /// </summary>
    public CategoriaProductoInfo CategoriaDespliegue { get; set; } = default!;

    /// <summary>
    /// El catálogo de imagen asociado al producto.
    /// </summary>
    public CatalogoImagenModel Imagen { get; set; } = new();

    /// <summary>
    /// El tipo del producto.
    /// </summary>
    public TipoProducto TipoProducto { get; set; } = default!;

    /// <summary>
    /// El orden de despliegue del producto.
    /// </summary>
    public int Orden { get; set; } = 1;

    /// <summary>
    /// La agrupación contable de la familia.
    /// </summary>
    public AgrupacionContable Familia { get; set; } = default!;

    /// <summary>
    /// Si el producto es complemento.
    /// </summary>
    public bool EsComplemento { get; set; }

    /// <summary>
    /// Si el producto está en inventario.
    /// </summary>
    public bool EnInventario { get; set; }

    /// <summary>
    /// La fecha de alta del producto.
    /// </summary>
    public DateOnly FechaAlta { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    /// <summary>
    /// El usuario que creó el producto.
    /// </summary>
    public UsuarioInfo UsuarioCreacion { get; set; } = default!;

    /// <summary>
    /// La fecha en que se creó el producto.
    /// </summary>
    public DateTime FechaSistema { get; set; } = DateTime.Now;

    /// <summary>
    /// El usuario que realizó la última modificación del producto.
    /// </summary>
    public UsuarioInfo UsuarioModificacion { get; set; } = default!;

    /// <summary>
    /// La fecha en que se realizó la última modificación del producto.
    /// </summary>
    public DateTime UltimaModificacion { get; set; } = DateTime.Now;

    /// <summary>
    /// Si el producto está activo.
    /// </summary>
    public bool EsActivo { get; set; } = true;

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

    internal bool Loading { get; set; }

    /// <summary>
    /// Si el detalle del producto fue cargado.
    /// </summary>
    public bool DetailLoaded { get; set; }

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

    internal void Save(ProductoFullInfo producto)
    {
        if (IsNew)
        {
            Id = producto.Id;
            FranquiciaId = producto.FranquiciaId;
            CentroCosto = producto.CentroCosto;
            IsNew = false;
        }

        Aka = producto.Aka;
        Sku = producto.Sku;
        Nombre = producto.Nombre;
        FranquiciaId = producto.FranquiciaId;

        if (producto.Categoria is not null)
        {
            Categoria = producto.Categoria;
        }

        if (producto.CategoriaDespliegue is not null)
        {
            CategoriaDespliegue = producto.CategoriaDespliegue;
        }

        if (producto.Imagen is not null)
        {
            Imagen = producto.Imagen.ToModel();
        }

        if (producto.TipoProducto is not null)
        {
            TipoProducto = producto.TipoProducto;
        }

        Orden = producto.Orden;

        if (producto.Familia is not null)
        {
            Familia = producto.Familia;
        }

        EsComplemento = producto.EsComplemento;
        EnInventario = producto.EnInventario;
        FechaAlta = producto.FechaAlta;
        UsuarioCreacion = producto.UsuarioCreacion;
        FechaSistema = producto.FechaSistema;
        UsuarioModificacion = producto.UsuarioModificacion;
        UltimaModificacion = producto.UltimaModificacion;
        EsActivo = producto.EsActivo;
        Complementos = producto.Complementos.ToModel();
        Cajas = [.. producto.Cajas];
        Relacionados = producto.Relacionados.ToModel();
        Partes = producto.Partes.ToModel();
    }

    internal void Update(ProductoViewModel producto)
    {
        Id = producto.Id;
        FranquiciaId = producto.FranquiciaId;
        CentroCosto = producto.CentroCosto;
        IsNew = false;
        Aka = producto.Aka;
        Sku = producto.Sku;
        Nombre = producto.Nombre;
        FranquiciaId = producto.FranquiciaId;
        Categoria = producto.Categoria;
        CategoriaDespliegue = producto.CategoriaDespliegue;
        Imagen = producto.Imagen;
        TipoProducto = producto.TipoProducto;
        Orden = producto.Orden;
        Familia = producto.Familia;
        EsComplemento = producto.EsComplemento;
        EnInventario = producto.EnInventario;
        FechaAlta = producto.FechaAlta;
        UsuarioCreacion = producto.UsuarioCreacion;
        FechaSistema = producto.FechaSistema;
        UsuarioModificacion = producto.UsuarioModificacion;
        UltimaModificacion = producto.UltimaModificacion;
        EsActivo = producto.EsActivo;
        Cajas = [.. producto.Cajas];
        Complementos = producto.Complementos;
        Relacionados = producto.Relacionados;
        Partes = producto.Partes;
    }
}
