using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Información de una zona por tramo de un servicio.
/// </summary>
public sealed record ZonaPorTramoInfo
{
    /// <summary>
    /// El tramo asociado.
    /// </summary>
    public TramoInfo Tramo { get; init; } = default!;

    /// <summary>
    /// La zona asociada.
    /// </summary>
    public ZonaInfo Zona { get; init; } = default!;

    /// <summary>
    /// Si la zona tramo es de retorno.
    /// </summary>
    public bool EsRetorno { get; init; }

    /// <summary>
    /// Si la zona tramo es de combinación.
    /// </summary>
    public bool EsCombinacion { get; init; }

    /// <summary>
    /// El orden de la zona tramo.
    /// </summary>
    public int Orden { get; init; }

    /// <summary>
    /// Si la zona tramo está activa.
    /// </summary>
    public bool EsActivo { get; set; }

    internal (int TramoId, int ZonaId, bool EsRetorno, bool EsCombinacion) Id => (Tramo.Id,  Zona.Id, EsRetorno, EsCombinacion);
}