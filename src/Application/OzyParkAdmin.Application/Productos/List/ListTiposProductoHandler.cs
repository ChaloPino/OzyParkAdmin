using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.List;

/// <summary>
/// El manejador de <see cref="ListTiposProducto"/>.
/// </summary>
public sealed class ListTiposProductoHandler : QueryListOfHandler<ListTiposProducto, TipoProducto>
{
    private readonly IGenericRepository<TipoProducto> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTiposProductoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/> para <see cref="TipoProducto"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListTiposProductoHandler(IGenericRepository<TipoProducto> repository, ILogger<ListTiposProductoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<TipoProducto>> ExecuteListAsync(ListTiposProducto query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        var sortExpressions = new SortExpressionCollection<TipoProducto>()
            .Add(x => x.Nombre, false);
        return await _repository.ListAsync(sortExpressions: sortExpressions, cancellationToken: cancellationToken);
    }
}
