using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.Find;

/// <summary>
/// El manejador de <see cref="FindServicio"/>.
/// </summary>
public sealed class FindServicioHandler : QueryHandler<FindServicio, ServicioFullInfo>
{
    private readonly IServicioRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="FindServicioHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IServicioRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public FindServicioHandler(IServicioRepository repository, ILogger<FindServicioHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ServicioFullInfo?> ExecuteQueryAsync(FindServicio query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        Servicio? servicio = await _repository.FindByIdAsync(query.ServicioId, cancellationToken);
        return servicio?.ToFullInfo();
    }
}
