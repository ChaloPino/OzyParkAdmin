using MassTransit.Mediator;
using OzyParkAdmin.Domain.CategoriasProducto;

namespace OzyParkAdmin.Application.CategoriasProducto.List;

/// <summary>
/// El manejador de <see cref="ListCategoriasProducto"/>.
/// </summary>
public sealed class ListCategoriasProductoHandler : MediatorRequestHandler<ListCategoriasProducto, ResultListOf<CategoriaProductoInfo>>
{
    private readonly ICategoriaProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListCategoriasProductoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICategoriaProductoRepository"/>.</param>
    public ListCategoriasProductoHandler(ICategoriaProductoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<CategoriaProductoInfo>> Handle(ListCategoriasProducto request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListByFranquiciaIdAsync(request.FranquiciaId, TipoCategoria.Todas, cancellationToken);
    }
}
