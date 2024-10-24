using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Application.CuposFecha.Search;

/// <summary>
/// Busca cupos por fecha que coincidan con el criterio de búsqueda.
/// </summary>
/// <param name="User">El usuario que realiza la consulta.</param>
/// <param name="SearchText">El texto de búsqueda.</param>
/// <param name="FilterExpressions">Las expresiones de filtrado.</param>
/// <param name="SortExpressions">Las expresiones de ordenamiento.</param>
/// <param name="Page">La página actual.</param>
/// <param name="PageSize">El tamaño de la página actual.</param>
public sealed record SearchCuposFecha(
    ClaimsPrincipal User,
    string? SearchText,
    FilterExpressionCollection<CupoFecha> FilterExpressions,
    SortExpressionCollection<CupoFecha> SortExpressions,
    int Page,
    int PageSize) : IQueryPagedOf<CupoFechaFullInfo>;
