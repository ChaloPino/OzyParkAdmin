using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Monedas.List;

/// <summary>
/// El manejador de <see cref="ListMonedas"/>.
/// </summary>
public sealed class ListMonedasHandler : QueryListOfHandler<ListMonedas, Moneda>
{
    private readonly IGenericRepository<Moneda> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListMonedasHandler"/>.
    /// </summary>
    /// <param name="repository">El repositorio de <see cref="Moneda"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListMonedasHandler(IGenericRepository<Moneda> repository, ILogger<ListMonedasHandler> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<Moneda>> ExecuteListAsync(ListMonedas query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }
}
