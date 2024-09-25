using Microsoft.EntityFrameworkCore;
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
                EF.Property<ProductoAgrupacion?>(x, "_productoAgrupacion") == null || 
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
            Familia = EF.Property<ProductoAgrupacion?>(x, "_productoAgrupacion") != null ? EF.Property<ProductoAgrupacion>(x, "_productoAgrupacion").AgrupacionContable : null,
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
}
