using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CanalesVenta.List;

/// <summary>
/// El manejador de <see cref="ListCanalesVenta"/>.
/// </summary>
public sealed class ListCanalesVentaHandler : QueryListOfHandler<ListCanalesVenta, CanalVenta>
{
    private readonly IGenericRepository<CanalVenta> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListCanalesVentaHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/> de <see cref="CanalVenta"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListCanalesVentaHandler(IGenericRepository<CanalVenta> repository, ILogger<ListCanalesVentaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<CanalVenta>> ExecuteListAsync(ListCanalesVenta query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        SortExpressionCollection<CanalVenta> sortExpressions = new SortExpressionCollection<CanalVenta>()
            .Add(x => x.Nombre, false);
        return await _repository.ListAsync(sortExpressions: sortExpressions, cancellationToken: cancellationToken);
    }
}
