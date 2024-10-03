using MassTransit.Mediator;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.DiasSemana.List;

/// <summary>
/// El manejador de <see cref="ListDiasSemana"/>.
/// </summary>
public sealed class ListDiasSemanaHandler : MediatorRequestHandler<ListDiasSemana, ResultListOf<DiaSemana>>
{
    private readonly IGenericRepository<DiaSemana> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListDiasSemanaHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/> de <see cref="DiaSemana"/>.</param>
    public ListDiasSemanaHandler(IGenericRepository<DiaSemana> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<DiaSemana>> Handle(ListDiasSemana request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        SortExpressionCollection<DiaSemana> sortExpressions = new SortExpressionCollection<DiaSemana>()
            .Add(x => x.Id, false);

        return await _repository.ListAsync(sortExpressions: sortExpressions, cancellationToken: cancellationToken);
    }
}
