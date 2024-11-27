using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.TiposSegmentacion.List;

/// <summary>
/// El manejador de <see cref="ListTiposSegmentacion"/>.
/// </summary>
public sealed class ListTiposSegmentacionHandler : QueryListOfHandler<ListTiposSegmentacion, TipoSegmentacion>
{
    private readonly IGenericRepository<TipoSegmentacion> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTiposSegmentacionHandler"/>.
    /// </summary>
    /// <param name="repository">El repositorio de <see cref="TipoSegmentacion"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListTiposSegmentacionHandler(IGenericRepository<TipoSegmentacion> repository, ILogger<ListTiposSegmentacionHandler> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<TipoSegmentacion>> ExecuteListAsync(ListTiposSegmentacion query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }
}
