using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Productos.Find;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CategoriasProducto;

namespace OzyParkAdmin.Application.CategoriasProducto.Find;

/// <summary>
/// El manejador de <see cref="FindCategoriaProducto" />
/// </summary>
public sealed class FindCategoriaProductoHandler : QueryHandler<FindCategoriaProducto, CategoriaProductoFullInfo>
{
    private readonly ICategoriaProductoRepository _categoriaProductoRepository;

    /// <summary>
    /// Crea una instancia de <see cref="FindCategoriaProductoHandler" />
    /// </summary>
    /// <param name="categoriaProductoRepository"></param>
    /// <param name="logger"></param>
    public FindCategoriaProductoHandler(ICategoriaProductoRepository categoriaProductoRepository, ILogger<FindCategoriaProductoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(categoriaProductoRepository);
        _categoriaProductoRepository = categoriaProductoRepository;
    }

    /// <inheritdoc/>
    protected override async Task<CategoriaProductoFullInfo?> ExecuteQueryAsync(FindCategoriaProducto query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        CategoriaProducto? categoriaProducto = await _categoriaProductoRepository.FindByIdAsync(query.categoriaProductoId, cancellationToken);
        return categoriaProducto?.ToFullInfo();
    }
}
