using MassTransit.Mediator;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cajas.Search;

/// <summary>
/// Busca todas las aperturas de caja que coincidan con el criterio de búsqueda.
/// </summary>
/// <param name="CentroCostoId">El id del centro de costo a buscar.</param>
/// <param name="SearchText">El texto de búsqueda.</param>
/// <param name="SearchDate">El día de búsqueda.</param>
/// <param name="FilterExpressions">La expresiones de filtrado.</param>
/// <param name="SortExpressions">Las expresiones de ordeanmiento.</param>
/// <param name="Page">La página actual.</param>
/// <param name="PageSize">El tamaño de la página actual.</param>
public sealed record SearchAperturasCaja(int CentroCostoId, string? SearchText, DateOnly SearchDate, FilterExpressionCollection<AperturaCajaInfo> FilterExpressions, SortExpressionCollection<AperturaCajaInfo> SortExpressions, int Page, int PageSize) : Request<PagedList<AperturaCajaInfo>>;
