using MassTransit.Mediator;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.List;

/// <summary>
/// El manejador de <see cref="ListTiposProducto"/>.
/// </summary>
public sealed class ListTiposProductoHandler : MediatorRequestHandler<ListTiposProducto, ResultListOf<TipoProducto>>
{
    private readonly IGenericRepository<TipoProducto> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTiposProductoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/> para <see cref="TipoProducto"/>.</param>
    public ListTiposProductoHandler(IGenericRepository<TipoProducto> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<TipoProducto>> Handle(ListTiposProducto request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var sortExpressions = new SortExpressionCollection<TipoProducto>()
            .Add(x => x.Nombre, false);
        return await _repository.ListAsync(sortExpressions: sortExpressions, cancellationToken: cancellationToken);
    }
}
