using MassTransit.Mediator;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Search;

/// <summary>
/// Encuentra usuarios que coincidan con el criterio búsqueda definido por <paramref name="SearchText"/>.
/// </summary>
/// <param name="User">El usuario que está realizando la búsqueda.</param>
/// <param name="SearchText">El criterio de búsqueda.</param>
/// <param name="FilterExpressions">Las expressiones de filtrado.</param>
/// <param name="SortExpressions">Las expresiones de ordenamiento.</param>
/// <param name="Page">La página actual.</param>
/// <param name="PageSize">El tamaño de la página actual.</param>
public sealed record SearchUsers(ClaimsPrincipal User, string? SearchText, FilterExpressionCollection<Usuario> FilterExpressions, SortExpressionCollection<Usuario> SortExpressions, int Page, int PageSize) : Request<PagedList<UsuarioFullInfo>>;
