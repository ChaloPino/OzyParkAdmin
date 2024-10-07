using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Tramos;
using OzyParkAdmin.Domain.Zonas;
using System.Security.Cryptography;

namespace OzyParkAdmin.Domain.Tickets;

/// <summary>
/// La entidad ticket.
/// </summary>
public sealed class Ticket
{
    private readonly List<TicketReimpreso> _reimpresiones = [];
    private readonly List<DetalleTicket> _detalle = [];

    /// <summary>
    /// El id del ticket.
    /// </summary>
    public string Id { get; private init; } = string.Empty;

    /// <summary>
    /// El id de impresión del ticket.
    /// </summary>
    public string ImpresionId { get; private init; } = string.Empty;

    /// <summary>
    /// El número de impresión del ticket.
    /// </summary>
    public string NumeroImpresion { get; private init; } = string.Empty;

    /// <summary>
    /// Si el ticket está usado.
    /// </summary>
    public bool EsUsado { get; private init; }

    /// <summary>
    /// Si el ticket fue enviado por correo electrónico.
    /// </summary>
    public bool EsEnvioCorreo { get; private init; }

    /// <summary>
    /// Si el ticket fue informado de manera externa.
    /// </summary>
    public bool EsInformadoExterno { get; private init; }

    /// <summary>
    /// Fecha y hora en que se imprimió el ticket.
    /// </summary>
    public DateTime FechaImpresion { get; private init; }

    /// <summary>
    /// Fecha en que se creó el ticket.
    /// </summary>
    public DateOnly Fecha { get; private init; }

    /// <summary>
    /// La cantidad de tramos que tiene el ticket.
    /// </summary>
    public int CantidadTramosTicket { get; private init; }

    /// <summary>
    /// La cantidad de tramos usados.
    /// </summary>
    public int CantidadTramosUsados { get; private init; }

    /// <summary>
    /// El id del servicio asociado al ticket.
    /// </summary>
    public int ServicioId { get; private init; }

    /// <summary>
    /// El servicio asociado al ticket.
    /// </summary>
    public Servicio Servicio { get; private init; } = default!;

    /// <summary>
    /// El id del tramo asociado al ticket.
    /// </summary>
    public int TramoId { get; private init; }

    /// <summary>
    /// El tramo asociado al ticket.
    /// </summary>
    public Tramo Tramo { get; private init; } = default!;

    /// <summary>
    /// El inicio de vigencia del ticket.
    /// </summary>
    public DateTime InicioVigencia { get; private init; }

    /// <summary>
    /// El fin de vigencia del ticket.
    /// </summary>
    public DateTime FinVigencia { get; private init; }

    /// <summary>
    /// La cantidad de reimpresiones del ticket.
    /// </summary>
    public byte NumeroReimpresion { get; private set; }

    /// <summary>
    /// La última fecha en que se reimprimió el ticket.
    /// </summary>
    public DateTime? FechaReimpresion { get; private set; }

    /// <summary>
    /// El usuario que creó el ticket.
    /// </summary>
    public Usuario Usuario { get; private init; } = default!;

    /// <summary>
    /// El tipo de anulación.
    /// </summary>
    public string TipoAnulacion { get; private init; } = "N";

    /// <summary>
    /// El id del tipo de pasajero del ticket.
    /// </summary>
    public int? TipoPasajeroId { get; private init; }

    /// <summary>
    /// El tipo de pasajero del ticket.
    /// </summary>
    public TipoPasajero? TipoPasajero { get; private init; }

    /// <summary>
    /// El el ticket es de cortesía.
    /// </summary>
    public bool EsCortesia { get; private init; }

    /// <summary>
    /// Si el ticket es para el guía.
    /// </summary>
    public bool EsGuia { get; private init; }

    /// <summary>
    /// El formato para generar el código QR.
    /// </summary>
    public string CodigoFormato { get; private init; } = string.Empty;

    /// <summary>
    /// El código QR generado para el ticket.
    /// </summary>
    public string Codigo { get; private init; } = string.Empty;

    /// <summary>
    /// El id de la zona de origen del ticket.
    /// </summary>
    public int? ZonaOrigenId { get; private init; }

    /// <summary>
    /// La zona de origen del ticket.
    /// </summary>
    public Zona? ZonaOrigen { get; private init; }

    /// <summary>
    /// El id de la zona de destino del ticket.
    /// </summary>
    public int? ZonaDestinoId { get; private init; }

    /// <summary>
    /// La zona destino del ticket.
    /// </summary>
    public Zona? ZonaDestino { get; private init; }

    /// <summary>
    /// El id de la zona de origen para el cupo.
    /// </summary>
    public int? ZonaCupoOrigenId { get; private init; }

    /// <summary>
    /// La zona de origen para el cupo.
    /// </summary>
    public Zona? ZonaCupoOrigen { get; private init; }

    /// <summary>
    /// El id del sentido del ticket.
    /// </summary>
    public int? SentidoId { get; private init; }

    /// <summary>
    /// El sentido del ticket.
    /// </summary>
    public Sentido? Sentido { get; private init; }

    /// <summary>
    /// El id del tipo de segmentación del ticket.
    /// </summary>
    public int TipoSegmentacionId { get; private init; }

    /// <summary>
    /// El tipo de segmentación del ticket.
    /// </summary>
    public TipoSegmentacion TipoSegmenetacion { get; private init; } = default!;

    /// <summary>
    /// El detalle del ticket.
    /// </summary>
    public IEnumerable<DetalleTicket> Detalle => _detalle;

    internal void MarkReprint(Usuario usuario, string ip)
    {
        NumeroReimpresion++;
        FechaReimpresion = DateTime.Now;
        _reimpresiones.Add(TicketReimpreso.Create(this, usuario, ip));
    }
}
