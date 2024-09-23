using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// El manejador de <see cref="ListServicios"/>.
/// </summary>
public sealed class ListServiciosHandler : MediatorRequestHandler<ListServicios, ResultListOf<ServicioInfo>>
{
    private readonly IServicioRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListServiciosHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IServicioRepository"/>.</param>
    public ListServiciosHandler(IServicioRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<ServicioInfo>> Handle(ListServicios request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListAsync(request.FranquiciaId, cancellationToken);
    }
}
