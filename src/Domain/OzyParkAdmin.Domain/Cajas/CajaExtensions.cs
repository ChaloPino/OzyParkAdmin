namespace OzyParkAdmin.Domain.Cajas;
internal static class CajaExtensions
{
    public static CajaInfo ToInfo(this Caja caja) =>
        new() { Id = caja.Id, Aka = caja.Aka, Descripcion = caja.Descripcion };
}
