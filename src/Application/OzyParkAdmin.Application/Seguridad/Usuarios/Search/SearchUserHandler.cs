using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Search;

/// <summary>
/// El manejador de <see cref="SearchUsers"/>.
/// </summary>
public sealed class SearchUserHandler : MediatorRequestHandler<SearchUsers, PagedList<UsuarioFullInfo>>
{
    private readonly IUsuarioRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="SearchUserHandler"/>.
    /// </summary>
    /// <param name="repository">El repositorio de usuarios.</param>
    public SearchUserHandler(IUsuarioRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<PagedList<UsuarioFullInfo>> Handle(SearchUsers request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        int[]? centrosCosto = request.User.GetCentrosCosto();
        string[]? roles = request.User.GetRoles();

        return await _repository.BuscarUsuariosAsync(request.SearchText, centrosCosto, roles, request.FilterExpressions, request.SortExpressions, request.Page, request.PageSize, cancellationToken);
    }
}
