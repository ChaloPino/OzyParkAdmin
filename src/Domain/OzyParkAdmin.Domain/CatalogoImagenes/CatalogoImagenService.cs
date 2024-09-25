namespace OzyParkAdmin.Domain.CatalogoImagenes;

/// <summary>
/// Servicio que permite realizar transacciones de los catálogos de imagen.
/// </summary>
public sealed class CatalogoImagenService
{
    private readonly ICatalogoImagenRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CatalogoImagenService"/>.
    /// </summary>
    /// <param name="repository"></param>
    public CatalogoImagenService(ICatalogoImagenRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <summary>
    /// Busca on crea un catálogo de imagen.
    /// </summary>
    /// <param name="info">El <see cref="CatalogoImagenInfo"/> usado para buscar o crear un catálogo de imagen.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El catálogo de imagen que se encontró o se creó.</returns>
    public async Task<CatalogoImagen> FindOrCreateAsync(CatalogoImagenInfo info, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(info);

        CatalogoImagen? catalogoImagen = await _repository.FindByAkaAsync(info.Aka, cancellationToken);

        if (catalogoImagen is null)
        {
            return CreateCatalogoImagen(info);
        }

        return UpdateCatalogoImagen(catalogoImagen, info);
        
    }

    private CatalogoImagen CreateCatalogoImagen(CatalogoImagenInfo info)
    {
        CatalogoImagen catalogoImagen = CatalogoImagen.Create(info.Aka, info.Base64, info.MimeType, info.Tipo);
        _repository.Add(catalogoImagen);
        return catalogoImagen;
    }


    private CatalogoImagen UpdateCatalogoImagen(CatalogoImagen catalogoImagen, CatalogoImagenInfo info)
    {
        catalogoImagen.Update(info.Base64, info.MimeType, info.Tipo);
        _repository.Update(catalogoImagen);
        return catalogoImagen;
    }


}
