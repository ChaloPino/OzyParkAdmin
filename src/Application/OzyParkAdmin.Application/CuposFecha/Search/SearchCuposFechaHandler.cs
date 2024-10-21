using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha.Search;

/// <summary>
/// El manejador de <see cref="SearchCuposFecha"/>.
/// </summary>
public sealed class SearchCuposFechaHandler : MediatorRequestHandler<SearchCuposFecha, PagedList<CupoFechaFullInfo>>
{
    private readonly ICupoFechaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchCuposFechaHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICupoFechaRepository"/>.</param>
    public SearchCuposFechaHandler(ICupoFechaRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<CupoFechaFullInfo>> Handle(SearchCuposFecha request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.SearchAsync(request.User.GetCentrosCosto(), request.SearchText, request.FilterExpressions, request.SortExpressions, request.Page, request.PageSize, cancellationToken);
    }
}
