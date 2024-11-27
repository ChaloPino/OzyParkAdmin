namespace OzyParkAdmin.Domain.Tramos;
internal static class TramoExtensions
{
    public static IEnumerable<TramoInfo> ToInfo(this IEnumerable<Tramo> source) =>
        [.. source.Select(ToInfo)];
    public static TramoInfo ToInfo(this Tramo tramo) =>
        new() {  Id = tramo.Id, Aka =  tramo.Aka, Descripcion = tramo.Descripcion };
}
