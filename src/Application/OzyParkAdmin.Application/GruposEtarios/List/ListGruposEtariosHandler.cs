using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.GruposEtarios;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.GruposEtarios.List;

/// <summary>
/// El manejador de <see cref="ListGruposEtarios"/>.
/// </summary>
public sealed class ListGruposEtariosHandler : QueryListOfHandler<ListGruposEtarios, GrupoEtarioInfo>
{
    private readonly IGenericRepository<GrupoEtario> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListGruposEtariosHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListGruposEtariosHandler(IGenericRepository<GrupoEtario> repository, ILogger<ListGruposEtariosHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<GrupoEtarioInfo>> ExecuteListAsync(ListGruposEtarios query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListAsync(
            selector: x => new GrupoEtarioInfo { Id = x.Id, Aka = x.Aka, Descripcion = x.Descripcion }, cancellationToken: cancellationToken);
    }
}
