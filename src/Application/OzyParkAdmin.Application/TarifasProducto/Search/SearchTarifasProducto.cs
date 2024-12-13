using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasProducto;

namespace OzyParkAdmin.Application.TarfiasProducto.Search;

/// <summary>
/// Busca tarifas de productos que coincidan con los criterios de búsqueda.
/// </summary>
/// <param name="CentroCostoId">El centro de costo asociado a los Productos.</param>
/// <param name="SearchText">El texto de búsqueda.</param>
/// <param name="FilterExpressions">Las expresiones de filtrado.</param>
/// <param name="SortExpressions">Las expresiones de ordenamiento.</param>
/// <param name="Page">La página actual.</param>
/// <param name="PageSize">El tamaño de la página actual.</param>
public sealed record SearchTarifasProducto(
    int CentroCostoId,
    string? SearchText,
    FilterExpressionCollection<TarifaProducto> FilterExpressions,
    SortExpressionCollection<TarifaProducto> SortExpressions,
    int Page,
    int PageSize) : IQueryPagedOf<TarifaProductoFullInfo>;
