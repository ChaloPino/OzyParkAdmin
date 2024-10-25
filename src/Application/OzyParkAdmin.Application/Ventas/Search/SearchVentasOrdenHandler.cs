using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Ventas;

namespace OzyParkAdmin.Application.Ventas.Search;

/// <summary>
/// El manejador de <see cref="SearchVentasOrden"/>
/// </summary>
public sealed class SearchVentasOrdenHandler : QueryPagedOfHandler<SearchVentasOrden, VentaOrdenInfo>
{
    private readonly IVentaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchVentasOrdenHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IVentaRepository"/></param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchVentasOrdenHandler(IVentaRepository repository, ILogger<SearchVentasOrdenHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<VentaOrdenInfo>> ExecutePagedListAsync(SearchVentasOrden query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.SearchVentasOrdenAsync(
            query.Fecha,
            query.NumeroOrden,
            query.VentaId,
            query.TicketId,
            query.Email,
            query.Telefono,
            query.Nombres,
            query.Apellidos,
            query.SortExpressions,
            query.Page,
            query.PageSize,
            cancellationToken);
    }
}
