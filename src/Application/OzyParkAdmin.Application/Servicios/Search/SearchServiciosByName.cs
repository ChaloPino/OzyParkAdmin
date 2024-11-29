using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Application.Servicios.Search;

/// <summary>
/// Busca servicios por el centro de costo al que pertenece y el nombre.
/// </summary>
/// <param name="CentroCostoId">El id del centro de costo.</param>
/// <param name="SearchText">El texto a buscar en el nombre.</param>
public sealed record SearchServiciosByName(int CentroCostoId, string? SearchText) : IQueryListOf<ServicioWithDetailInfo>;
