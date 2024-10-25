using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CategoriasProducto;

namespace OzyParkAdmin.Application.CategoriasProducto.List;

/// <summary>
/// El manejador de <see cref="ListCategoriasProductoFinales"/>.
/// </summary>
public sealed class ListCategoriasProductoFinalesHandler : QueryListOfHandler<ListCategoriasProductoFinales, CategoriaProductoInfo>
{
    private readonly ICategoriaProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instnacia de <see cref="ListCategoriasProductoFinalesHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICategoriaProductoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListCategoriasProductoFinalesHandler(ICategoriaProductoRepository repository, ILogger<ListCategoriasProductoFinalesHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    /// <exception cref="NotImplementedException"></exception>
    protected override async Task<List<CategoriaProductoInfo>> ExecuteListAsync(ListCategoriasProductoFinales query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListByFranquiciaIdAsync(query.FranquiciaId, TipoCategoria.Finales, cancellationToken);
    }
}
