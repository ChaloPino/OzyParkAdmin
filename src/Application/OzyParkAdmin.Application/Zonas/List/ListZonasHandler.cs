using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Application.Zonas.List;

/// <summary>
/// El manejador de <see cref="ListZonas"/>.
/// </summary>
public sealed class ListZonasHandler : QueryListOfHandler<ListZonas, ZonaInfo>
{
    private readonly IZonaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListZonasHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IZonaRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListZonasHandler(IZonaRepository repository, ILogger<ListZonasHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<ZonaInfo>> ExecuteListAsync(ListZonas query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListZonasAsync(cancellationToken);
    }
}
