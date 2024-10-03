using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// El turno de un caja.
/// </summary>
public sealed class AperturaTurno
{
    private readonly List<MovimientoTurno> _movimientos = [];

    /// <summary>
    /// El id del turno.
    /// </summary>
    public Guid Id { get; private init; }

    /// <summary>
    /// La gaveta asociada al turno.
    /// </summary>
    public Gaveta Gaveta { get; private init; }

    /// <summary>
    /// La fecha en que se abrió el turno.
    /// </summary>
    public DateTime FechaApertura { get; private init; }

    /// <summary>
    /// Fecha de creación del turno.
    /// </summary>
    public DateTime FechaSistema { get; private init; }

    /// <summary>
    /// La ip de apertura.
    /// </summary>
    public string? IpAddressApertura { get; private set; }

    /// <summary>
    /// La ip de cierre.
    /// </summary>
    public string? IpAddressCierre { get; private set; }

    /// <summary>
    /// El efectivo que se uso como inicio del turno.
    /// </summary>
    public decimal EfectivoInicio { get; private init; }

    /// <summary>
    /// El usuario del turno.
    /// </summary>
    public Usuario Usuario { get; private init; } = default!;

    /// <summary>
    /// La fecha en que se cerró el turno.
    /// </summary>
    public DateTime? FechaCierre { get; private set; }

    /// <summary>
    /// El efectivo declarado por el ejecutivo en el cierre del turno.
    /// </summary>
    public decimal? EfectivoCierreEjecutivo { get; private set; }

    /// <summary>
    /// El efectivo calculado por el sistema en el cierre del turno.
    /// </summary>
    public decimal? EfectivoCierreSistema { get; private set; }

    /// <summary>
    /// El efectivo declarado por el supervisor al cerrar el turno.
    /// </summary>
    public decimal? EfectivoCierreSupervisor { get; private set; }

    /// <summary>
    /// La diferencia entre el <see cref="EfectivoCierreEjecutivo"/> y el <see cref="EfectivoCierreSistema"/>.
    /// </summary>
    public decimal? DiferenciaEfectivo { get; private set; }

    /// <summary>
    /// El monto de pagos de tarjeta de crédito y débito declarado por el ejecutivo en el cierre del turno.
    /// </summary>
    public decimal? MontoTransbankEjecutivo { get; private set; }

    /// <summary>
    /// El monto de pagos de tarjeta de crédito y débito calculado por el sistema en el cierre del turno.
    /// </summary>
    public decimal? MontoTransbankSistema { get; private set; }

    /// <summary>
    /// El monto de pagos de tarjeta de crédito y débito declarado por el supervisor al cierre.
    /// </summary>
    public decimal? MontoTransbankSupervisor { get; private set; }

    /// <summary>
    /// La diferencia entre el <see cref="MontoTransbankEjecutivo"/> y el <see cref="MontoTransbankSistema"/>.
    /// </summary>
    public decimal? DiferenciaMontoTransbank { get; private set; }

    /// <summary>
    /// El supervisor que realiza la revisión y cierre del turno.
    /// </summary>
    public Usuario? Supervisor { get; private set; }

    /// <summary>
    /// Valor que indica si el cierre se realizó por el ejecutivo.
    /// </summary>
    public bool? CerradoPorCajero { get; private set; }

    /// <summary>
    /// Los comentarios colocados por el supervisor al revisar y cerrar el turno.
    /// </summary>
    public string? Comentario { get; private set; }

    /// <summary>
    /// El estado del turno.
    /// </summary>
    public EstadoTurno Estado { get; set; }

    /// <summary>
    /// Los movimientos realizados en el turno.
    /// </summary>
    public IEnumerable<MovimientoTurno> Movimientos => _movimientos;

    internal void Cerrar(decimal regularizacionEfectivo, decimal regularizacionMontoTransbank, Usuario supervisor, string comentario, IEnumerable<DetalleTurnoInfo> movimientos)
    {
        if (Estado == EstadoTurno.Abierto)
        {
            EfectivoCierreEjecutivo = regularizacionEfectivo;
            MontoTransbankEjecutivo = regularizacionMontoTransbank;
            EfectivoCierreSistema = movimientos.Where(x => x.FormaPagoAka == "E").Sum(x => x.MontoConSigno);
            MontoTransbankSistema = movimientos.Where(x => x.FormaPagoAka == "C" || x.FormaPagoAka == "D").Sum(x => x.MontoConSigno);
            CerradoPorCajero = false;
            FechaCierre = DateTime.Now;
        }

        Supervisor = supervisor;
        EfectivoCierreSupervisor = regularizacionEfectivo;
        MontoTransbankSupervisor = regularizacionMontoTransbank;

        Comentario = comentario;
        Estado = EstadoTurno.Cerrado;
    }

    internal void Reabrir()
    {
        MovimientoTurno? deposito = _movimientos.Where(x =>
            x.TipoTurnoMovimiento == TipoMovimientoTurno.Deposito &&
            x.Estado == EstadoMovimientoTurno.Activo &&
            x.Supervisor is not null)
            .OrderByDescending(x => x.Fecha)
            .FirstOrDefault();

        if (deposito is not null)
        {
            _movimientos.Remove(deposito);
        }

        EfectivoCierreSupervisor = null;
        MontoTransbankSupervisor = null;
        Supervisor = null;
        Comentario = null;

        if (CerradoPorCajero is not null && CerradoPorCajero.Value)
        {
            Estado = EstadoTurno.Pendiente;
            return;
        }

        Estado = EstadoTurno.Abierto;
        EfectivoCierreEjecutivo = null;
        EfectivoCierreSistema = null;
        MontoTransbankSistema = null;
        MontoTransbankEjecutivo = null;
        FechaCierre = null;
        CerradoPorCajero = null;
    }
}