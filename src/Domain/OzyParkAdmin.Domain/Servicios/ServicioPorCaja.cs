using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// La caja asociada a un servicio.
/// </summary>
public sealed class ServicioPorCaja
{
    /// <summary>
    /// La caja asociada.
    /// </summary>
    public Caja Caja { get; private set; } = default!;

    /// <summary>
    /// Si no usa zona para las cajas móviles.
    /// </summary>
    public bool NoUsaZona { get; private set; }

    internal static ServicioPorCaja Create(Caja caja, bool noUsaZona) =>
        new() {  Caja = caja, NoUsaZona = noUsaZona };

    internal void Update(bool noUsaZona) =>
        NoUsaZona = noUsaZona;
}