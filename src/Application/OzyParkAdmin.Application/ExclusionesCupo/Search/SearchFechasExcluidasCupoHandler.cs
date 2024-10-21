using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.ExclusionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.ExclusionesCupo.Search;

/// <summary>
/// El manejador de <see cref="SearchFechasExcluidasCupo"/>
/// </summary>
public sealed class SearchFechasExcluidasCupoHandler : MediatorRequestHandler<SearchFechasExcluidasCupo, PagedList<FechaExcluidaCupoFullInfo>>
{
    private readonly IFechaExcluidaCupoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchFechasExcluidasCupoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IFechaExcluidaCupoRepository"/>.</param>
    public SearchFechasExcluidasCupoHandler(IFechaExcluidaCupoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<FechaExcluidaCupoFullInfo>> Handle(SearchFechasExcluidasCupo request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.SearchAsync(
            request.User.GetCentrosCosto(),
            request.SearchText,
            request.FilterExpressions,
            request.SortExpressions,
            request.Page,
            request.PageSize,
            cancellationToken);
    }
}
