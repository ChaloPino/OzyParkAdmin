using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Application.EscenariosCupo.List;

/// <summary>
/// El manejador de <see cref="ListEscenariosCupo"/>.
/// </summary>
public sealed class ListEscenariosCupoHandler : QueryListOfHandler<ListEscenariosCupo, EscenarioCupoInfo>
{
    private readonly IEscenarioCupoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListEscenariosCupoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IEscenarioCupoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListEscenariosCupoHandler(IEscenarioCupoRepository repository, ILogger<ListEscenariosCupoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<EscenarioCupoInfo>> ExecuteListAsync(ListEscenariosCupo query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListAsync(query.User.GetCentrosCosto(), cancellationToken);
    }
}
