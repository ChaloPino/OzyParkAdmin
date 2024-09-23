using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios;
internal static class ServicioExtensions
{
    public static PagedList<ServicioFullInfo> ToInfo(this PagedList<Servicio> source) =>
        new() { CurrentPage = source.CurrentPage, PageSize = source.PageSize, TotalCount = source.TotalCount, Items = source.Items.ToInfo() };

    private static IEnumerable<ServicioFullInfo> ToInfo(this IEnumerable<Servicio> source) =>
        source.Select(x => x.ToInfo()).ToList();
}
