using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Seguridad.Roles;

namespace OzyParkAdmin.Application.Seguridad.Roles.List;

/// <summary>
/// El hanlder de <see cref="ListRoles"/>.
/// </summary>
public sealed class ListRolesHandler : QueryListOfHandler<ListRoles, Rol>
{
    private readonly IRolRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListRolesHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IRolRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListRolesHandler(IRolRepository repository, ILogger<ListRolesHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<Rol>> ExecuteListAsync(ListRoles query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListChildRolesAsync(query.User.GetRoles(), cancellationToken);
    }
}
