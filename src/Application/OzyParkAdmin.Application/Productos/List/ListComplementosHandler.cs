using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Application.Productos.List;

/// <summary>
/// El manejador de <see cref="ListComplementos"/>
/// </summary>
public sealed class ListComplementosHandler : QueryListOfHandler<ListComplementos, ProductoInfo>
{
    private readonly IProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListComplementosHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IProductoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListComplementosHandler(IProductoRepository repository, ILogger<ListComplementosHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<ProductoInfo>> ExecuteListAsync(ListComplementos query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListComplementosByCategoriaAsync(query.CategoriaId, query.ExceptoProductoId, cancellationToken);
    }
}
