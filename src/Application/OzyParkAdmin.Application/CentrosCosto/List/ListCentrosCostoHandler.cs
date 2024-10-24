using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CentrosCosto;

namespace OzyParkAdmin.Application.CentrosCosto.List;

/// <summary>
/// El handler de <see cref="ListCentrosCosto"/>.
/// </summary>
public sealed class ListCentrosCostoHandler : QueryListOfHandler<ListCentrosCosto, CentroCostoInfo>
{
    private readonly ICentroCostoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListCentrosCostoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICentroCostoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListCentrosCostoHandler(ICentroCostoRepository repository, ILogger<ListCentrosCostoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<CentroCostoInfo>> ExecuteListAsync(ListCentrosCosto query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListCentrosCostoAsync(query.User.GetCentrosCosto(), cancellationToken);
    }
}
