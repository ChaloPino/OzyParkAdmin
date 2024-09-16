using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Seguridad.Roles;

namespace OzyParkAdmin.Application.Seguridad.Roles.List;

/// <summary>
/// El hanlder de <see cref="ListRoles"/>.
/// </summary>
public sealed class ListRolesHandler : MediatorRequestHandler<ListRoles, ResultListOf<Rol>>
{
    private readonly IRolRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListRolesHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IRolRepository"/>.</param>
    public ListRolesHandler(IRolRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<Rol>> Handle(ListRoles request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListChildRolesAsync(request.User.GetRoles(), cancellationToken);
    }
}
