using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CatalogoImagenes;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.OmisionesCupo;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;
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

    /// <inheritdoc/>
    public async Task<PagedList<CategoriaProductoFullInfo>> SearchCategoriaProductoAsync(string? searchText, FilterExpressionCollection<CategoriaProducto> filterExpressions, SortExpressionCollection<CategoriaProducto> sortExpressions, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filterExpressions);
        ArgumentNullException.ThrowIfNull(sortExpressions);

        //Ojo que AsNoTracking tiene un problema de recursividad. Por ejemplo para el Caso del "NombreComleto" que se conforma en forma recursiva.
        //> genera error IQueryable<CategoriaProducto> query = EntitySet.AsSplitQuery();
        IQueryable<CategoriaProducto> query = EntitySet.AsNoTracking();

        if (searchText is not null)
        {
            query = query.Where(x =>
                x.Aka.Contains(searchText) ||
                x.Nombre.Contains(searchText) ||
                x.UsuarioCreacion.UserName.Contains(searchText) ||
                x.UsuarioModificacion.UserName.Contains(searchText));
        }

        query = filterExpressions.Where(query);

        int totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        //TODO: ver como resolver traer el "mombreCompleto"
        var items = await sortExpressions.Sort(query).Skip(page * pageSize).Take(pageSize).Select(x => new CategoriaProductoFullInfo
        {
            Id = x.Id,
            FranquiciaId = x.FranquiciaId,
            Aka = x.Aka,
            Nombre = x.Nombre,
            EsActivo = x.EsActivo,
            Padre = x.Padre != null ? new CategoriaProductoInfo { Id = x.Padre.Id, Aka = x.Padre.Aka, Nombre = x.Padre.Nombre, EsActivo = x.Padre.EsActivo, NombreCompleto = x.Padre.Nombre } : null,
            EsFinal = x.EsFinal,
            Imagen = new CatalogoImagenInfo { Aka = x.Imagen.Aka, Base64 = x.Imagen.Base64, MimeType = x.Imagen.MimeType, Tipo = x.Imagen.Tipo },
            Orden = x.Orden,
            EsTop = x.EsTop,
            Nivel = x.Nivel,
            PrimeroProductos = x.PrimeroProductos,
            UsuarioCreacion = new UsuarioInfo { Id = x.UsuarioCreacion.Id, UserName = x.UsuarioCreacion.UserName, FriendlyName = x.UsuarioCreacion.FriendlyName },
            FechaCreacion = x.FechaCreacion,
            UsuarioModificacion = new UsuarioInfo { Id = x.UsuarioModificacion.Id, UserName = x.UsuarioModificacion.UserName, FriendlyName = x.UsuarioModificacion.FriendlyName },
            UltimaModificacion = x.UltimaModificacion,
            CajasAsignadas = x.CajasAsignadas,
            CanalesVenta = x.CanalesVenta,
            Hijos = x.Hijos.Select(s => new CategoriaProductoInfo
            {
                Id = s.Id,
                Aka = s.Aka,
                Nombre = s.Nombre,
                EsActivo = s.EsActivo,
                NombreCompleto = s.Nombre
            }),
        }).ToListAsync(cancellationToken).ConfigureAwait(false);

        return new PagedList<CategoriaProductoFullInfo>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items,
        };

    }
}
