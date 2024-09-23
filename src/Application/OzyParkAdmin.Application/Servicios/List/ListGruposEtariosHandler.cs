using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.List;

/// <summary>
/// El manejador de <see cref="ListGruposEtarios"/>.
/// </summary>
public sealed class ListGruposEtariosHandler : MediatorRequestHandler<ListGruposEtarios, ResultListOf<GrupoEtarioInfo>>
{
    private readonly IGenericRepository<GrupoEtario> _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListGruposEtariosHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IGenericRepository{TEntity}"/>.</param>
    public ListGruposEtariosHandler(IGenericRepository<GrupoEtario> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<GrupoEtarioInfo>> Handle(ListGruposEtarios request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListAsync(
            selector: x => new GrupoEtarioInfo { Id = x.Id, Aka = x.Aka, Descripcion = x.Descripcion }, cancellationToken: cancellationToken);
    }
}
