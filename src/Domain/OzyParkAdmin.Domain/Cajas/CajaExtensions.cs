using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Cajas;
internal static class CajaExtensions
{
    public static ImmutableArray<CajaInfo> ToInfo(this IEnumerable<Caja> source) =>
        [.. source.Select(ToInfo)];

    public static CajaInfo ToInfo(this Caja caja) =>
        new() { Id = caja.Id, Aka = caja.Aka, Descripcion = caja.Descripcion };
}
