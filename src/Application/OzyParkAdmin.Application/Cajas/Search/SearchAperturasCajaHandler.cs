using MassTransit.Mediator;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cajas.Search;

/// <summary>
/// El manejador de <see cref="SearchAperturasCaja"/>.
/// </summary>
public sealed class SearchAperturasCajaHandler : MediatorRequestHandler<SearchAperturasCaja, PagedList<AperturaCajaInfo>>
{
    private readonly ICajaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchAperturasCajaHandler"/>.
    /// </summary>
    /// <param name="repository"></param>
    public SearchAperturasCajaHandler(ICajaRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<AperturaCajaInfo>> Handle(SearchAperturasCaja request, CancellationToken cancellationToken) =>
        await _repository.SearchAperturaCajasAsync(request.CentroCostoId, request.SearchText, request.SearchDate, request.FilterExpressions, request.SortExpressions, request.Page, request.PageSize, cancellationToken);
}
