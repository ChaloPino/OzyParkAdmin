using Microsoft.CodeAnalysis;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Franquicias;
using System.Collections.Immutable;

namespace OzyParkAdmin.Components.Admin.Shared;

internal static class ModelExtensions
{
    public static List<CentroCostoModel> ToModel(this IEnumerable<CentroCostoInfo> centrosCosto) =>
        centrosCosto.Select(ToModel).ToList();

    public static CentroCostoModel ToModel(this CentroCostoInfo centroCosto) =>
        new(centroCosto.Id, centroCosto.Descripcion);

    public static List<FranquiciaModel> ToModel(this IEnumerable<Franquicia> franquicias) =>
        franquicias.Select(ToModel).ToList();

    private static FranquiciaModel ToModel(Franquicia franquicia) =>
        new(franquicia.Id, franquicia.Nombre);

    public static CentroCostoInfo ToInfo(this CentroCostoModel centroCosto) =>
        new() {  Id = centroCosto.Id, Descripcion = centroCosto.Nombre };
}
