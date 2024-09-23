using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Search;

/// <summary>
/// El manejador de <see cref="SearchServicios"/>.
/// </summary>
public sealed class SearchServicioHandler : MediatorRequestHandler<SearchServicios, PagedList<ServicioFullInfo>>
{
    private readonly IServicioRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchServicioHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IServicioRepository"/>.</param>
    public SearchServicioHandler(IServicioRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<ServicioFullInfo>> Handle(SearchServicios request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.SearchServiciosAsync(request.User.GetCentrosCosto(), request.SearchText, request.FilterExpressions, request.SortExpressions, request.Page, request.PageSize, cancellationToken);
    }
}
