using MassTransit.Mediator;
using OzyParkAdmin.Domain.OmisionesCupo;
using OzyParkAdmin.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Application.OmisionesCupo.Search;

/// <summary>
/// El manejador de <see cref="SearchOmisionesEscenarioCupoExlusion"/>.
/// </summary>
public sealed class SearchOmisionesEscenarioCupoExlusionHandler : MediatorRequestHandler<SearchOmisionesEscenarioCupoExlusion, PagedList<IgnoraEscenarioCupoExclusionFullInfo>>
{
    private readonly IIgnoraEscenarioCupoExclusionRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="IIgnoraEscenarioCupoExclusionRepository"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IIgnoraEscenarioCupoExclusionRepository"/>.</param>
    public SearchOmisionesEscenarioCupoExlusionHandler(IIgnoraEscenarioCupoExclusionRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<IgnoraEscenarioCupoExclusionFullInfo>> Handle(SearchOmisionesEscenarioCupoExlusion request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await _repository.SearchAsync(
            request.SearchText,
            request.FilterExpressions,
            request.SortExpressions,
            request.Page,
            request.PageSize,
            cancellationToken);
    }
}
