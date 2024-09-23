using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Las zonas asociadas al servicio y al tramo.
/// </summary>
public class ZonaPorTramo
{
    /// <summary>
    /// El tramo asociado.
    /// </summary>
    public Tramo Tramo { get; private set; } = default!;

    /// <summary>
    /// La zona asociada.
    /// </summary>
    public Zona Zona { get; private set; } = default!;

    /// <summary>
    /// Si la zona por tramo es un retorno.
    /// </summary>
    public bool EsRetorno { get; private set; }

    /// <summary>
    /// Si la zona por tramo es una combinación.
    /// </summary>
    public bool EsCombinacion { get; private set; }

    /// <summary>
    /// El orden como se presenta la información.
    /// </summary>
    public int Orden { get; private set; }

    /// <summary>
    /// Si la zona por tramo está activa.
    /// </summary>
    public bool EsActivo { get; private set; }

    internal (int TramoId, int ZonaId, bool EsRetorno, bool EsCombinacion) Id => (Tramo.Id, Zona.Id, EsRetorno, EsCombinacion);

    internal static ZonaPorTramo Create(Tramo tramo, Zona zona, bool esRetorno, bool esCombinacion, int orden, bool esActivo) =>
        new() { Tramo = tramo, Zona = zona, EsRetorno = esRetorno, EsCombinacion = esCombinacion, Orden = orden, EsActivo = esActivo };

    internal void Update(bool esRetorno, bool esCombinacion, int orden, bool esActivo)
    {
        EsRetorno = esRetorno;
        EsCombinacion = esCombinacion;
        Orden = orden;
        EsActivo = esActivo;
    }
}