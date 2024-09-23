namespace OzyParkAdmin.Domain.CentrosCosto;
internal static class CentroCostoExtensions
{
    public static CentroCostoInfo ToInfo(this CentroCosto centroCosto) =>
        new() {  Id = centroCosto.Id, Descripcion = centroCosto.Descripcion };

    public static List<CentroCostoInfo> ToInfo(this IEnumerable<CentroCosto> source) =>
        [.. source.Select(ToInfo)];
}
