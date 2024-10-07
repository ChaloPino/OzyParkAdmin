using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.Tickets;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.Tickets;

/// <summary>
/// El repositorio de <see cref="Ticket"/>.
/// </summary>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class TicketRepository(OzyParkAdminContext context) : Repository<Ticket>(context), ITicketRepository
{
    /// <inheritdoc/>
    public async Task<Ticket?> FindByIdAsync(string ticketId, CancellationToken cancellationToken) =>
        await EntitySet
            .AsSplitQuery()
            .Include(x => x.Servicio.CentroCosto)
            .Include(x => x.Tramo)
            .Include(x => x.TipoPasajero)
            .Include(x => x.ZonaOrigen)
            .Include(x => x.ZonaDestino)
            .Include(x => x.ZonaCupoOrigen)
            .Include(x => x.Sentido)
            .Include(x => x.TipoSegmenetacion)
            .Include(x => x.Usuario)
            .Include("Detalle.GrupoEtario")
            .FirstOrDefaultAsync(x => x.Id == ticketId, cancellationToken);
}
