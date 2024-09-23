using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// El manejador de <see cref="ListTiposVigencia"/>.
/// </summary>
public sealed class ListTiposVigenciaHandler : MediatorRequestHandler<ListTiposVigencia, ResultListOf<TipoVigencia>>
{
    private readonly IGenericRepository<TipoVigencia> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTiposVigenciaHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/>.</param>
    public ListTiposVigenciaHandler(IGenericRepository<TipoVigencia> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<TipoVigencia>> Handle(ListTiposVigencia request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }
}
