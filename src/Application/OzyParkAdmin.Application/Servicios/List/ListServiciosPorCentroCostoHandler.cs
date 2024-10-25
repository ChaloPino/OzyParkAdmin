using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// El manejador de <see cref="ListServiciosPorCentroCosto"/>.
/// </summary>
public sealed class ListServiciosPorCentroCostoHandler : QueryListOfHandler<ListServiciosPorCentroCosto, ServicioInfo>
{
    private readonly IServicioRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListServiciosPorCentroCostoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IServicioRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListServiciosPorCentroCostoHandler(IServicioRepository repository, ILogger<ListServiciosPorCentroCostoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<ServicioInfo>> ExecuteListAsync(ListServiciosPorCentroCosto query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListByCentroCostoAsync(query.CentroCostoId, cancellationToken);
    }
}
