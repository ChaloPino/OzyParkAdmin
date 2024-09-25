namespace OzyParkAdmin.Domain.CatalogoImagenes;

/// <summary>
/// El repositorio de <see cref="CatalogoImagen"/>.
/// </summary>
public interface ICatalogoImagenRepository
{
    /// <summary>
    /// Agrega el catálogo de imagen en el repositorio.
    /// </summary>
    /// <param name="catalogoImagen">El <see cref="CatalogoImagen"/> a agregar.</param>
    void Add(CatalogoImagen catalogoImagen);

    /// <summary>
    /// Actualiza el catálogo de imagen en el repositorio.
    /// </summary>
    /// <param name="catalogoImagen">El <see cref="CatalogoImagen"/> a actualizar.</param>
    void Update(CatalogoImagen catalogoImagen);

    /// <summary>
    /// Busca un catálogo de imagen por su aka.
    /// </summary>
    /// <param name="aka">El aka a buscar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El catálogo de imagen si existe.</returns>
    Task<CatalogoImagen?> FindByAkaAsync(string aka, CancellationToken cancellationToken);
}
