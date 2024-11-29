using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.TiposHorario.List;

/// <summary>
/// El manejador de <see cref="ListTiposHorario"/>.
/// </summary>
public sealed class ListTiposHorarioHandler : QueryListOfHandler<ListTiposHorario, TipoHorario>
{
    private readonly IGenericRepository<TipoHorario> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTiposHorarioHandler"/>.
    /// </summary>
    /// <param name="repository">El repositorio de <see cref="TipoHorario"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListTiposHorarioHandler(IGenericRepository<TipoHorario> repository, ILogger<ListTiposHorarioHandler> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<TipoHorario>> ExecuteListAsync(ListTiposHorario query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }
}
