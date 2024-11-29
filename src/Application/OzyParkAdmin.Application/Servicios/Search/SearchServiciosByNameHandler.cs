using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.Search;

/// <summary>
/// El manejador de <see cref="SearchServiciosByName"/>.
/// </summary>
public sealed class SearchServiciosByNameHandler : QueryListOfHandler<SearchServiciosByName, ServicioWithDetailInfo>
{
    private readonly IServicioRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchServiciosByNameHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IServicioRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchServiciosByNameHandler(IServicioRepository repository, ILogger<SearchServiciosByNameHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<ServicioWithDetailInfo>> ExecuteListAsync(SearchServiciosByName query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.SearchByCentroCostoAsync(query.CentroCostoId, query.SearchText, cancellationToken);
    }
}
