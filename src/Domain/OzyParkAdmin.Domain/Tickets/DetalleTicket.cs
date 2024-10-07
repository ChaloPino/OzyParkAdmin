using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Domain.Tickets;

/// <summary>
/// El detalle del ticket.
/// </summary>
public sealed class DetalleTicket
{
    /// <summary>
    /// El id del grupo etario.
    /// </summary>
    public int GrupoEtarioId { get; private init; }

    /// <summary>
    /// El grupo etario.
    /// </summary>
    public GrupoEtario GrupoEtario { get; private init; }

    /// <summary>
    /// La cantidad de pasajeros.
    /// </summary>
    public byte NumeroPasajeros { get; private init; }

}