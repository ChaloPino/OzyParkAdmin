using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.TiposDia.List;

/// <summary>
/// El manejador de <see cref="ListTiposDia"/>.
/// </summary>
public sealed class ListTiposDiaHandler : QueryListOfHandler<ListTiposDia, TipoDia>
{
    private readonly IGenericRepository<TipoDia> _repositorty;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListTiposDiaHandler"/>
    /// </summary>
    /// <param name="repository">El repositorio de <see cref="TipoDia"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListTiposDiaHandler(IGenericRepository<TipoDia> repository, ILogger<ListTiposDiaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repositorty = repository;
    }

    /// <inheritdoc/>
    protected override Task<List<TipoDia>> ExecuteListAsync(ListTiposDia query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return _repositorty.ListAsync(cancellationToken: cancellationToken);
    }
}
