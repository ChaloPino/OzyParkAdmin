using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// Manejador de <see cref="ListTiposDistribucion"/>
/// </summary>
public sealed class ListTiposDistribucionHandler : QueryListOfHandler<ListTiposDistribucion, TipoDistribucion>
{
    private readonly IGenericRepository<TipoDistribucion> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTiposDistribucionHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListTiposDistribucionHandler(IGenericRepository<TipoDistribucion> repository, ILogger<ListTiposDistribucionHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<TipoDistribucion>> ExecuteListAsync(ListTiposDistribucion query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }
}
