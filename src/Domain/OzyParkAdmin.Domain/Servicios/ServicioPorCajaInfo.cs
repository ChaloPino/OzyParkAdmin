using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// La información de la asociación de un servicio en una caja.
/// </summary>
public sealed record ServicioPorCajaInfo
{
    /// <summary>
    /// La caja asociada.
    /// </summary>
    public CajaInfo Caja { get; init; } = default!;

    /// <summary>
    /// Si no usa zona para las ventas móviles.
    /// </summary>
    public bool NoUsaZona { get; init; }
}