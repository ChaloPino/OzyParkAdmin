using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cupos;

namespace OzyParkAdmin.Application.Cupos.Search;

/// <summary>
/// El manejador de <see cref="SearchCalendario"/>.
/// </summary>
public sealed class SearchCalendarioHandler : QueryListOfHandler<SearchCalendario, CupoFechaInfo>
{
    private readonly ICupoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchCalendarioHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICupoRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchCalendarioHandler(ICupoRepository repository, ILogger<SearchCalendarioHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<CupoFechaInfo>> ExecuteListAsync(SearchCalendario query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        DateTime inicio = query.Inicio ?? DateTime.Today;
        inicio = inicio.Date;
        DateTime fin = query.Fin ?? inicio;
        fin = fin.Date;


        return await _repository.SearchCuposParaCalendarioAsync(
            query.CanalVenta.Id,
            query.Alcance.Valor,
            query.Servicio.Id,
            query.ZonaOrigen?.Id,
            inicio,
            (fin - inicio).Days,
            cancellationToken);
    }
}
