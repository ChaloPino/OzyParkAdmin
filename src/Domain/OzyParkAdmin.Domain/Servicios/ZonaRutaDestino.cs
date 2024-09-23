using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El destino de una zona ruta.
/// </summary>
public class ZonaRutaDestino
{
    /// <summary>
    /// La zona destino.
    /// </summary>
    public Zona ZonaDestino { get; private set; } = default!;

    /// <summary>
    /// Si el destino está activo.
    /// </summary>
    public bool EsActivo { get; set; }
}