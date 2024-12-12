using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;

namespace OzyParkAdmin.Application.DetalleEscenarioCupo.List;
public sealed class ListDetalleEscenarioCupoHandler : QueryListOfHandler<ListDetalleEscenarioCupo, DetalleEscenarioCupoInfo>
{
    private readonly IDetalleEscenarioCupoRepository _repository;
    public ListDetalleEscenarioCupoHandler(IDetalleEscenarioCupoRepository repository, ILogger<ListDetalleEscenarioCupoHandler> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    protected override async Task<List<DetalleEscenarioCupoInfo>> ExecuteListAsync(ListDetalleEscenarioCupo query, CancellationToken cancellationToken)
    {
        return await _repository.ListAsync(query.EscenarioCupoId, cancellationToken);
    }
}
