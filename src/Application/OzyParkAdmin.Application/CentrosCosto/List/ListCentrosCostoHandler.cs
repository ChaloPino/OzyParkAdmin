using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.CentrosCosto;

namespace OzyParkAdmin.Application.CentrosCosto.List;

/// <summary>
/// El handler de <see cref="ListCentrosCosto"/>.
/// </summary>
public sealed class ListCentrosCostoHandler : MediatorRequestHandler<ListCentrosCosto, ResultListOf<CentroCostoInfo>>
{
    private readonly ICentroCostoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListCentrosCostoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICentroCostoRepository"/>.</param>
    public ListCentrosCostoHandler(ICentroCostoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<CentroCostoInfo>> Handle(ListCentrosCosto request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListCentrosCostoAsync(request.User.GetCentrosCosto(), cancellationToken);
    }
}
