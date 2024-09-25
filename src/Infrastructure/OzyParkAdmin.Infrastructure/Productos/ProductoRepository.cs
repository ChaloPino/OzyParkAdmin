using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Shared;
using System.Data;

namespace OzyParkAdmin.Infrastructure.Productos;

/// <summary>
/// El repositorio de <see cref="Producto"/>.
/// </summary>
public sealed class ProductoRepository(OzyParkAdminContext context) : Repository<Producto>(context), IProductoRepository
{
    private readonly DbSet<CategoriaProducto> _categoriaSet = context.Set<CategoriaProducto>();

    /// <inheritdoc/>
    public async Task<List<ProductoInfo>> ListComplementosByCategoriaAsync(int categoriaId, CancellationToken cancellationToken)
    {
        List<int> categorias = [];

        await ConseguirCategoriasProductosRecursivoAsync(categoriaId, categorias, cancellationToken).ConfigureAwait(false);

        return await EntitySet
            .Where(x => categorias.Contains(x.Id) && x.EsComplemento)
            .OrderBy(x => x.Nombre)
            .Select(x => new ProductoInfo {  Id = x.Id, Aka = x.Aka, Nombre = x.Nombre, EsActivo = x.EsActivo })
            .ToListAsync(cancellationToken);
    }

    private async Task ConseguirCategoriasProductosRecursivoAsync(int categoriaId, List<int> categorias, CancellationToken cancellationToken)
    {
        CategoriaProducto? categoria = await _categoriaSet
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Padre)
            .Include(x => x.Hijos)
            .FirstOrDefaultAsync(x => x.Id == categoriaId, cancellationToken);

        if (categoria is null)
        {
            return;
        }

        categorias.Add(categoria.Id);

        if (categoria.Padre is not null && !categorias.Contains(categoria.Padre.Id))
        {
            await ConseguirCategoriasProductosRecursivoAsync(categoria.Padre.Id, categorias, cancellationToken);
        }

        foreach (CategoriaProducto hijo in categoria.Hijos)
        {
            await ConseguirCategoriasProductosRecursivoAsync(hijo.Id, categorias, cancellationToken);
        }
    }

    /// <inheritdoc/>
    public async Task<PagedList<ProductoFullInfo>> SearchProductosAsync(int[]? centroCostoIds, string? searchText, FilterExpressionCollection<Producto> filterExpressions, SortExpressionCollection<Producto> sortExpressions, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filterExpressions);
        ArgumentNullException.ThrowIfNull(sortExpressions);

        IQueryable<Producto> query = EntitySet.AsNoTracking();

        if (centroCostoIds is not null)
        {
            query = query.Where(x => centroCostoIds.Contains(x.CentroCosto.Id));
        }

        if (searchText is not null)
        {
            query = query.Where(x =>
                x.Sku.Contains(searchText) ||
                x.Aka.Contains(searchText) ||
                x.Nombre.Contains(searchText) ||
                x.UsuarioCreacion.FriendlyName.Contains(searchText) ||
                x.UsuarioModificacion.FriendlyName.Contains(searchText) ||
                x.TipoProducto.Nombre.Contains(searchText) ||
                x.Categoria.Nombre.Contains(searchText) ||
                EF.Property<ProductoAgrupacion>(x, "_productoAgrupacion").AgrupacionContable.Aka.Contains(searchText));
        }

        filterExpressions.Replace("Familia.Aka", x => EF.Property<ProductoAgrupacion>(x, "_productoAgrupacion").AgrupacionContable.Aka);
        sortExpressions.Replace("Familia.Aka", x => EF.Property<ProductoAgrupacion>(x, "_productoAgrupacion").AgrupacionContable.Aka);

        query = filterExpressions.Where(query);

        int count = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        query = sortExpressions.Sort(query).Skip(page * pageSize).Take(pageSize);

        List<ProductoFullInfo> list = await query.Select(x => new ProductoFullInfo
        {
            Id = x.Id,
            Aka = x.Aka,
            Sku = x.Sku,
            Nombre = x.Nombre,
            CentroCosto = new CentroCostoInfo { Id = x.CentroCosto.Id, Descripcion = x.CentroCosto.Descripcion },
            FranquiciaId = x.FranquiciaId,
            Categoria = new CategoriaProductoInfo {  Id = x.Categoria.Id, Aka = x.Categoria.Aka, Nombre = x.Categoria.Nombre, EsActivo = x.Categoria.EsActivo },
            CategoriaDespliegue = new CategoriaProductoInfo { Id = x.CategoriaDespliegue.Id, Aka = x.CategoriaDespliegue.Aka, Nombre = x.CategoriaDespliegue.Nombre, EsActivo = x.CategoriaDespliegue.EsActivo },
            Imagen = new CatalogoImagenInfo { Aka = x.Imagen.Aka, Base64 = x.Imagen.Base64, MimeType = x.Imagen.MimeType, Tipo = x.Imagen.Tipo },
            TipoProducto = x.TipoProducto,
            Orden = x.Orden,
            Familia = EF.Property<ProductoAgrupacion>(x, "_productoAgrupacion").AgrupacionContable,
            EsComplemento = x.EsComplemento,
            EnInventario = x.EnInventario,
            FechaAlta = x.FechaAlta,
            FechaSistema = x.FechaSistema,
            UsuarioCreacion = new UsuarioInfo {  Id = x.UsuarioCreacion.Id, UserName = x.UsuarioCreacion.UserName, FriendlyName = x.UsuarioCreacion.FriendlyName },
            UltimaModificacion = x.UltimaModificacion,
            UsuarioModificacion = new UsuarioInfo { Id = x.UsuarioModificacion.Id, UserName = x.UsuarioModificacion.UserName, FriendlyName = x.UsuarioModificacion.FriendlyName },
            EsActivo = x.EsActivo,
        }).ToListAsync(cancellationToken).ConfigureAwait(false);

        return new PagedList<ProductoFullInfo>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = count,
            Items = list,
        };
    }

    /// <inheritdoc/>
    public async Task<Producto?> FindByIdAsync(int id, CancellationToken cancellationToken) =>
        await EntitySet.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <inheritdoc/>
    public async Task<IEnumerable<Producto>> FindByIdsAsync(int[] productoIds, CancellationToken cancellationToken) =>
        await EntitySet.AsSplitQuery().Where(x => productoIds.Contains(x.Id)).ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<int> MaxIdAsync(CancellationToken cancellationToken)
    {
        int? id = await EntitySet.MaxAsync(x => (int?)x.Id, cancellationToken);
        return id ?? 0;
    }

    /// <inheritdoc/>
    public async Task<Producto?> FindByAkaAsync(int franquiciaId, string? aka, CancellationToken cancellationToken) =>
        await EntitySet.FirstOrDefaultAsync(x => x.FranquiciaId == franquiciaId && x.Aka == aka, cancellationToken);

    /// <inheritdoc/>
    public async Task<bool> ExistAkaAsync(int productoId, int franquiciaId, string? aka, CancellationToken cancellationToken) =>
        await EntitySet.AnyAsync(x => x.FranquiciaId == franquiciaId && x.Aka == aka && x.Id != productoId, cancellationToken);
}
