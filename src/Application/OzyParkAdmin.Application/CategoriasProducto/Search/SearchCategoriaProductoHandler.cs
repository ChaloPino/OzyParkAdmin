using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Productos.Search;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Application.CategoriasProducto.Search;

/// <summary>
/// El manejador de <see cref="SearchCategoriaProductoHandler"/>.
/// </summary>
public sealed class SearchCategoriaProductoHandler: QueryPagedOfHandler<SearchCategoriaProducto, CategoriaProductoFullInfo>
{
    private readonly ICategoriaProductoRepository _categoriaProductoRepository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchCategoriaProductoHandler"/>.
    /// </summary>
    /// <param name="categoriaProductoRepository">El <see cref="ICategoriaProductoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchCategoriaProductoHandler(ICategoriaProductoRepository categoriaProductoRepository, ILogger<SearchCategoriaProductoHandler> logger)
        :base(logger)
    {
        ArgumentNullException.ThrowIfNull(categoriaProductoRepository);
        _categoriaProductoRepository = categoriaProductoRepository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<CategoriaProductoFullInfo>> ExecutePagedListAsync(SearchCategoriaProducto query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _categoriaProductoRepository.SearchCategoriaProductoAsync(
            query.SearchText,
            query.FilterExpressions,
            query.SortExpressions,
            query.Page,
            query.PageSize,
            cancellationToken);
    }
}
