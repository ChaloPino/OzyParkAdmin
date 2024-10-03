using MudBlazor;
using MudBlazor.Interfaces;
using OzyParkAdmin.Application.Cajas.Search;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Seguridad.Permisos;
using OzyParkAdmin.Shared;
using System.Diagnostics;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas.Models;

internal static class CajaMappers
{
    public static SearchAperturasCaja ToSearch(this GridState<AperturaCajaViewModel> state, string? searchText, DateTime? searchDate, CentroCostoInfo centroCosto) =>
        new(centroCosto?.Id ?? 0, searchText, searchDate.ToDateOnly(), state.ToFilterExpressions(searchDate), state.ToSortExpressions(), state.Page, state.PageSize);

    private static DateOnly ToDateOnly(this DateTime? date) =>
        date is null ? DateOnly.FromDateTime(DateTime.Today) : DateOnly.FromDateTime(date.Value);

    private static FilterExpressionCollection<AperturaCajaInfo> ToFilterExpressions(this GridState<AperturaCajaViewModel> state, DateTime? date)
    {
        FilterExpressionCollection<AperturaCajaInfo> filterExpressions = new();

        foreach (IFilterDefinition<AperturaCajaViewModel> filterDefinition in state.FilterDefinitions)
        {
            filterDefinition.ToFilterExpression(filterExpressions, date);
        }

        return filterExpressions;
    }

    private static void ToFilterExpression(this IFilterDefinition<AperturaCajaViewModel> filterDefinition, FilterExpressionCollection<AperturaCajaInfo> filterExpressions, DateTime? date)
    {
        _ = filterDefinition.Column!.PropertyName switch
        {
            nameof(AperturaCajaViewModel.Id) => filterExpressions.Add(x => x.Id, filterDefinition.Operator!, filterDefinition.Value),
            nameof(AperturaCajaViewModel.Aka) => filterExpressions.Add(x => x.Aka, filterDefinition.Operator!, filterDefinition.Value),
            nameof(AperturaCajaViewModel.Descripcion) => filterExpressions.Add(x => x.Descripcion, filterDefinition.Operator!, filterDefinition.Value),
            nameof(AperturaCajaViewModel.Equipo) => filterExpressions.Add(x => x.Equipo, filterDefinition.Operator!, filterDefinition.Value),
            nameof(AperturaCajaViewModel.CentroCosto) => filterExpressions.Add(x => x.CentroCosto, filterDefinition.Operator!, filterDefinition.Value),
            nameof(AperturaCajaViewModel.Franquicia) => filterExpressions.Add(x => x.Franquicia, filterDefinition.Operator!, filterDefinition.Value),
            nameof(AperturaCajaViewModel.PuntoVenta) => filterExpressions.Add(x => x.PuntoVenta, filterDefinition.Operator!, filterDefinition.Value),
            nameof(AperturaCajaViewModel.DiaApertura) => filterExpressions.Add(x => x.DiaApertura, filterDefinition.Operator!, filterDefinition.Value),
            "FechaApertura.TimeOfDay" => filterExpressions.CreateFechaAperturaFilter(filterDefinition.Operator!, date, filterDefinition.Value),
            nameof(AperturaCajaViewModel.Estado) => filterExpressions.Add(x => x.Estado, filterDefinition.Operator!, filterDefinition.Value),
            _ => throw new UnreachableException(),
        };
    }

    private static FilterExpressionCollection<AperturaCajaInfo> CreateFechaAperturaFilter(this FilterExpressionCollection<AperturaCajaInfo> filterExpressions, string @operator, DateTime? date, object? value)
    {
        if (date is not null && value is TimeSpan timeOfDay)
        {
            DateTime dateAndTime = date.Value.Add(timeOfDay);
            filterExpressions.Add(x => x.FechaApertura, @operator, dateAndTime);
        }

        return filterExpressions;
    }

    private static SortExpressionCollection<AperturaCajaInfo> ToSortExpressions(this GridState<AperturaCajaViewModel> state)
    {
        SortExpressionCollection<AperturaCajaInfo> sortExpressions = new();

        foreach (SortDefinition<AperturaCajaViewModel> sortDefinition in state.SortDefinitions)
        {
            sortDefinition.ToSortExpression(sortExpressions);
        }

        return sortExpressions;
    }

    private static void ToSortExpression(this SortDefinition<AperturaCajaViewModel> sortDefinition, SortExpressionCollection<AperturaCajaInfo> sortExpressions)
    {
        _ = sortDefinition.SortBy switch
        {
            nameof(AperturaCajaViewModel.Id) => sortExpressions.Add(x => x.Id, sortDefinition.Descending),
            nameof(AperturaCajaViewModel.Aka) => sortExpressions.Add(x => x.Aka, sortDefinition.Descending),
            nameof(AperturaCajaViewModel.Descripcion) => sortExpressions.Add(x => x.Descripcion, sortDefinition.Descending),
            nameof(AperturaCajaViewModel.Equipo) => sortExpressions.Add(x => x.Equipo, sortDefinition.Descending),
            nameof(AperturaCajaViewModel.CentroCosto) => sortExpressions.Add(x => x.CentroCosto, sortDefinition.Descending),
            nameof(AperturaCajaViewModel.Franquicia) => sortExpressions.Add(x => x.Franquicia, sortDefinition.Descending),
            nameof(AperturaCajaViewModel.PuntoVenta) => sortExpressions.Add(x => x.PuntoVenta, sortDefinition.Descending),
            nameof(AperturaCajaViewModel.DiaApertura) => sortExpressions.Add(x => x.DiaApertura, sortDefinition.Descending),
            "FechaApertura.TimeOfDay" => sortExpressions.Add(x => x.FechaApertura, sortDefinition.Descending),
            nameof(AperturaCajaViewModel.Estado) => sortExpressions.Add(x => x.Estado, sortDefinition.Descending),
            _ => throw new UnreachableException(),
        };
    }

