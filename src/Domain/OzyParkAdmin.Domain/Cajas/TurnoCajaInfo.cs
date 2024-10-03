using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// La información del turno de una caja.
/// </summary>
public class TurnoCajaInfo
{
    /// <summary>
    /// El id del día.
    /// </summary>
    public Guid DiaId { get; set; }

    /// <summary>
    /// El id del turno.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// El punto de venta.
    /// </summary>
    public string PuntoVenta { get; set; } = string.Empty;

    /// <summary>
    /// La información de la caja asociada al turno.
    /// </summary>
    public CajaInfo Caja { get; set; } = default!;

    /// <summary>
    /// La gaveta asociada al turno.
    /// </summary>
    public GavetaInfo Gaveta { get; set; } = default!;

    /// <summary>
    /// La fecha en que se abrió el turno.
    /// </summary>
    public DateTime FechaApertura { get; set; }

    /// <summary>
    /// La ip de la apertura.
    /// </summary>
    public string? IpAddressApertura { get; set; }

    /// <summary>
    /// La ip del cierre.
    /// </summary>
    public string? IpAddressCierre { get; set; }

    /// <summary>
    /// La fecha de cierre.
    /// </summary>
    public DateTime? FechaCierre { get; set; }

    /// <summary>
    /// El efectivo que se uso como inicio del turno.
    /// </summary>
    public decimal EfectivoInicio { get; set; }

    /// <summary>
    /// El usuario del turno.
    /// </summary>
    public string Usuario { get; set; } = default!;

    /// <summary>
    /// El efectivo declarado por el ejecutivo en el cierre del turno.
    /// </summary>
    public decimal? EfectivoCierreEjecutivo { get; set; }

    /// <summary>
    /// El efectivo calculado por el sistema en el cierre del turno.
    /// </summary>
    public decimal? EfectivoCierreSistema { get; set; }

    /// <summary>
    /// La diferencia entre el <see cref="EfectivoCierreEjecutivo"/> y el <see cref="EfectivoCierreSistema"/>.
    /// </summary>
    public decimal? DiferenciaEfectivo { get; set; }

    /// <summary>
    /// El monto de pagos de tarjeta de crédito y débito declarado por el ejecutivo en el cierre del turno.
    /// </summary>
    public decimal? MontoTransbankEjecutivo { get; set; }

    /// <summary>
    /// El monto de pagos de tarjeta de crédito y débito calculado por el sistema en el cierre del turno.
    /// </summary>
    public decimal? MontoTransbankSistema { get; set; }

    /// <summary>
    /// El efectivo de cierre declarado por el supervisor.
    /// </summary>
    public decimal? EfectivoCierreSupervisor { get; set; }

    /// <summary>
    /// Los montos de tarjeta de crédito y débito declarado por el supervisor.
    /// </summary>
    public decimal? MontoTransbankSupervisor { get; set; }

    /// <summary>
    /// El monto de los vouchers.
    /// </summary>
    public decimal? MontoVoucher { get; set; }

    /// <summary>
    /// La diferencia entre el <see cref="MontoTransbankEjecutivo"/> y el <see cref="MontoTransbankSistema"/>.
    /// </summary>
    public decimal? DiferenciaMontoTransbank { get; set; }

    /// <summary>
    /// El supervisor que realiza la revisión y cierre del turno.
    /// </summary>
    public string? Supervisor { get; set; }

    /// <summary>
    /// Los comentarios colocados por el supervisor al revisar y cerrar el turno.
    /// </summary>
    public string? Comentario { get; set; }

    /// <summary>
    /// El estado del turno.
    /// </summary>
    public EstadoTurno Estado { get; set; }

    /// <summary>
    /// El estado del día.
    /// </summary>
    public EstadoDia EstadoDia { get; set; }

    /// <summary>
    /// El resumen del turno.
    /// </summary>
    public List<ResumenTurnoInfo> Resumen { get; set; } = [];

    /// <summary>
    /// El detalle del turno.
    /// </summary>
    public List<DetalleTurnoInfo> Detalle { get; set; } = [];
}