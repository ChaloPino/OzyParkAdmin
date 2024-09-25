using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.Servicios;

/// <summary>
/// El repositorio de <see cref="Servicio"/>.
/// </summary>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class ServicioRepository(OzyParkAdminContext context) : Repository<Servicio>(context), IServicioRepository
{
    /// <inheritdoc/>
    public async Task<Servicio?> FindByAkaAsync(int franquiciaId, string? aka, CancellationToken cancellationToken)
    {
        return await EntitySet.AsSplitQuery().FirstOrDefaultAsync(x => x.FranquiciaId == franquiciaId && x.Aka == aka, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Servicio?> FindByIdAsync(int servicioId, CancellationToken cancellationToken)
    {
        return await EntitySet.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == servicioId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<ServicioInfo>> ListAsync(int franquiciaId, CancellationToken cancellationToken)
    {
        return await EntitySet.AsNoTracking().AsSingleQuery().Where(x => x.FranquiciaId == franquiciaId)
            .OrderBy(x => x.Nombre)
            .Select(x => new ServicioInfo { Id = x.Id, Aka = x.Aka, Nombre = x.Nombre })
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> MaxIdAsync(CancellationToken cancellationToken)
    {
        int? id = await EntitySet.MaxAsync(x => (int?)x.Id, cancellationToken);
        return id is null ? 0 : id.Value;
    }

    /// <inheritdoc/>
    public async Task<PagedList<ServicioFullInfo>> SearchServiciosAsync(int[]? centrosCostoId, string? searchText, FilterExpressionCollection<Servicio> filterExpressions, SortExpressionCollection<Servicio> sortExpressions, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filterExpressions);
        ArgumentNullException.ThrowIfNull(sortExpressions);

        IQueryable<Servicio> query = EntitySet.AsNoTracking().AsSplitQuery();

        if (centrosCostoId is not null)
        {
            query = query.Where(x => centrosCostoId.Contains(x.CentroCosto.Id));
        }

        if (searchText is not null)
        {
            query = query.Where(x =>
                x.Aka.Contains(searchText) ||
                x.Nombre.Contains(searchText) ||
                x.CentroCosto.Descripcion.Contains(searchText) ||
                x.TipoDistribucion.Descripcion.Contains(searchText) ||
                x.TipoVigencia.Descripcion.Contains(searchText));
        }

        query = filterExpressions.Where(query);

        int count = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        List<ServicioFullInfo> servicios =  await sortExpressions.Sort(query).Skip(page * pageSize).Take(pageSize).Select(x => new ServicioFullInfo
        {
            Id = x.Id,
            Aka = x.Aka,
            Nombre = x.Nombre,
            FranquiciaId = x.FranquiciaId,
            CentroCosto = new CentroCostoInfo {  Id = x.CentroCosto.Id, Descripcion = x.CentroCosto.Descripcion },
            TipoControl = x.TipoControl,
            TipoDistribucion = x.TipoDistribucion,
            TipoServicio = x.TipoServicio,
            TipoVigencia = x.TipoVigencia,
            NumeroVigencia = x.NumeroVigencia,
            NumeroValidez = x.NumeroValidez,
            NumeroPaxMinimo = x.NumeroPaxMinimo,
            NumeroPaxMaximo = x.NumeroPaxMaximo,
            EsConHora = x.EsConHora,
            EsPorTramos = x.EsPorTramos,
            EsParaVenta = x.EsParaVenta,
            Orden = x.Orden,
            HolguraInicio = x.HolguraInicio,
            HolguraFin = x.HolguraFin,
            EsActivo = x.EsActivo,
            EsParaMovil = x.Movil != null,
            MostrarTramos = x.Movil != null ? x.Movil.MostrarTramos : null,
            EsParaBuses = x.Bus != null,
            IdaVuelta = x.Bus != null ? x.Bus.IdaVuelta : null,
            HolguraEntrada = x.HolguraEntrada,
            Politicas = EF.Property<ServicioPolitica?>(x, "_servicioPolitica") == null ? EF.Property<ServicioPolitica>(x, "_servicioPolitica").Politicas : null,
            ControlParental = x.ControlParental,
            ServicioResponsableId = EF.Property<ServicioControlParental?>(x, "_servicioControlParental") == null ? EF.Property<ServicioControlParental>(x, "_servicioControlParental").ServicioResponsableId : null,
            PlantillaId = x.PlantillaId,
            PlantillaDigitalId = x.PlantillaDigitalId,
        }).ToListAsync(cancellationToken).ConfigureAwait(false);

        return new PagedList<ServicioFullInfo>()
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = count,
            Items = servicios,
        };
    }
}
