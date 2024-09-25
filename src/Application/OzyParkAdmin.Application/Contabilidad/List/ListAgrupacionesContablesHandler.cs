using MassTransit.Mediator;
using OzyParkAdmin.Domain.Contabilidad;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Contabilidad.List;

/// <summary>
/// El manejador de <see cref="ListAgrupacionesContables"/>.
/// </summary>
public sealed class ListAgrupacionesContablesHandler : MediatorRequestHandler<ListAgrupacionesContables, ResultListOf<AgrupacionContable>>
{
    private readonly IGenericRepository<AgrupacionContable> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListAgrupacionesContablesHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/> para <see cref="AgrupacionContable"/>.</param>
    public ListAgrupacionesContablesHandler(IGenericRepository<AgrupacionContable> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<AgrupacionContable>> Handle(ListAgrupacionesContables request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var sortExpressions = new SortExpressionCollection<AgrupacionContable>()
            .Add(x => x.Aka, false);

        return await _repository.ListAsync(sortExpressions: sortExpressions, cancellationToken: cancellationToken);
    }
}
