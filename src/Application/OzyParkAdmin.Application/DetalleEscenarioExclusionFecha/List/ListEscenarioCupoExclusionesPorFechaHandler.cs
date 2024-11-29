using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.EscenariosCupo;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using OzyParkAdmin.Domain.Repositories;

namespace OzyParkAdmin.Application.DetalleEscenarioExclusionFecha.List;

public sealed class ListEscenarioCupoExclusionesPorFechaHandler : QueryListOfHandler<ListEscenarioCupoExclusionesPorFecha, DetalleEscenarioCupoExclusionFechaFullInfo>
{
    private readonly IDetalleEscenarioCupoExclusionFechaRepository _repository;
    public ListEscenarioCupoExclusionesPorFechaHandler(ILogger<ListEscenarioCupoExclusionesPorFechaHandler> logger, IDetalleEscenarioCupoExclusionFechaRepository repository)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    protected override async Task<List<DetalleEscenarioCupoExclusionFechaFullInfo>> ExecuteListAsync(ListEscenarioCupoExclusionesPorFecha query, CancellationToken cancellationToken)
    {
        var results = await _repository.GetExclusionesByEscenarioCupoIdAsync(query.escenarioCupoId, cancellationToken);

        return results.Select(x => x.ToFullInfo()).ToList();
    }
}
