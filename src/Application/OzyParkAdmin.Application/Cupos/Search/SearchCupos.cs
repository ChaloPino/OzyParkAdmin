using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Cupos.Search;

/// <summary>
/// Busca cupos que coincidan con el criterio de búsqueda.
/// </summary>
/// <param name="User">El usuario que realiza la consulta.</param>
/// <param name="SearchText">El texto de búsqueda.</param>
/// <param name="FilterExpressions">Las expresiones de filtrado.</param>
/// <param name="SortExpressions">Las expresiones de ordenamiento.</param>
/// <param name="Page">La página actual.</param>
/// <param name="PageSize">El tamaño de la página actual.</param>
public sealed record SearchCupos(
    ClaimsPrincipal User,
    string? SearchText,
    FilterExpressionCollection<Cupo> FilterExpressions,
    SortExpressionCollection<Cupo> SortExpressions,
    int Page,
    int PageSize) : IQueryPagedOf<CupoFullInfo>;
