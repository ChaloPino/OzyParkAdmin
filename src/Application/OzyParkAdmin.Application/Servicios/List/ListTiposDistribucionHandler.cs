using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// Manejador de <see cref="ListTiposDistribucion"/>
/// </summary>
public sealed class ListTiposDistribucionHandler : MediatorRequestHandler<ListTiposDistribucion, ResultListOf<TipoDistribucion>>
{
    private readonly IGenericRepository<TipoDistribucion> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTiposDistribucionHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/>.</param>
    public ListTiposDistribucionHandler(IGenericRepository<TipoDistribucion> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<TipoDistribucion>> Handle(ListTiposDistribucion request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }
}
