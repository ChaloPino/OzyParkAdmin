using MassTransit.Mediator;
using OzyParkAdmin.Domain.ExclusionesCupo;
using OzyParkAdmin.Domain.Shared;
using System.Security.Claims;

namespace OzyParkAdmin.Application.ExclusionesCupo.Search;

/// <summary>
/// Busca fechas excluidas para cupos que cumplan con los criterios de búsqueda.
/// </summary>
/// <param name="User">El usuario que realiza la consulta.</param>
/// <param name="SearchText">El texto de búsqueda.</param>
/// <param name="FilterExpressions">Las expresiones de filtrado.</param>
/// <param name="SortExpressions">La expresiones de ordenamiento.</param>
/// <param name="Page">La página actual.</param>
/// <param name="PageSize">El tamaño de la página actual.</param>
public sealed record SearchFechasExcluidasCupo(
    ClaimsPrincipal User,
    string? SearchText,
    FilterExpressionCollection<FechaExcluidaCupo> FilterExpressions,
    SortExpressionCollection<FechaExcluidaCupo> SortExpressions,
    int Page,
    int PageSize) : Request<PagedList<FechaExcluidaCupoFullInfo>>;
