using MassTransit.Mediator;
using OzyParkAdmin.Domain.Cupos;
using System.ComponentModel;

namespace OzyParkAdmin.Application.Cupos.Search;

/// <summary>
/// El manejador de <see cref="SearchCalendario"/>.
/// </summary>
public sealed class SearchCalendarioHandler : MediatorRequestHandler<SearchCalendario, ResultListOf<CupoFechaInfo>>
{
    private readonly ICupoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchCalendarioHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICupoRepository"/>.</param>
    public SearchCalendarioHandler(ICupoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<CupoFechaInfo>> Handle(SearchCalendario request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        DateTime inicio = request.Inicio ?? DateTime.Today;
        inicio = inicio.Date;
        DateTime fin = request.Fin ?? inicio;
        fin = fin.Date;


        return await _repository.SearchCuposParaCalendarioAsync(
            request.CanalVenta.Id,
            request.Alcance.Valor,
            request.Servicio.Id,
            request.ZonaOrigen?.Id,
            inicio,
            (fin - inicio).Days,
            cancellationToken);
    }
}
