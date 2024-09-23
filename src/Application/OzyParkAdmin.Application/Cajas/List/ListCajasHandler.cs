using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Application.Cajas.List;

/// <summary>
/// El manejador de <see cref="ListCajas"/>.
/// </summary>
public sealed class ListCajasHandler : MediatorRequestHandler<ListCajas, ResultListOf<CajaInfo>>
{
    private readonly ICajaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListCajasHandler"/>.
    /// </summary>
    /// <param name="repostitory">El <see cref="ICajaRepository"/>.</param>
    public ListCajasHandler(ICajaRepository repostitory)
    {
        ArgumentNullException.ThrowIfNull(repostitory);
        _repository = repostitory;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<CajaInfo>> Handle(ListCajas request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListCajasAsync(request.User.GetCentrosCosto(), cancellationToken);
    }
}
