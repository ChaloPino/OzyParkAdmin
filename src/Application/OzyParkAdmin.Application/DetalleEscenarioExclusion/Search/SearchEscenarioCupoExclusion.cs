using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.DetalleEscenarioExclusion.Search;

/// <summary>
/// Busca fechas excluidas para cupos que cumplan con los criterios de búsqueda.
/// </summary>
/// <param name="EscenarioId">Id del escenario cupo.</param>
/// <param name="SearchText">El texto de búsqueda.</param>
/// <param name="FilterExpressions">Las expresiones de filtrado.</param>
/// <param name="SortExpressions">La expresiones de ordenamiento.</param>
/// <param name="Page">La página actual.</param>
/// <param name="PageSize">El tamaño de la página actual.</param>
public sealed record SearchEscenarioCupoExclusion(
    int[] ServiciosIds,
    int[] CanalesDeVentaIds,
    int[] DiasDeSemanaIds,
    int EscenarioId,
    string? SearchText,
    FilterExpressionCollection<DetalleEscenarioCupoExclusion> FilterExpressions,
    SortExpressionCollection<DetalleEscenarioCupoExclusion> SortExpressions,
    int Page,
    int PageSize) : IQueryPagedOf<DetalleEscenarioCupoExclusionFullInfo>;


