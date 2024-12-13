using Dapper;
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
    /// <inheritdoc/>
    public async Task<bool> ExistAkaAsync(int productoId, int franquiciaId, string? aka, CancellationToken cancellationToken) =>
        await EntitySet.AnyAsync(x => x.FranquiciaId == franquiciaId && x.Aka == aka && x.Id != productoId, cancellationToken);

    /// <inheritdoc/>
    public async Task<bool> ExistSkuAsync(int productoId, int franquiciaId, string? sku, CancellationToken cancellationToken) =>
        await EntitySet.AnyAsync(x => x.FranquiciaId == franquiciaId && x.Sku == sku && x.Id != productoId, cancellationToken);

    /// <inheritdoc/>
    public async Task<Producto?> FindByIdAsync(int id, CancellationToken cancellationToken) =>
        await EntitySet.AsSplitQuery().Include("Imagen").Include("Complementos.Complemento").Include("Relacionados.Relacionado").Include("Partes.Parte").Include("Cajas").FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    /// <inheritdoc/>
    public async Task<Producto?> FindByIdAsync(int id, ProductoDetail incluirDetalle, CancellationToken cancellationToken)
    {
        IQueryable<Producto> query = EntitySet.IgnoreAutoIncludes().AsSplitQuery();

        if ((incluirDetalle & ProductoDetail.Cajas) == ProductoDetail.Cajas)
        {
            query = query.Include("Cajas");
        }

        if ((incluirDetalle & ProductoDetail.Partes) == ProductoDetail.Partes)
        {
            query = query.Include("Partes.Parte");
        }

        if ((incluirDetalle & ProductoDetail.Complementos) == ProductoDetail.Complementos)
        {
            query = query.Include("Complementos.Complemento");
        }

        if ((incluirDetalle & ProductoDetail.Relacionados) == ProductoDetail.Relacionados)
        {
            query = query.Include("Complementos.Complemento");
        }

        return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Producto>> FindByIdsAsync(int[] productoIds, CancellationToken cancellationToken) =>
        await EntitySet.AsSplitQuery().Where(x => productoIds.Contains(x.Id)).ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<Producto?> FindByAkaAsync(int franquiciaId, string? aka, CancellationToken cancellationToken) =>
        await EntitySet.FirstOrDefaultAsync(x => x.FranquiciaId == franquiciaId && x.Aka == aka, cancellationToken);

    /// <inheritdoc/>
    public async Task<List<ProductoInfo>> ListComplementosByCategoriaAsync(int categoriaId, int exceptoProductoId, CancellationToken cancellationToken)
    {
        CommandDefinition command = new(
            commandText: "qsp_retornaComplementos_prc",
                parameters: new Dictionary<string, object> { ["@EI_categoriaId"] = categoriaId, ["@EI_productoId"] = exceptoProductoId },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);
        var productos = await Context.Database.GetDbConnection().QueryAsync<ProductoInfo>(command);
        return productos.ToList();
    }

    /// <inheritdoc/>
    public async Task<List<ProductoInfo>> ListProductosParaPartesAsync(int franquiciaId, int exceptoProductoId, CancellationToken cancellationToken)
    {
        return await EntitySet
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.FranquiciaId == franquiciaId && x.Id != exceptoProductoId && x.TipoProducto.ControlaInventario)
            .Select(x => new ProductoInfo { Id = x.Id, Aka = x.Aka, Sku = x.Sku, Nombre = x.Nombre, EsActivo = x.EsActivo })
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> MaxIdAsync(CancellationToken cancellationToken)
    {
        int? id = await EntitySet.MaxAsync(x => (int?)x.Id, cancellationToken);
        return id ?? 0;
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
            Categoria = new CategoriaProductoInfo { Id = x.Categoria.Id, Aka = x.Categoria.Aka, Nombre = x.Categoria.Nombre, EsActivo = x.Categoria.EsActivo },
            CategoriaDespliegue = new CategoriaProductoInfo { Id = x.CategoriaDespliegue.Id, Aka = x.CategoriaDespliegue.Aka, Nombre = x.CategoriaDespliegue.Nombre, EsActivo = x.CategoriaDespliegue.EsActivo },
            Imagen = new CatalogoImagenInfo { Aka = x.Imagen.Aka, Base64 = x.Imagen.Base64, MimeType = x.Imagen.MimeType, Tipo = x.Imagen.Tipo },
            TipoProducto = x.TipoProducto,
            Orden = x.Orden,
            Familia = EF.Property<ProductoAgrupacion>(x, "_productoAgrupacion").AgrupacionContable,
            EsComplemento = x.EsComplemento,
            EnInventario = x.EnInventario,
            FechaAlta = x.FechaAlta,
            FechaSistema = x.FechaSistema,
            UsuarioCreacion = new UsuarioInfo { Id = x.UsuarioCreacion.Id, UserName = x.UsuarioCreacion.UserName, FriendlyName = x.UsuarioCreacion.FriendlyName },
            UltimaModificacion = x.UltimaModificacion,
            UsuarioModificacion = new UsuarioInfo { Id = x.UsuarioModificacion.Id, UserName = x.UsuarioModificacion.UserName, FriendlyName = x.UsuarioModificacion.FriendlyName },
            EsActivo = x.EsActivo,
            Complementos = x.Complementos.Select(c => new ProductoComplementarioInfo
            {
                Complemento = new ProductoInfo { Id = c.Complemento.Id, Aka = c.Complemento.Aka, Sku = c.Complemento.Sku, Nombre = c.Complemento.Nombre, EsActivo = c.Complemento.EsActivo },
                Orden = c.Orden,
            })
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
    public async Task<List<ProductoInfo>> SearchProductosAsync(int centroCostoId, string? searchText, CancellationToken cancellationToken)
    {

        IQueryable<Producto> query = EntitySet.AsNoTracking();

        query = query.Where(x => x.CentroCosto.Id == centroCostoId);

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

        List<ProductoInfo> list = await query.Select(x => new ProductoInfo
        {
            Id = x.Id,
            Aka = x.Aka,
            Sku = x.Sku,
            Nombre = x.Nombre,
            EsActivo = x.EsActivo,
        }).ToListAsync(cancellationToken).ConfigureAwait(false);

        return list;
    }
}
