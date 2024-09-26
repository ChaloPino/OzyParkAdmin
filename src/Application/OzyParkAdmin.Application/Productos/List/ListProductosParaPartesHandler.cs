using MassTransit.Mediator;
using OzyParkAdmin.Domain.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Application.Productos.List;

/// <summary>
/// El manejador de <see cref="ListProductosParaPartes"/>.
/// </summary>
public sealed class ListProductosParaPartesHandler : MediatorRequestHandler<ListProductosParaPartes, ResultListOf<ProductoInfo>>
{
    private readonly IProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListProductosParaPartesHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IProductoRepository"/>.</param>
    public ListProductosParaPartesHandler(IProductoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<ProductoInfo>> Handle(ListProductosParaPartes request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListProductosParaPartesAsync(request.FranquiciaId, request.ExceptoProductoId, cancellationToken);
    }
}
