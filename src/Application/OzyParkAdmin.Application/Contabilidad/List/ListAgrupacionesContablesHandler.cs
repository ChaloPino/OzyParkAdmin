using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Contabilidad;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Contabilidad.List;

/// <summary>
/// El manejador de <see cref="ListAgrupacionesContables"/>.
/// </summary>
public sealed class ListAgrupacionesContablesHandler : QueryListOfHandler<ListAgrupacionesContables, AgrupacionContable>
{
    private readonly IGenericRepository<AgrupacionContable> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListAgrupacionesContablesHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/> para <see cref="AgrupacionContable"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListAgrupacionesContablesHandler(IGenericRepository<AgrupacionContable> repository, ILogger<ListAgrupacionesContablesHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<AgrupacionContable>> ExecuteListAsync(ListAgrupacionesContables query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var sortExpressions = new SortExpressionCollection<AgrupacionContable>()
            .Add(x => x.Aka, false);

        return await _repository.ListAsync(sortExpressions: sortExpressions, cancellationToken: cancellationToken);
    }
}
