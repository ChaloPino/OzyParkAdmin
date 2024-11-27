using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasServicio;

namespace OzyParkAdmin.Application.TarfiasServicio.Search;

/// <summary>
/// El manejador de <see cref="SearchTarifasServicio"/>.
/// </summary>
public sealed class SearchTarifasServicioHandler : QueryPagedOfHandler<SearchTarifasServicio, TarifaServicioFullInfo>
{
    private readonly ITarifaServicioRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchTarifasServicioHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ITarifaServicioRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchTarifasServicioHandler(
        ITarifaServicioRepository repository,
        ILogger<SearchTarifasServicioHandler> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<TarifaServicioFullInfo>> ExecutePagedListAsync(SearchTarifasServicio query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        var tarifas = await _repository.SearchTarifasServiciosAsync(
            query.CentroCostoId,
            query.SearchText,
            query.FilterExpressions,
            query.SortExpressions,
            query.Page,
            query.PageSize,
            cancellationToken);

        return new PagedList<TarifaServicioFullInfo>
        {
            CurrentPage = tarifas.CurrentPage,
            PageSize = tarifas.PageSize,
            TotalCount = tarifas.TotalCount,
            Items = tarifas.Items.ToFullInfo(),
        };
    }
}
