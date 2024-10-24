using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CategoriasProducto;

namespace OzyParkAdmin.Application.CategoriasProducto.List;

/// <summary>
/// El manejador de <see cref="ListCategoriasProducto"/>.
/// </summary>
public sealed class ListCategoriasProductoHandler : QueryListOfHandler<ListCategoriasProducto, CategoriaProductoInfo>
{
    private readonly ICategoriaProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListCategoriasProductoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICategoriaProductoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListCategoriasProductoHandler(ICategoriaProductoRepository repository, ILogger<ListCategoriasProductoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<CategoriaProductoInfo>> ExecuteListAsync(ListCategoriasProducto query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListByFranquiciaIdAsync(query.FranquiciaId, TipoCategoria.Todas, cancellationToken);
    }
}
