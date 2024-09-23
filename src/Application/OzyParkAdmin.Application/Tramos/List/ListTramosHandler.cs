using MassTransit.Mediator;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Application.Tramos.List;

/// <summary>
/// El manejador de <see cref="ListTramos"/>.
/// </summary>
public sealed class ListTramosHandler : MediatorRequestHandler<ListTramos, ResultListOf<TramoInfo>>
{
    private readonly ITramoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTramosHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ITramoRepository"/>.</param>
    public ListTramosHandler(ITramoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<TramoInfo>> Handle(ListTramos request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListTramosAsync(cancellationToken).ConfigureAwait(false);
    }
}
