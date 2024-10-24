using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.DiasSemana.List;

/// <summary>
/// El manejador de <see cref="ListDiasSemana"/>.
/// </summary>
public sealed class ListDiasSemanaHandler : QueryListOfHandler<ListDiasSemana, DiaSemana>
{
    private readonly IGenericRepository<DiaSemana> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListDiasSemanaHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/> de <see cref="DiaSemana"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListDiasSemanaHandler(IGenericRepository<DiaSemana> repository, ILogger<ListDiasSemanaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<DiaSemana>> ExecuteListAsync(ListDiasSemana query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        SortExpressionCollection<DiaSemana> sortExpressions = new SortExpressionCollection<DiaSemana>()
            .Add(x => x.Id, false);

        return await _repository.ListAsync(sortExpressions: sortExpressions, cancellationToken: cancellationToken);
    }
}
