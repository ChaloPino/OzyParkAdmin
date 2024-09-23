using MassTransit.Mediator;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Application.Zonas.List;

/// <summary>
/// El manejador de <see cref="ListZonas"/>.
/// </summary>
public sealed class ListZonasHandler : MediatorRequestHandler<ListZonas, ResultListOf<ZonaInfo>>
{
    private readonly IZonaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListZonasHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IZonaRepository"/>.</param>
    public ListZonasHandler(IZonaRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<ZonaInfo>> Handle(ListZonas request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListZonasAsync(cancellationToken);
    }
}