    public static ObservableGridData<AperturaCajaViewModel> ToGridData(this PagedList<AperturaCajaInfo> source, IMudStateHasChanged stateHasChanged, CajaAcciones acciones) =>
        new ObservableGridData<AperturaCajaViewModel, int>(source.Items.Select(x => x.ToViewModel(acciones)), source.TotalCount, stateHasChanged, x => x.CajaId);

    private static AperturaCajaViewModel ToViewModel(this AperturaCajaInfo aperturaCaja, CajaAcciones acciones) =>
        new()
        {
            Id = aperturaCaja.Id,
            CajaId = aperturaCaja.CajaId,
            Aka = aperturaCaja.Aka,
            Descripcion = aperturaCaja.Descripcion,
            Equipo = aperturaCaja.Equipo,
            CentroCosto = aperturaCaja.CentroCosto,
            Franquicia = aperturaCaja.Franquicia,
            PuntoVenta = aperturaCaja.PuntoVenta,
            DiaApertura = aperturaCaja.DiaApertura,
            FechaApertura = aperturaCaja.FechaApertura,
            Estado = aperturaCaja.Estado,
            FechaCierre = aperturaCaja.FechaCierre,
            EfectivoCierre = aperturaCaja.EfectivoCierre,
            MontoTransbankCierre = aperturaCaja.EfectivoCierre,
            Usuario = aperturaCaja.Usuario,
            Supervisor = aperturaCaja.Supervisor,
            Comentario = aperturaCaja.Comentario,
            UltimoTurnoId = aperturaCaja.UltimoTurnoId,
            UltimoTurnoEstado = aperturaCaja.UltimoTurnoEstado,
            UltimoTurnoFechaApertura = aperturaCaja.UltimoTurnoFechaApertura,
            Editable = PermisoCajaService.HasAction(acciones, CajaAcciones.Editar),
            PuedeCerrarDia = PermisoCajaService.HasAction(acciones, CajaAcciones.CerrarDia),
            PuedeReabrirDia = PermisoCajaService.HasAction(acciones, CajaAcciones.ReabrirDia),
        };

    public static List<TurnoCajaModel> ToModel(this IEnumerable<TurnoCajaInfo> source, CajaAcciones acciones) =>
        [.. source.Select(x => x.ToModel(acciones))];

    private static TurnoCajaModel ToModel(this TurnoCajaInfo turno, CajaAcciones acciones) =>
        new()
        {
            DiaId = turno.DiaId,
            Id = turno.Id,
            Caja = turno.Caja.Aka,
            Comentario = turno.Comentario,
            Detalle = [.. turno.Detalle.OrderBy(x => x.Fecha).ThenBy(x => x.Hora).ThenBy(x => x.Orden)],
            DiferenciaEfectivo = turno.DiferenciaEfectivo,
            DiferenciaMontoTransbank = turno.DiferenciaMontoTransbank,
            EfectivoCierre = turno.EfectivoCierreEjecutivo,
            EfectivoSistema = turno.EfectivoCierreSistema,
            EfectivoCierreSupervisor = turno.EfectivoCierreSupervisor,
            MontoTransbankSupervisor = turno.MontoTransbankSupervisor,
            Estado = turno.Estado,
            EstadoDia = turno.EstadoDia,
            FechaCierre = turno.FechaCierre,
            FechaInicio = turno.FechaApertura,
            Gaveta = turno.Gaveta.Aka,
            IpAddressApertura = turno.IpAddressApertura,
            IpAddressCierre = turno.IpAddressCierre,
            MontoInicio = turno.EfectivoInicio,
            MontoTransbank = turno.MontoTransbankEjecutivo,
            TransbankSistema = turno.MontoTransbankSistema,
            MontoVoucher = turno.MontoVoucher,
            PuntoVenta = turno.PuntoVenta,
            Usuario = turno.Usuario,
            Resumen = turno.Resumen.ToModel(turno.Detalle),
            Editable = PermisoCajaService.HasAction(acciones, CajaAcciones.Editar),
            PuedeCerrarTurno = PermisoCajaService.HasAction(acciones, CajaAcciones.CerrarTurno),
            PuedeReabrirTurno = PermisoCajaService.HasAction(acciones, CajaAcciones.ReabrirTurno),
            PuedeVisualizarDetalle = PermisoCajaService.HasAction(acciones, CajaAcciones.VisualizarDetalleTurno),
            PuedeVisualizarDetalleCerrado = PermisoCajaService.HasAction(acciones, CajaAcciones.VisualizarDetalleTurnoCerrado),
        };

    private static List<ResumenTurnoModel> ToModel(this IEnumerable<ResumenTurnoInfo> source, IEnumerable<DetalleTurnoInfo> detalle) =>
        [.. source.Select(x => x.ToModel(detalle))];

    private static ResumenTurnoModel ToModel(this ResumenTurnoInfo resumen, IEnumerable<DetalleTurnoInfo> detalle) =>
        new(resumen, detalle);
}
