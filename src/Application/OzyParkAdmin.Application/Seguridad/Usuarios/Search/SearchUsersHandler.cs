using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Search;

/// <summary>
/// El manejador de <see cref="SearchUsers"/>.
/// </summary>
public sealed class SearchUsersHandler : QueryPagedOfHandler<SearchUsers, UsuarioFullInfo>
{
    private readonly IUsuarioRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchUsersHandler"/>.
    /// </summary>
    /// <param name="repository">El repositorio de usuarios.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public SearchUsersHandler(IUsuarioRepository repository, ILogger<SearchUsersHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<UsuarioFullInfo>> ExecutePagedListAsync(SearchUsers query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        int[]? centrosCosto = query.User.GetCentrosCosto();
        string[]? roles = query.User.GetRoles();

        return await _repository.BuscarUsuariosAsync(
            query.SearchText,
            centrosCosto,
            roles,
            query.FilterExpressions,
            query.SortExpressions,
            query.Page,
            query.PageSize,
            cancellationToken);
    }
}
