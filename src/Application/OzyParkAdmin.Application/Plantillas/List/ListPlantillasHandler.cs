using MassTransit.Mediator;
using OzyParkAdmin.Domain.Plantillas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Plantillas.List;

/// <summary>
/// El manejador de <see cref="ListPlantillas"/>.
/// </summary>
public sealed class ListPlantillasHandler : MediatorRequestHandler<ListPlantillas, ResultListOf<Plantilla>>
{
    private readonly IGenericRepository<Plantilla> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListPlantillasHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/> de plantillas.</param>
    public ListPlantillasHandler(IGenericRepository<Plantilla> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<Plantilla>> Handle(ListPlantillas request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListAsync(cancellationToken:  cancellationToken);
    }
}
