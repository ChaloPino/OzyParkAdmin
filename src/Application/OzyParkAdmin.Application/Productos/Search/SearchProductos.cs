using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Productos.Search;

/// <summary>
/// Realiza la búsqueda de productos que coincidan con los criterios de búsqueda.
/// </summary>
/// <param name="User">El usuario que realiza la consulta.</param>
/// <param name="SearchText">El texto de búsqueda.</param>
/// <param name="FilterExpressions">Las expresiones de filtrado.</param>
/// <param name="SortExpressions">Las expresiones de ordenamiento.</param>
/// <param name="Page">La página actual.</param>
/// <param name="PageSize">El tamaño de la página actual.</param>
public sealed record SearchProductos(
    ClaimsPrincipal User,
    string? SearchText,
    FilterExpressionCollection<Producto> FilterExpressions,
    SortExpressionCollection<Producto> SortExpressions,
    int Page,
    int PageSize) : IQueryPagedOf<ProductoFullInfo>;
