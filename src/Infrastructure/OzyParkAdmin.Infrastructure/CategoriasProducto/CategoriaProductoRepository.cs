using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Infrastructure.Shared;
using System.Diagnostics;

namespace OzyParkAdmin.Infrastructure.CategoriasProducto;

/// <summary>
/// El repositorio de <see cref="CategoriaProducto"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="CategoriaProductoRepository"/>.
/// </remarks>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class CategoriaProductoRepository(OzyParkAdminContext context) : Repository<CategoriaProducto>(context), ICategoriaProductoRepository
{
    /// <inheritdoc/>
    public async Task<CategoriaProducto?> FindByIdAsync(int id, CancellationToken cancellationToken) =>
        await EntitySet.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<List<CategoriaProductoInfo>> ListByFranquiciaIdAsync(int franquiciaId, TipoCategoria tipoCategoria, CancellationToken cancellationToken)
    {
        IQueryable<CategoriaProducto> query = EntitySet.AsSplitQuery().Where(x => x.FranquiciaId == franquiciaId);

        query = tipoCategoria switch
        {
            TipoCategoria.Padres => query.Where(x => x.Padre == null),
            TipoCategoria.Intermedias => query.Where(x => !x.EsFinal && x.Padre != null),
            TipoCategoria.Finales => query.Where(x => x.EsFinal),
            TipoCategoria.Todas => query,
            _ => throw new UnreachableException(),
        };

        List<CategoriaProducto> categorias = await query
            .Include(x => x.Padre)
            .OrderBy(x => x.Orden)
            .ToListAsync(cancellationToken);

        return categorias.ToInfo();
    }

    /// <inheritdoc/>
    public async Task<int> MaxIdAsync(CancellationToken cancellationToken)
    {
        int? id = await EntitySet.MaxAsync(x => (int?)x.Id, cancellationToken);
        return id ?? 0;
    }

}
