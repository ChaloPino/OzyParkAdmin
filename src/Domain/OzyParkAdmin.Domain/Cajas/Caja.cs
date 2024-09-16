using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.PuntosVenta;

namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// Entidad caja.
/// </summary>
public sealed class Caja
{
    private readonly List<Gaveta> _gavetas = [];
    //private readonly List<CategoriaPorCaja> _categorias = [];
    /// <summary>
    /// Identificador único de la caja.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Franquicia a la que pertenece la caja.
    /// </summary>
    public Franquicia Franquicia { get; private set; } = default!;

    /// <summary>
    /// Centro de costo a la que pertenece la caja.
    /// </summary>
    public CentroCosto CentroCosto { get; private set; } = default!;

    /// <summary>
    /// Punto de venta al que pertenece la caja.
    /// </summary>
    public PuntoVenta PuntoVenta { get; private set; } = default!;

    /// <summary>
    /// Aka de la caka.
    /// </summary>
    public string Aka { get; private set; } = default!;

    /// <summary>
    /// Descripción de la caja.
    /// </summary>
    public string Descripcion { get; private set; } = default!;

    /// <summary>
    /// Equipo asociado a la caja.
    /// </summary>
    public string? Equipo { get; private set; }

    /// <summary>
    /// Si la caja está activa o no.
    /// </summary>
    public bool EsActivo { get; private set; }

    /// <summary>
    /// Si la caja soporta integración con POS.
    /// </summary>
    public bool EsIntegradoPOS { get; private set; }

    /// <summary>
    /// Puerto de integración con el POS.
    /// </summary>
    public string? PuertoIntegracionPOS { get; private set; }

    /// <summary>
    /// Si la caja tiene una bodega hija asociada.
    /// </summary>
    public int? BodegaHijoId { get; private set; }

    /// <summary>
    /// Si la caja es para un mall o no.
    /// </summary>
    public bool EsMall { get; private set; }

    /// <summary>
    /// Gavetas de la caja.
    /// </summary>
    public IEnumerable<Gaveta> Gavetas => _gavetas;

    ///// <summary>
    ///// Categorías de productos asociados a la caja.
    ///// </summary>
    //public IEnumerable<CategoriaPorCaja> Categorias => _categorias;

    /// <summary>
    /// Descripción completa de la caja.
    /// </summary>
    public string FullDescripcion => $"{Descripcion} ({Aka})";

    /// <summary>
    /// Crea una nueva caja.
    /// </summary>
    /// <param name="id">Identificador único.</param>
    /// <param name="franquicia">Franquicia.</param>
    /// <param name="centroCosto">Centro de costo.</param>
    /// <param name="puntoVenta">Punto de venta.</param>
    /// <param name="aka">Aka.</param>
    /// <param name="descripcion">Descripción.</param>
    /// <param name="equipo">Equipo.</param>
    /// <param name="esIntegradoPOS">Si está integrado con un POS.</param>
    /// <param name="puertoIntegracionPOS">Puerto de integración con el POS.</param>
    /// <param name="bodegaHijoId">Bodega hija asociada.</param>
    /// <param name="esMall">Si es para mall.</param>
    /// <returns>Una nueva <see cref="Caja"/>.</returns>
    public static Caja Crear(
        int id,
        Franquicia franquicia,
        CentroCosto centroCosto,
        PuntoVenta puntoVenta,
        string aka,
        string descripcion,
        string? equipo,
        bool esIntegradoPOS,
        string? puertoIntegracionPOS,
        int? bodegaHijoId,
        bool esMall) =>
        new()
        {
            Id = id,
            Franquicia = franquicia,
            CentroCosto = centroCosto,
            PuntoVenta = puntoVenta,
            Aka = aka,
            Descripcion = descripcion,
            Equipo = equipo,
            EsIntegradoPOS = esIntegradoPOS,
            PuertoIntegracionPOS = puertoIntegracionPOS,
            BodegaHijoId = bodegaHijoId,
            EsMall = esMall,
            EsActivo = true,
        };

    ///// <summary>
    ///// Asigna categorias a la caja.
    ///// </summary>
    ///// <param name="categorias">Lista de categorías que serán asociadas a la caja.</param>
    //public void AsignarCategorias(IEnumerable<ICategoriaOrdenInfo> categorias)
    //{
    //    if (Categorias == null)
    //    {
    //        Categorias = new HashSet<CategoriaPorCaja>();
    //    }

    //    if (categorias == null)
    //    {
    //        categorias = Enumerable.Empty<ICategoriaOrdenInfo>();
    //    }

    //    List<ICategoriaOrdenInfo> categoriasToAdd = (from categoria in categorias
    //                                                 join defPersisted in Categorias on categoria.CategoriaId equals defPersisted.CategoriaId into defCategorias
    //                                                 from persisted in defCategorias.DefaultIfEmpty()
    //                                                 where persisted == null
    //                                                 select categoria).ToList();

    //    List<CategoriaPorCaja> categoriasToRemove = (from persisted in Categorias
    //                                                 join defCategoria in categorias on persisted.CategoriaId equals defCategoria.CategoriaId into defCategorias
    //                                                 from categoria in defCategorias.DefaultIfEmpty()
    //                                                 where categoria == null
    //                                                 select persisted).ToList();

    //    List<KeyValuePair<CategoriaPorCaja, ICategoriaOrdenInfo>> categoriasToUpdate = (from persisted in Categorias
    //                                                                                    join categoria in categorias on persisted.CategoriaId equals categoria.CategoriaId
    //                                                                                    select new KeyValuePair<CategoriaPorCaja, ICategoriaOrdenInfo>(persisted, categoria))
    //                                                                                    .ToList();

    //    categoriasToAdd.ForEach(categoria => _categorias.Add(new CategoriaPorCaja(categoria.CategoriaId, this, categoria.Orden)));
    //    categoriasToRemove.ForEach(categoria => _categorias.Remove(categoria));
    //    categoriasToUpdate.ForEach(pair => pair.Key.CambiarOrden(pair.Value.Orden));
    //}
}
