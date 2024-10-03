using MassTransit.Mediator;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CanalesVenta.List;

/// <summary>
/// El manejador de <see cref="ListCanalesVenta"/>.
/// </summary>
public sealed class ListCanalesVentaHandler : MediatorRequestHandler<ListCanalesVenta, ResultListOf<CanalVenta>>
{
    private readonly IGenericRepository<CanalVenta> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListCanalesVentaHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/> de <see cref="CanalVenta"/>.</param>
    public ListCanalesVentaHandler(IGenericRepository<CanalVenta> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<CanalVenta>> Handle(ListCanalesVenta request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        SortExpressionCollection<CanalVenta> sortExpressions = new SortExpressionCollection<CanalVenta>()
            .Add(x => x.Nombre, false);
        return await _repository.ListAsync(sortExpressions: sortExpressions, cancellationToken: cancellationToken);
    }
}
