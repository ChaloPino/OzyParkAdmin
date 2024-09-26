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
/// La entidad producto.
/// </summary>
public sealed class Producto
{
    private ProductoAgrupacion _productoAgrupacion = default!;
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
    public AgrupacionContable Familia => _productoAgrupacion.AgrupacionContable;

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

    internal static ResultOf<Producto> Create(
        int id,
        string aka,
        string sku,
        string nombre,
        int franquiciaId,
        CentroCosto centroCosto,
        CategoriaProducto categoria,
        CategoriaProducto categoriaDespliegue,
        CatalogoImagen imagen,
        TipoProducto tipoProducto,
        AgrupacionContable familia,
        int orden,
        bool esComplemento,
        DateOnly fechaAlta,
        Usuario usuarioCreacion,
        IEnumerable<(ProductoComplementarioInfo Info, Producto Producto)> complementos)
    {
        Producto producto = new()
        {
            Id = id,
            Aka = aka,
            Sku = sku,
            Nombre = nombre,
            FranquiciaId = franquiciaId,
            CentroCosto = centroCosto,
            Categoria = categoria,
            CategoriaDespliegue = categoriaDespliegue,
            TipoProducto = tipoProducto,
            Orden = orden,
            EsComplemento = esComplemento,
            FechaAlta = fechaAlta,
            UsuarioCreacion = usuarioCreacion,
            UsuarioModificacion = usuarioCreacion,
            Imagen = imagen,
        };

        producto.AssignFamilia(familia);
        producto.AssignComplementos(complementos);
        return producto;
    }

    internal ResultOf<Producto> Update(
        string aka,
        string sku,
        string nombre,
        CategoriaProducto categoria,
        CategoriaProducto categoriaDespliegue,
        CatalogoImagen imagen,
        TipoProducto tipoProducto,
        AgrupacionContable familia,
        int orden,
        bool esComplemento,
        DateOnly fechaAlta,
        Usuario usuarioCreacion,
        IEnumerable<(ProductoComplementarioInfo Info, Producto Producto)> complementos)
    {
        Aka = aka;
        Sku = sku;
        Nombre = nombre;
        Categoria = categoria;
        CategoriaDespliegue = categoriaDespliegue;
        TipoProducto = tipoProducto;
        Orden = orden;
        EsComplemento = esComplemento;
        FechaAlta = fechaAlta;
        UltimaModificacion = DateTime.Now;
        UsuarioModificacion = usuarioCreacion;
        Imagen = imagen;

        AssignFamilia(familia);
        AssignComplementos(complementos);
        return this;
    }

    internal void Activar() =>
        EsActivo = true;

    internal void Desactivar() =>
        EsActivo = false;

    internal void Bloquear() =>
        EnInventario = true;

    internal void Desbloquear() =>
        EnInventario = false;

    private void AssignFamilia(AgrupacionContable familia)
    {
        if (_productoAgrupacion is not null && _productoAgrupacion.AgrupacionContable != familia)
        {
            _productoAgrupacion.Update(familia);
            return;
        }

        _productoAgrupacion = ProductoAgrupacion.Create(familia);
    }

    private void AssignComplementos(IEnumerable<(ProductoComplementarioInfo Info, Producto Producto)> complementos)
    {
        var toAdd = (from complemento in complementos
                     join persisted in _complementos on complemento.Producto.Id equals persisted.Complemento.Id into productoComplementos
                     from productoComplemento in productoComplementos.DefaultIfEmpty()
                     where productoComplemento is null
                     select (complemento.Producto, complemento.Info.Orden)).ToList();

        var toRemove = (from persisted in _complementos
                        join complemento in complementos on persisted.Complemento.Id equals complemento.Producto.Id into productoComplementos
                        from productoComplemento in productoComplementos.DefaultIfEmpty()
                        where productoComplemento.Producto is null
                        select persisted).ToList();

        var toUpdate = (from persisted in _complementos
                        join complemento in complementos on persisted.Complemento.Id equals complemento.Producto.Id
                        select (persisted, complemento.Info.Orden)).ToList();

        toAdd.ForEach(AddComplemento);
        toRemove.ForEach(RemoveComplemento);
        toUpdate.ForEach(UpdateComplemento);
    }

    private void AddComplemento((Producto Producto, int Orden) complemento) =>
        _complementos.Add(ProductoComplementario.Create(complemento.Producto, complemento.Orden));

    private void RemoveComplemento(ProductoComplementario complementario) =>
        _complementos.Remove(complementario);

    private static void UpdateComplemento((ProductoComplementario Complementario, int Orden) info) =>
        info.Complementario.Update(info.Orden);

    internal ResultOf<Producto> AssignCajas(IEnumerable<Caja> cajasToAssign)
    {
        List<Caja> toAdd = (from cajaToAssign in cajasToAssign
                            join persited in _cajas on cajaToAssign.Id equals persited.Id into cajas
                            from caja in cajas.DefaultIfEmpty()
                            where caja is null
                            select cajaToAssign).ToList();

        List<Caja> toRemove = (from cajaToRemove in _cajas
                               join cajaToAssign in cajasToAssign on cajaToRemove.Id equals cajaToAssign.Id into cajas
                               from caja in cajas.DefaultIfEmpty()
                               where caja is null
                               select cajaToRemove).ToList();

        toAdd.ForEach(AddCaja);
        toRemove.ForEach(RemoveCaja);

        return this;
    }

    private void AddCaja(Caja caja) =>
        _cajas.Add(caja);

    private void RemoveCaja(Caja caja) =>
        _cajas.Remove(caja);

    internal ResultOf<Producto> AssignPartes(IEnumerable<(ProductoParteInfo Info, Producto Producto)> partes)
    {
        var toAdd = (from parteToAssign in partes
                     join persisted in _partes on parteToAssign.Producto.Id equals persisted.Parte.Id into defPartes
                     from parte in defPartes.DefaultIfEmpty()
                     where parte is null
                     select (parteToAssign.Producto, parteToAssign.Info.Cantidad, parteToAssign.Info.EsOpcional)).ToList();

        var toRemove = (from parteToRemove in _partes
                        join parteToAssign in partes on parteToRemove.Parte.Id equals parteToAssign.Producto.Id into defPartes
                        from parte in defPartes.DefaultIfEmpty()
                        where parte.Producto is null
                        select parteToRemove).ToList();

        var toUpdate = (from parteToUpdate in _partes
                        join parteToAssign in partes on parteToUpdate.Parte.Id equals parteToAssign.Producto.Id
                        select (parteToUpdate, parteToAssign.Info.Cantidad, parteToAssign.Info.EsOpcional)).ToList();

        toAdd.ForEach(AddParte);
        toRemove.ForEach(RemoveParte);
        toUpdate.ForEach(UpdateParte);
        return this;
    }

    private void AddParte((Producto Parte, decimal Cantidad, bool EsOpcional) parteToAdd) =>
        _partes.Add(ProductoParte.Create(parteToAdd.Parte, parteToAdd.Cantidad, parteToAdd.EsOpcional));

    private void RemoveParte(ProductoParte parteToRemove) =>
        _partes.Remove(parteToRemove);

    private static void UpdateParte((ProductoParte Parte, decimal Cantidad, bool EsOpcional) parteToUpdate) =>
        parteToUpdate.Parte.Update(parteToUpdate.Cantidad, parteToUpdate.EsOpcional);
}
