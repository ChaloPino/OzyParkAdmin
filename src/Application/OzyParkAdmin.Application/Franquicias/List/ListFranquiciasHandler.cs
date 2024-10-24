using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Franquicias;

namespace OzyParkAdmin.Application.Franquicias.List;

/// <summary>
/// El manejador de <see cref="ListFranquicias"/>.
/// </summary>
public sealed class ListFranquiciasHandler : QueryListOfHandler<ListFranquicias, FranquiciaInfo>
{
    private readonly IFranquiciaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListFranquiciasHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IFranquiciaRepository"/></param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListFranquiciasHandler(IFranquiciaRepository repository, ILogger<ListFranquiciasHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<FranquiciaInfo>> ExecuteListAsync(ListFranquicias query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        int[]? franquiciaIds = query.User.GetFranquicias();
        List<Franquicia> franquicias = await _repository.ListFranquiciasAsync(franquiciaIds, cancellationToken);
        return franquicias.ToInfo();
    }
}
