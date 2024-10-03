using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cupos.Search;

/// <summary>
/// El manejador de <see cref="SearchCupos"/>.
/// </summary>
public sealed class SearchCuposHandler : MediatorRequestHandler<SearchCupos, PagedList<CupoFullInfo>>
{
    private readonly ICupoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchCuposHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICupoRepository"/>.</param>
    public SearchCuposHandler(ICupoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<CupoFullInfo>> Handle(SearchCupos request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.SearchAsync(request.User.GetCentrosCosto(), request.SearchText, request.FilterExpressions, request.SortExpressions, request.Page, request.PageSize, cancellationToken);
    }
}
