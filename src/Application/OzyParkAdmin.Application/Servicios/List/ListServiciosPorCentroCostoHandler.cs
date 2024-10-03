using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// El manejador de <see cref="ListServiciosPorCentroCosto"/>.
/// </summary>
public sealed class ListServiciosPorCentroCostoHandler : MediatorRequestHandler<ListServiciosPorCentroCosto, ResultListOf<ServicioInfo>>
{
    private readonly IServicioRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListServiciosPorCentroCostoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IServicioRepository"/>.</param>
    public ListServiciosPorCentroCostoHandler(IServicioRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }
    /// <inheritdoc/>
    protected override async Task<ResultListOf<ServicioInfo>> Handle(ListServiciosPorCentroCosto request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListByCentroCostoAsync(request.CentroCostoId, cancellationToken);
    }
}
