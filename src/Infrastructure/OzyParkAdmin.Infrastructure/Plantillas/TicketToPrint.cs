using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Tickets;
using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Infrastructure.Plantillas;

/// <summary>
/// La información del ticket para ser impreso.
/// </summary>
public class TicketToPrint
{
    /// <summary>
    /// El id de venta.
    /// </summary>
    public required string ParentId { get; set; }

    /// <summary>
    /// El id del ticket.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// El id de impresión del ticket.
    /// </summary>
    public required string ImpresionId { get; set; }

    /// <summary>
    /// El número de impresión del ticket.
    /// </summary>
    public required string NumeroImpresion { get; set; }

    /// <summary>
    /// La cantidad de reimpresiones.
    /// </summary>
    public required byte NumeroReimpresion { get; set; }

    /// <summary>
    /// Si está usado.
    /// </summary>
    public required bool EsUsado { get; set; }

    /// <summary>
    /// Si se envía por correo.
    /// </summary>
    public required bool EsEnvioCorreo { get; set; }

    /// <summary>
    /// Si se informa al exterior.
    /// </summary>
    public required bool EsInformadoExterno { get; set; }

    /// <summary>
    /// La fecha de impresión.
    /// </summary>
    public required DateTime FechaImpresion { get; set; }

    /// <summary>
    /// La fecha de reimpresión.
    /// </summary>
    public required DateTime? FechaReimpresion { get; set; }

    /// <summary>
    /// La fecha de creación del ticket.
    /// </summary>
    public required DateOnly Fecha { get; set; }

    /// <summary>
    /// La cantidad de tramos.
    /// </summary>
    public required int CantidadTramosTicket { get; set; }

    /// <summary>
    /// La cantidad de tramos usados.
    /// </summary>
    public required int CantidasTramosUsados { get; set; }

    /// <summary>
    /// El inicio de vigencia.
    /// </summary>
    public required DateTime InicioVigencia { get; set; }

    /// <summary>
    /// El fin de vigencia.
    /// </summary>
    public required DateTime FinVigencia { get; set; }

    /// <summary>
    /// El tipo de anulación.
    /// </summary>
    public required string TipoAnulacion { get; set; }

    /// <summary>
    /// Si es de cortesía.
    /// </summary>
    public required bool EsCortesia { get; set; }

    /// <summary>
    /// Si es para un guía.
    /// </summary>
    public required bool EsGuia { get; set; }

    /// <summary>
    /// El servicio del ticket.
    /// </summary>
    public required Servicio Servicio { get; set; }

    /// <summary>
    /// El punto de venta de la caja.
    /// </summary>
    public required string PuntoVenta { get; set; }

    /// <summary>
    /// El centro de costo del servicio.
    /// </summary>
    public int CentroCostoId => Servicio.CentroCostoId ?? 0;

    /// <summary>
    /// El id del tramo.
    /// </summary>
    public required int TramoId { get; set; }

    /// <summary>
    /// El tramo del servicio.
    /// </summary>
    public required Tramo Tramo { get; set; }

    /// <summary>
    /// El tipo de pasajero.
    /// </summary>
    public required TipoPasajero? TipoPasajero { get; set; }

    /// <summary>
    /// El nombre del usuario.
    /// </summary>
    public required string UsuarioNombre { get; set; }

    /// <summary>
    /// el id de la zona de origen.
    /// </summary>
    public required int? ZonaOrigenId { get; set; }

    /// <summary>
    /// La zona de origen.
    /// </summary>
    public required Zona? ZonaOrigen { get; set; }

    /// <summary>
    /// El id de la zona destino.
    /// </summary>
    public required int? ZonaDesintoId { get; set; }

    /// <summary>
    /// La zona de destino.
    /// </summary>
    public required Zona? ZonaDestino { get; set; }

    /// <summary>
    /// El id de la zona de origen para el cupo.
    /// </summary>
    public required int? ZonaCupoOridenId { get; set; }

    /// <summary>
    /// La zona de origen para el cupo.
    /// </summary>
    public required Zona? ZonaCupoOrigen { get; set; }

    /// <summary>
    /// El id del sentido del ticket.
    /// </summary>
    public required int? SentidoId { get; set; }

    /// <summary>
    /// El sentido del ticket.
    /// </summary>
    public required Sentido? Sentido { get; set; }

    /// <summary>
    /// El código QR del ticket.
    /// </summary>
    public required string Codigo { get; set; }

    /// <summary>
    /// El formato para el código QR.
    /// </summary>
    public required string CodigoFormato { get; set; }

    /// <summary>
    /// El id del tipo de segmentacion.
    /// </summary>
    public required int TipoSegmentacionId { get; set; }

    /// <summary>
    /// El tipo de segmentación.
    /// </summary>
    public required TipoSegmentacion TipoSegmentacion { get; set; }

    /// <summary>
    /// El detalle del ticket.
    /// </summary>
    public required IEnumerable<DetalleTicket> Detalles { get; set; }

    /// <summary>
    /// La hora de apertura.
    /// </summary>
    public TimeSpan HoraApertura => Servicio.CentroCosto.ConseguirHoraApertura(DateOnly.FromDateTime(InicioVigencia));

    /// <summary>
    /// La hora de cierre.
    /// </summary>
    public TimeSpan HoraCierre => Servicio.CentroCosto.ConseguirHoraCierre(DateOnly.FromDateTime(InicioVigencia));

}