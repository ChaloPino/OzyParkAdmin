using MassTransit.Mediator;
using OzyParkAdmin.Domain.OmisionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.OmisionesCupo.Search;

/// <summary>
/// Busca varias omisiones de exclusión de escenarios de cupo que coincidan con los criterios de búsqueda.
/// </summary>
/// <param name="SearchText">Un texto libre para buscar en el escenario y el canal de venta.</param>
/// <param name="FilterExpressions">Las expresiones de filtrado.</param>
/// <param name="SortExpressions">Las expresiones de ordenamiento.</param>
/// <param name="Page">La página actual.</param>
/// <param name="PageSize">El tamaño de la página actual.</param>
public sealed record SearchOmisionesEscenarioCupoExlusion(
    string? SearchText,
    FilterExpressionCollection<IgnoraEscenarioCupoExclusion> FilterExpressions,
    SortExpressionCollection<IgnoraEscenarioCupoExclusion> SortExpressions,
    int Page,
    int PageSize) : Request<PagedList<IgnoraEscenarioCupoExclusionFullInfo>>;
