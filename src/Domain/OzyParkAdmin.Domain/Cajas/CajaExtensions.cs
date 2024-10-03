using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;

namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// Contiene métodos de extensión para <see cref="Caja"/>.
/// </summary>
public static class CajaExtensions
{
    /// <summary>
    /// Convierte una lista de <see cref="Caja"/> a una lista inmutable de <see cref="CajaInfo"/>.
    /// </summary>
    /// <param name="source">La lista de <see cref="Caja"/> a convertir.</param>
    /// <returns>La lista inmutable de <see cref="CajaInfo"/> convertida desde <paramref name="source"/>.</returns>
    public static ImmutableArray<CajaInfo> ToInfo(this IEnumerable<Caja> source) =>
        [.. source.Select(ToInfo)];

    /// <summary>
    /// Convierte un <see cref="Caja"/> a <see cref="CajaInfo"/>.
    /// </summary>
    /// <param name="caja">La <see cref="Caja"/> a convertir.</param>
    /// <returns>La <see cref="CajaInfo"/> convertida desde <paramref name="caja"/>.</returns>
    public static CajaInfo ToInfo(this Caja caja) =>
        new() { Id = caja.Id, Aka = caja.Aka, Descripcion = caja.Descripcion };

    /// <summary>
    /// Convierte una <see cref="AperturaDia"/> a <see cref="AperturaCajaInfo"/>.
    /// </summary>
    /// <param name="apertura">La <see cref="AperturaDia"/> a convertir.</param>
    /// <returns>La <see cref="AperturaCajaInfo"/> convertida desde <paramref name="apertura"/>. </returns>
    public static AperturaCajaInfo ToInfo(this AperturaDia apertura) =>
        new()
        {
            Id = apertura.Id,
            CajaId = apertura.Caja.Id,
            Aka = apertura.Caja.Aka,
            Descripcion = apertura.Caja.Descripcion,
            CentroCosto = apertura.Caja.CentroCosto.Descripcion,
            Franquicia = apertura.Caja.Franquicia.Descripcion,
            PuntoVenta = apertura.Caja.PuntoVenta.Nombre,
            Equipo = apertura.Caja.Equipo,
            DiaApertura = apertura.DiaApertura,
            FechaApertura = apertura.FechaApertura,
            Usuario = apertura.Usuario.FriendlyName,
            FechaCierre = apertura.FechaCierre,
            EfectivoCierre = apertura.EfectivoCierre,
            MontoTransbankCierre = apertura.MontoTransbankCierre,
            Estado = apertura.Estado,
            Comentario = apertura.Comentario,
            Supervisor = apertura.Supervisor?.FriendlyName,
        };

    /// <summary>
    /// Convierte el turno de la <paramref name="aperturaDia"/> usando <paramref name="turnoId"/> a <see cref="TurnoCajaInfo"/>.
    /// </summary>
    /// <param name="aperturaDia">La <see cref="AperturaDia"/> donde se buscará el turno.</param>
    /// <param name="turnoId">El id del turno a buscar.</param>
    /// <param name="movimientos">Los movimientos del turno.</param>
    /// <returns>El resultado de convertir a <see cref="TurnoCajaInfo"/> convertido desde <paramref name="aperturaDia"/> usando <paramref name="turnoId"/>.</returns>
    public static ResultOf<TurnoCajaInfo> ToTurnoInfo(this AperturaDia aperturaDia, Guid turnoId, IEnumerable<DetalleTurnoInfo> movimientos)
    {
        AperturaTurno? turno = aperturaDia.FindTurno(turnoId);

        if (turno is null)
        {
            return new NotFound();
        }

        return turno.ToInfo(aperturaDia, movimientos);

    }

    private static TurnoCajaInfo ToInfo(this AperturaTurno turno, AperturaDia aperturaDia, IEnumerable<DetalleTurnoInfo> movimientos) =>
        new()
        {
            DiaId = aperturaDia.Id,
            Id = turno.Id,
            Caja = new CajaInfo { Id = aperturaDia.Caja.Id, Aka = aperturaDia.Caja.Aka, Descripcion = aperturaDia.Caja.Descripcion, },
            Gaveta = new GavetaInfo { Id = turno.Gaveta.Id, Aka = turno.Gaveta.Aka, },
            FechaApertura = turno.FechaApertura,
            EfectivoInicio = turno.EfectivoInicio,
            IpAddressApertura = turno.IpAddressApertura,
            IpAddressCierre = turno.IpAddressCierre,
            EfectivoCierreEjecutivo = turno.EfectivoCierreEjecutivo,
            MontoTransbankEjecutivo = turno.MontoTransbankEjecutivo,
            EfectivoCierreSistema = turno.EfectivoCierreSistema,
            MontoTransbankSistema = turno.MontoTransbankSistema,
            DiferenciaEfectivo = turno.DiferenciaEfectivo,
            DiferenciaMontoTransbank = turno.DiferenciaMontoTransbank,
            FechaCierre = turno.FechaCierre,
            PuntoVenta = aperturaDia.Caja.PuntoVenta.Nombre,
            Supervisor = turno.Supervisor?.FriendlyName,
            Usuario = turno.Usuario.FriendlyName,
            EfectivoCierreSupervisor = turno.EfectivoCierreSupervisor,
            MontoTransbankSupervisor = turno.MontoTransbankSupervisor,
            MontoVoucher = movimientos.Where(x => x.FormaPagoAka == "V").Sum(x => x.Monto),
            Estado = turno.Estado,
            EstadoDia = aperturaDia.Estado,
            Comentario = turno.Comentario,
            Resumen = movimientos.ToResumen(),
            Detalle = movimientos.ToList(),
        };

    private static List<ResumenTurnoInfo> ToResumen(this IEnumerable<DetalleTurnoInfo> source) =>
        source.GroupBy(ToResumen).Select(ToResumen).ToList();

    private static ResumenTurnoInfo ToResumen(this DetalleTurnoInfo movimiento) =>
        new() { EsAbono = movimiento.EsAbono, Tipo = movimiento.Tipo, Movimiento = movimiento.Movimiento, };

    private static ResumenTurnoInfo ToResumen(this IGrouping<ResumenTurnoInfo, DetalleTurnoInfo> grupo) =>
        new()
        {
            EsAbono = grupo.Key.EsAbono,
            Tipo = grupo.Key.Tipo,
            Movimiento = grupo.Key.Movimiento,
            Cantidad = grupo.Count(),
            Monto = grupo.Sum(x => x.MontoConSigno),
        };
}
