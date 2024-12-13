using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;

namespace OzyParkAdmin.Application.Productos.Search;

/// <summary>
/// Busca Productos por el centro de costo al que pertenece y el nombre.
/// </summary>
/// <param name="CentroCostoId">El id del centro de costo.</param>
/// <param name="SearchText">El texto a buscar en el nombre.</param>
public sealed record SearchProductosByName(int CentroCostoId, string? SearchText) : IQueryListOf<ProductoInfo>;
