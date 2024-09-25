using MassTransit.Mediator;
using OzyParkAdmin.Domain.CategoriasProducto;

namespace OzyParkAdmin.Application.CategoriasProducto.List;

/// <summary>
/// El manejador de <see cref="ListCategoriasProductoFinales"/>.
/// </summary>
public sealed class ListCategoriasProductoFinalesHandler : MediatorRequestHandler<ListCategoriasProductoFinales, ResultListOf<CategoriaProductoInfo>>
{
    private readonly ICategoriaProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instnacia de <see cref="ListCategoriasProductoFinalesHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICategoriaProductoRepository"/>.</param>
    public ListCategoriasProductoFinalesHandler(ICategoriaProductoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<CategoriaProductoInfo>> Handle(ListCategoriasProductoFinales request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListByFranquiciaIdAsync(request.FranquiciaId, TipoCategoria.Finales, cancellationToken);
    }
}
