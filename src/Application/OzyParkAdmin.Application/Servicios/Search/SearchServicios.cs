using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Servicios.Search;

/// <summary>
/// Encuentra servicios que coincidan con el criterio de búsqueda definido por <paramref name="SearchText"/>.
/// </summary>
/// <param name="User">El usuario que está realizando la búsqueda.</param>
/// <param name="SearchText">El criterio de búsqueda.</param>
/// <param name="FilterExpressions">Las expresiones de filtrado.</param>
/// <param name="SortExpressions">Las expresiones de ordenamiento.</param>
/// <param name="Page">La página actual.</param>
/// <param name="PageSize">El tamaño de la página actual.</param>
public sealed record SearchServicios(
    ClaimsPrincipal User,
    string? SearchText,
    FilterExpressionCollection<Servicio> FilterExpressions,
    SortExpressionCollection<Servicio> SortExpressions,
    int Page,
    int PageSize) : IQueryPagedOf<ServicioFullInfo>;
