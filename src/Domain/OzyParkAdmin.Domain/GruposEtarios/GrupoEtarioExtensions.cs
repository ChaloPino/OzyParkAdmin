using OzyParkAdmin.Domain.Servicios;
using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.GruposEtarios;
internal static class GrupoEtarioExtensions
{
    public static IEnumerable<GrupoEtarioInfo> ToInfo(this IEnumerable<GrupoEtario> gruposEtarios, IEnumerable<GrupoEtario> defaultGruposEtarios) =>
        gruposEtarios.Any() ? [.. gruposEtarios.Select(ToInfo)] : [.. defaultGruposEtarios.Select(ToInfo)];

    public static ImmutableArray<GrupoEtarioInfo> ToInfo(this IEnumerable<GrupoEtario> source) =>
        [.. source.Select(ToInfo)];

    public static GrupoEtarioInfo ToInfo(this GrupoEtario grupoEtario) =>
        new() { Id = grupoEtario.Id, Aka = grupoEtario.Aka, Descripcion = grupoEtario.Descripcion };
}
