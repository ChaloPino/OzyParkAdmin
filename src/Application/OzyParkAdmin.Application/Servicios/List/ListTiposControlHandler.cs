using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// El manejador de <see cref="ListTiposControl"/>.
/// </summary>
public sealed class ListTiposControlHandler : MediatorRequestHandler<ListTiposControl, ResultListOf<TipoControl>>
{
    private readonly IGenericRepository<TipoControl> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTiposControlHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/>.</param>
    public ListTiposControlHandler(IGenericRepository<TipoControl> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<TipoControl>> Handle(ListTiposControl request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListAsync(cancellationToken: cancellationToken);

    }
}
