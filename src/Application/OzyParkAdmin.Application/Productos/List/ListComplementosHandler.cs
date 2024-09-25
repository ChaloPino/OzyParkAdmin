using MassTransit.Mediator;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Application.Productos.List;

/// <summary>
/// El manejador de <see cref="ListComplementos"/>
/// </summary>
public sealed class ListComplementosHandler : MediatorRequestHandler<ListComplementos, ResultListOf<ProductoInfo>>
{
    private readonly IProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListComplementosHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IProductoRepository"/>.</param>
    public ListComplementosHandler(IProductoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<ProductoInfo>> Handle(ListComplementos request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListComplementosByCategoriaAsync(request.CategoriaId, cancellationToken);
    }
}
