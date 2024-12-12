using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;

namespace OzyParkAdmin.Application.DetallesEscenariosCuposExclusiones.List;
public sealed class ListDetalleEscenarioCupoExclsuionesHandler : QueryListOfHandler<ListDetalleEscenarioCupoExclsiones, DetalleEscenarioCupoExclusionFullInfo>
{
    private readonly IDetalleEscenarioCupoExclusionRepository _repository;
    public ListDetalleEscenarioCupoExclsuionesHandler(ILogger<ListDetalleEscenarioCupoExclsuionesHandler> logger, IDetalleEscenarioCupoExclusionRepository repository) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(_repository);

        _repository = repository;
    }
    /// <inheritdoc/>
    protected override async Task<List<DetalleEscenarioCupoExclusionFullInfo>> ExecuteListAsync(ListDetalleEscenarioCupoExclsiones query, CancellationToken cancellationToken)
    {
        var result = await _repository.GetExclusionesInfoByEscenarioCupoIdAsync(query.EscenarioCupoId, cancellationToken);

        return result.ToList();
    }
}
