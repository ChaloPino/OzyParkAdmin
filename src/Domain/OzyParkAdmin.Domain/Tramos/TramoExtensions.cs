namespace OzyParkAdmin.Domain.Tramos;
internal static class TramoExtensions
{
    public static TramoInfo ToInfo(this Tramo tramo) =>
        new() {  Id = tramo.Id, Aka =  tramo.Aka, Descripcion = tramo.Descripcion };
}
