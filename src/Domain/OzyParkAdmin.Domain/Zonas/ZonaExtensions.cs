namespace OzyParkAdmin.Domain.Zonas;
internal static class ZonaExtensions
{
    public static ZonaInfo ToInfo(this Zona zona) =>
        new() { Id = zona.Id, Descripcion = zona.Descripcion };
}
