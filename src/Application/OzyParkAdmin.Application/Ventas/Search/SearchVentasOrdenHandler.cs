using MassTransit.Mediator;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Ventas;

namespace OzyParkAdmin.Application.Ventas.Search;

/// <summary>
/// El manejador de <see cref="SearchVentasOrden"/>
/// </summary>
public sealed class SearchVentasOrdenHandler : MediatorRequestHandler<SearchVentasOrden, PagedList<VentaOrdenInfo>>
{
    private readonly IVentaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchVentasOrdenHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IVentaRepository"/></param>
    public SearchVentasOrdenHandler(IVentaRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }


    /// <inheritdoc/>
    protected override async Task<PagedList<VentaOrdenInfo>> Handle(SearchVentasOrden request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.SearchVentasOrdenAsync(
            request.Fecha,
            request.NumeroOrden,
            request.VentaId,
            request.TicketId,
            request.Email,
            request.Telefono,
            request.Nombres,
            request.Apellidos,
            request.SortExpressions,
            request.Page,
            request.PageSize,
            cancellationToken);
    }
}
