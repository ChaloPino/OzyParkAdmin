using MassTransit.Mediator;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Ventas;

namespace OzyParkAdmin.Application.Ventas.Search;

/// <summary>
/// Busca todas las ventas con orden que coincidan con los criterios de búsqueda.
/// </summary>
/// <param name="Fecha">La fecha que se quiere buscar.</param>
/// <param name="NumeroOrden">El número de orden que se quiere buscar.</param>
/// <param name="VentaId">El número de venta que se quiere buscar.</param>
/// <param name="TicketId">El número de ticket que se quiere buscar.</param>
/// <param name="Email">La dirección de correo electrónico que se quiere buscar.</param>
/// <param name="Telefono">El número de teléfono que se quiere buscar.</param>
/// <param name="Nombres">El nombre del cliente que se quiere buscar.</param>
/// <param name="Apellidos">Los apellidos del cliente que se quiere buscar.</param>
/// <param name="SortExpressions">Las expresiones de ordenamiento.</param>
/// <param name="Page">La página actual.</param>
/// <param name="PageSize">El tamaño de la página.</param>
public sealed record SearchVentasOrden(DateTime Fecha, string? NumeroOrden, string? VentaId, string? TicketId, string? Email, string? Telefono, string? Nombres, string? Apellidos, SortExpressionCollection<VentaOrdenInfo> SortExpressions, int Page, int PageSize) : Request<PagedList<VentaOrdenInfo>>;
