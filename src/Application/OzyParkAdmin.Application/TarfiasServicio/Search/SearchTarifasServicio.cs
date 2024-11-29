using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasServicio;

namespace OzyParkAdmin.Application.TarfiasServicio.Search;

/// <summary>
/// Busca tarifas de servicios que coincidan con los criterios de búsqueda.
/// </summary>
/// <param name="CentroCostoId">El centro de costo asociado a los servicios.</param>
/// <param name="SearchText">El texto de búsqueda.</param>
/// <param name="FilterExpressions">Las expresiones de filtrado.</param>
/// <param name="SortExpressions">Las expresiones de ordenamiento.</param>
/// <param name="Page">La página actual.</param>
/// <param name="PageSize">El tamaño de la página actual.</param>
public sealed record SearchTarifasServicio(
    int CentroCostoId,
    string? SearchText,
    FilterExpressionCollection<TarifaServicio> FilterExpressions,
    SortExpressionCollection<TarifaServicio> SortExpressions,
    int Page,
    int PageSize) : IQueryPagedOf<TarifaServicioFullInfo>;
