using Dapper;
using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Shared;
using System.Collections.Immutable;
using System.Data;

namespace OzyParkAdmin.Infrastructure.Cajas;

/// <summary>
/// El repositorio de <see cref="Caja"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="CajaRepository"/>.
/// </remarks>
/// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
public sealed class CajaRepository(OzyParkAdminContext context) : Repository<Caja>(context), ICajaRepository
{
    /// <inheritdoc/>
    public async Task<AperturaCajaDetalleInfo?> FindAperturaCajaDetalleAsync(Guid aperturaCajaId, CancellationToken cancellationToken)
    {
        DynamicParameters parameters = new();
        parameters.Add("AperturaCajaId", aperturaCajaId);
        CommandDefinition definition = new(
            commandText: "qsp_getAperturaDiaDetalles_prc",
            parameters: parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        var reader = await Context.Database.GetDbConnection().QueryMultipleAsync(definition);

        IEnumerable<TurnoData> turnos = await reader.ReadAsync<TurnoData>();
        IEnumerable<ServicioDiaInfo> servicios = await reader.ReadAsync<ServicioDiaInfo>();
        IEnumerable<ResumenData> resumenes = await reader.ReadAsync<ResumenData>();
        IEnumerable<DetalleData> detalles = await reader.ReadAsync<DetalleData>();

        IEnumerable<TurnoCajaInfo> turnosInfo = turnos.Select(x => new TurnoCajaInfo
        {
            DiaId = x.AperturaCajaId,
            Id = x.AperturaTurnoId,
            Caja = new CajaInfo { Id = x.CajaId, Aka = x.CajaAka },
            Gaveta = new GavetaInfo { Id = x.GavetaId, Aka = x.GavetaAka },
            PuntoVenta = x.PuntoVenta,
            Usuario = x.Usuario,
            Supervisor = x.Supervisor,
            FechaApertura = x.FechaApertura,
            IpAddressApertura = x.IpAddressApertura,
            IpAddressCierre = x.IpAddressCierre,
            FechaCierre = x.FechaCierre,
            EfectivoInicio = x.EfectivoInicio,
            EfectivoCierreEjecutivo = x.EfectivoCierreEjecutivo,
            MontoTransbankEjecutivo = x.MontoTransbankEjecutivo,
            MontoVoucher = x.MontoVoucher,
            DiferenciaEfectivo = x.DiferenciaEfectivo,
            DiferenciaMontoTransbank = x.DiferenciaMontoTransbank,
            EfectivoCierreSistema = x.EfectivoCierreSistema,
            MontoTransbankSistema = x.MontoTransbankSistema,
            EfectivoCierreSupervisor = x.EfectivoCierreSupervisor,
            MontoTransbankSupervisor = x.MontoTransbankSupervisor,
            Comentario = x.Comentario,
            Estado = (EstadoTurno)x.Estado,
            EstadoDia = (EstadoDia)x.EstadoDia,
        }).ToList();

        (from turno in turnosInfo
         join resumen in resumenes on turno.Id equals resumen.AperturaTurnoId
         let resumenIfo = new ResumenTurnoInfo { Tipo = resumen.Tipo, Movimiento = resumen.Movimiento, EsAbono = resumen.EsAbono, Cantidad = resumen.Cantidad, Monto = resumen.Monto }
         group resumenIfo by turno into grupoResumen
         select grupoResumen).ToList().ForEach(x => x.Key.Resumen.AddRange(x));

        (from turno in turnosInfo
         join detalle in detalles on turno.Id equals detalle.AperturaTurnoId
         let detalleInfo = new DetalleTurnoInfo
         {
             Tipo = detalle.Tipo,
             EsAbono = detalle.EsAbono,
             Fecha = DateOnly.FromDateTime(detalle.Fecha),
             Hora = detalle.Hora,
             Monto = detalle.Monto,
             MontoConSigno = detalle.MontoConSigno,
             Movimiento = detalle.Movimiento,
             FormaPagoAka = detalle.FormaPagoAka,
             FormaPago = detalle.FormaPago,
             NumeroReferencia = detalle.NumeroReferencia,
             Orden = detalle.Orden,
             Supervisor = detalle.Supervisor,
             TieneAccion = detalle.TieneAccion,
             Usuario = detalle.Usuario
         }
         group detalleInfo by turno into grupoDetalle
         select grupoDetalle).ToList().ForEach(x => x.Key.Detalle.AddRange(x));

        return new()
        {
            Turnos = [..turnosInfo],
            Servicios = [..servicios],
        };
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Caja>> FindByIdsAsync(int[] cajaIds, CancellationToken cancellationToken)
    {
        return await EntitySet.Where(x => cajaIds.Contains(x.Id)).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<AperturaDia?> FindAperturaDiaAsync(Guid diaId, CancellationToken cancellationToken)
    {
        return await Context.Set<AperturaDia>().AsSplitQuery()
            .Include("Caja.PuntoVenta")
            .Include("Caja.CentroCosto")
            .Include("Caja.Franquicia")
            .Include("Usuario")
            .Include("Supervisor")
            .Include("Turnos.Usuario")
            .Include("Turnos.Supervisor")
            .Include("Turnos.Gaveta")
            .FirstOrDefaultAsync(x => x.Id == diaId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<CajaInfo>> ListCajasAsync(int[]? centroCostoIds, CancellationToken cancellationToken)
    {
        return centroCostoIds is null
            ? await EntitySet.AsNoTracking().Select(x => new CajaInfo { Id = x.Id, Aka = x.Aka, Descripcion = x.Descripcion }).ToListAsync(cancellationToken)
            : await EntitySet.AsNoTracking().Where(x => centroCostoIds.Contains(x.CentroCosto.Id)).Select(x => new CajaInfo { Id = x.Id, Aka = x.Aka, Descripcion = x.Descripcion }).ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<PagedList<AperturaCajaInfo>> SearchAperturaCajasAsync(int centroCostoId, string? searchText, DateOnly searchDate, FilterExpressionCollection<AperturaCajaInfo> filterExpressions, SortExpressionCollection<AperturaCajaInfo> sortExpressions, int page, int pageSize, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filterExpressions);
        ArgumentNullException.ThrowIfNull(sortExpressions);

        var maxTurnoQuery = from apertura in Context.Set<AperturaDia>()
                            from turno in apertura.Turnos
                            join maxTurno in from apertura in Context.Set<AperturaDia>()
                                             from turno in apertura.Turnos
                                             where apertura.Caja.CentroCosto.Id == centroCostoId
                                             group turno by apertura.Id into grupoTurno
                                             select new { Id = grupoTurno.Key, FechaApertura = grupoTurno.Max(x => x.FechaApertura) }
                            on new { apertura.Id, turno.FechaApertura } equals maxTurno
                            select new { apertura.Id, TurnoId = turno.Id, turno.Estado, turno.FechaApertura };


        var query = from caja in EntitySet.Where(x => x.CentroCosto.Id == centroCostoId)
                    join leftApertura in Context.Set<AperturaDia>().Where(x => x.DiaApertura == searchDate) on caja.Id equals leftApertura.Caja.Id into leftAperturas
                    from apertura in leftAperturas.DefaultIfEmpty()
                    join leftSupervisor in Context.Set<Usuario>() on apertura.Supervisor.Id equals leftSupervisor.Id into leftSupervisores
                    from supervisor in leftSupervisores.DefaultIfEmpty()
                    join maxTurno in maxTurnoQuery on apertura.Id equals maxTurno.Id into leftTurnos
                    from turno in leftTurnos.DefaultIfEmpty()
                    select new AperturaCajaInfo
                    {
                        CajaId = caja.Id,
                        Aka = caja.Aka,
                        Descripcion = caja.Descripcion,
                        Equipo = caja.Equipo,
                        CentroCosto = caja.CentroCosto.Descripcion,
                        Franquicia = caja.Franquicia.Descripcion,
                        PuntoVenta = caja.PuntoVenta.Nombre,
                        Id = apertura.Id,
                        DiaApertura = apertura.DiaApertura,
                        FechaApertura = apertura.FechaApertura,
                        Estado = apertura.Estado,
                        FechaCierre = apertura.FechaCierre,
                        EfectivoCierre = apertura.EfectivoCierre,
                        MontoTransbankCierre = apertura.MontoTransbankCierre,
                        Usuario = apertura.Usuario.FriendlyName,
                        Supervisor = supervisor.FriendlyName,
                        Comentario = apertura.Comentario,
                        UltimoTurnoId = turno.TurnoId,
                        UltimoTurnoEstado = turno.Estado,
                        UltimoTurnoFechaApertura = turno.FechaApertura,
                    };

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(x =>
                x.Aka.Contains(searchText) ||
                x.Descripcion.Contains(searchText) ||
                x.Equipo.Contains(searchText) ||
                x.Franquicia.Contains(searchText) ||
                x.PuntoVenta.Contains(searchText) ||
                x.CentroCosto.Contains(searchText));
        }

        query = filterExpressions.Where(query);

        int count = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        var aperturas = await sortExpressions.Sort(query).Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

        return new PagedList<AperturaCajaInfo>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = count,
            Items = aperturas,
        };
    }
}
