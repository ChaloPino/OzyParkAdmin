using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Application.Cajas.Find;

/// <summary>
/// El manejador de <see cref="FindAperturaCajaDetalle"/>.
/// </summary>
public sealed class FindAperturaCajaDetalleHandler : QueryHandler<FindAperturaCajaDetalle, AperturaCajaDetalleInfo>
{
    private readonly ICajaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="FindAperturaCajaDetalleHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICajaRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public FindAperturaCajaDetalleHandler(ICajaRepository repository, ILogger<FindAperturaCajaDetalleHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<AperturaCajaDetalleInfo?> ExecuteQueryAsync(FindAperturaCajaDetalle query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.FindAperturaCajaDetalleAsync(query.AperturaCajaId, cancellationToken);
    }
}
