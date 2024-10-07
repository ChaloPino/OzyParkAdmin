using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Ventas;

/// <summary>
/// El repositorio de <see cref="Venta"/>.
/// </summary>
public interface IVentaRepository
{
    /// <summary>
    /// Busca una venta dado su id.
    /// </summary>
    /// <param name="ventaId">El id de la venta a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La venta si existe.</returns>
    Task<Venta?> FindByIdAsync(string ventaId, CancellationToken cancellationToken);

    /// <summary>
    /// Busca las ventas realizadas con un orden que coincidan con los criterios de búqueda.
    /// </summary>
    /// <param name="fecha">La fecha de la búsqueda.</param>
    /// <param name="numeroOrden">El número de orden a buscar.</param>
    /// <param name="ventaId">El número de venta a buscar.</param>
    /// <param name="ticketId">El número de ticket a buscar.</param>
    /// <param name="email">La dirección de correo electrónico a buscar. </param>
    /// <param name="telefono">El número de teléfono a buscar.</param>
    /// <param name="nombres">El nombre del cliente a buscar.</param>
    /// <param name="apellidos">Los apellidos del cliente a buscar.</param>
    /// <param name="sortExpressions">Las expresiones de ordenamiento.</param>
    /// <param name="page">La página actual.</param>
    /// <param name="pageSize">El tamaño de la página actual.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Una lista paginada con los <see cref="VentaOrdenInfo"/> que coincidan con los criterios de búsqueda.</returns>
    Task<PagedList<VentaOrdenInfo>> SearchVentasOrdenAsync(DateTime fecha, string? numeroOrden, string? ventaId, string? ticketId, string? email, string? telefono, string? nombres, string? apellidos, SortExpressionCollection<VentaOrdenInfo> sortExpressions, int page, int pageSize, CancellationToken cancellationToken);
}
