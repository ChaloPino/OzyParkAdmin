namespace OzyParkAdmin.Components.Admin.Ventas.Models;

/// <summary>
/// El modelo del ticket descargado.
/// </summary>
/// <param name="TicketId">El id del ticket.</param>
/// <param name="Stream">El stream del contenido del documento.</param>
/// <param name="ContentType">El tipo de contenido.</param>
public sealed record DownloadedTicketModel(string TicketId, Stream Stream, string ContentType);
