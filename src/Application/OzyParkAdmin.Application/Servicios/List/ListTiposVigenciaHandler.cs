using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// El manejador de <see cref="ListTiposVigencia"/>.
/// </summary>
public sealed class ListTiposVigenciaHandler : QueryListOfHandler<ListTiposVigencia, TipoVigencia>
{
    private readonly IGenericRepository<TipoVigencia> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTiposVigenciaHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListTiposVigenciaHandler(IGenericRepository<TipoVigencia> repository, ILogger<ListTiposVigenciaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<TipoVigencia>> ExecuteListAsync(ListTiposVigencia query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }
}
