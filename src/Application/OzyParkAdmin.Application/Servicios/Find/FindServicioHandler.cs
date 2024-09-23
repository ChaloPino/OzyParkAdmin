using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Find;

/// <summary>
/// El manejador de <see cref="FindServicio"/>.
/// </summary>
public sealed class FindServicioHandler : MediatorRequestHandler<FindServicio, ResultOf<ServicioFullInfo>>
{
    private readonly IServicioRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="FindServicioHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IServicioRepository"/>.</param>
    public FindServicioHandler(IServicioRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<ServicioFullInfo>> Handle(FindServicio request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        Servicio? servicio = await _repository.FindByIdAsync(request.ServicioId, cancellationToken);
        return servicio is null ? new NotFound() : servicio.ToInfo();
    }
}
