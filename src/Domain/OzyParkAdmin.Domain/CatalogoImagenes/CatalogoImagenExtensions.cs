namespace OzyParkAdmin.Domain.CatalogoImagenes;
internal static class CatalogoImagenExtensions
{
    public static CatalogoImagenInfo ToInfo(this CatalogoImagen catalogoImagen) =>
        new() {  Aka =  catalogoImagen.Aka, Base64 =  catalogoImagen.Base64, MimeType = catalogoImagen.MimeType, Tipo = catalogoImagen.Tipo };
}
