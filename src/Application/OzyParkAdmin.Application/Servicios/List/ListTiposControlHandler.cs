using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// El manejador de <see cref="ListTiposControl"/>.
/// </summary>
public sealed class ListTiposControlHandler : QueryListOfHandler<ListTiposControl, TipoControl>
{
    private readonly IGenericRepository<TipoControl> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTiposControlHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListTiposControlHandler(IGenericRepository<TipoControl> repository, ILogger<ListTiposControlHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<TipoControl>> ExecuteListAsync(ListTiposControl query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }
}
