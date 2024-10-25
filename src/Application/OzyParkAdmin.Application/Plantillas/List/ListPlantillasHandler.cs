using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Plantillas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Plantillas.List;

/// <summary>
/// El manejador de <see cref="ListPlantillas"/>.
/// </summary>
public sealed class ListPlantillasHandler : QueryListOfHandler<ListPlantillas, Plantilla>
{
    private readonly IGenericRepository<Plantilla> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListPlantillasHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/> de plantillas.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListPlantillasHandler(IGenericRepository<Plantilla> repository, ILogger<ListPlantillasHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<Plantilla>> ExecuteListAsync(ListPlantillas query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListAsync(cancellationToken:  cancellationToken);
    }
}
