using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.CatalogoImagenes;

/// <summary>
/// El repositorio de <see cref="CatalogoImagen"/>
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="CatalogoImagenRepository"/>.
/// </remarks>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class CatalogoImagenRepository(OzyParkAdminContext context) : Repository<CatalogoImagen>(context), ICatalogoImagenRepository
{
    /// <inheritdoc/>
    public void Add(CatalogoImagen catalogoImagen) =>
        Context.Add(catalogoImagen);

    /// <inheritdoc/>
    public async Task<CatalogoImagen?> FindByAkaAsync(string aka, CancellationToken cancellationToken) =>
        await EntitySet.FirstOrDefaultAsync(x => x.Aka == aka, cancellationToken);

    /// <inheritdoc/>
    public void Update(CatalogoImagen catalogoImagen) =>
        Context.Update(catalogoImagen);
}
