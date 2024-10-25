using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Tramos;

namespace OzyParkAdmin.Application.Tramos.List;

/// <summary>
/// El manejador de <see cref="ListTramos"/>.
/// </summary>
public sealed class ListTramosHandler : QueryListOfHandler<ListTramos, TramoInfo>
{
    private readonly ITramoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTramosHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ITramoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListTramosHandler(ITramoRepository repository, ILogger<ListTramosHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<TramoInfo>> ExecuteListAsync(ListTramos query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListTramosAsync(cancellationToken).ConfigureAwait(false);
    }
}
