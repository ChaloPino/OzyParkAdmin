using MassTransit.Mediator;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cajas.Find;

/// <summary>
/// El manejador de <see cref="FindAperturaCajaDetalle"/>.
/// </summary>
public sealed class FindAperturaCajaDetalleHandler : MediatorRequestHandler<FindAperturaCajaDetalle, ResultOf<AperturaCajaDetalleInfo>>
{
    private readonly ICajaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="FindAperturaCajaDetalleHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICajaRepository"/>.</param>
    public FindAperturaCajaDetalleHandler(ICajaRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<AperturaCajaDetalleInfo>> Handle(FindAperturaCajaDetalle request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        AperturaCajaDetalleInfo? detalle = await _repository.FindAperturaCajaDetalleAsync(request.AperturaCajaId, cancellationToken);
        return detalle is null ? new NotFound() : detalle;

    }
}
