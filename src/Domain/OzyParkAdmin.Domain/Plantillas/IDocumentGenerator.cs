using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Tickets;

namespace OzyParkAdmin.Domain.Plantillas;

/// <summary>
/// Generador de documentos basados en plantillas.
/// </summary>
public interface IDocumentGenerator
{
    /// <summary>
    /// Genera el documento de un ticket.
    /// </summary>
    /// <param name="ticket">El ticket usado para generar el documento.</param>
    /// <param name="ventaId">El id de la venta a la que está asociada el ticket.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El binario del documento del ticket.</returns>
    Task<ResultOf<byte[]>> GenerateTicketDocumentAsync(Ticket ticket, string ventaId, CancellationToken cancellationToken);
}
