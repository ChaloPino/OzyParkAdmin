using Microsoft.CodeAnalysis.Differencing;
using MudBlazor;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas.Models;

/// <summary>
/// El model para un turno de caja.
/// </summary>
public sealed record TurnoCajaModel
{
    private decimal? _efectivoSistema;
    private decimal? _transbankSistema;

    /// <summary>
    /// El id del día.
    /// </summary>
    public Guid DiaId { get; set; }

    /// <summary>
    /// El id del turno.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// El punto de venta.
    /// </summary>
    public string PuntoVenta { get; init; } = string.Empty;

    /// <summary>
    /// La caja
    /// </summary>
    public string Caja { get; init; } = string.Empty;

    /// <summary>
    /// La gaveta.
    /// </summary>
    public string Gaveta { get; init; } = string.Empty;

    /// <summary>
    /// El usuario.
    /// </summary>
    public string Usuario { get; init; } = string.Empty;

    /// <summary>
    /// La fecha de inicio.
    /// </summary>
    public DateTime FechaInicio { get; init; }

    /// <summary>
    /// La ip de apertura.
    /// </summary>
    public string? IpAddressApertura { get; init; }

    /// <summary>
    /// La ip de cierre.
    /// </summary>
    public string? IpAddressCierre { get; init; }

    /// <summary>
    /// El monto de inicio.
    /// </summary>
    public decimal MontoInicio { get; init; }

    /// <summary>
    /// La fecha de cierre.
    /// </summary>
    public DateTime? FechaCierre { get; set; }

    /// <summary>
    /// El efectivo de cierre.
    /// </summary>
    public decimal? EfectivoCierre { get; set; }

    /// <summary>
    /// El efectivo de cierre declarado por el supervisor.
    /// </summary>
    public decimal? EfectivoCierreSupervisor { get; set; }

    /// <summary>
    /// Los montos de tarjeta de crédito y débito declarado por el supervisor.
    /// </summary>
    public decimal? MontoTransbankSupervisor { get; set; }

    /// <summary>
    /// Los montos de tarjeta de crédito y débito al cierre.
    /// </summary>
    public decimal? MontoTransbank { get; set; }

    /// <summary>
    /// El monto de voucher.
    /// </summary>
    public decimal? MontoVoucher { get; init; }

    /// <summary>
    /// El efectivo calculado.
    /// </summary>
    public decimal? EfectivoSistema
    {
        get => _efectivoSistema ?? Detalle.Where(x => x.FormaPagoAka == "E").Sum(x => x.MontoConSigno);
        set => _efectivoSistema = value;
    }

    /// <summary>
    /// Los montos de tarjeta de crédito y debito calculados.
    /// </summary>
    public decimal? TransbankSistema
    {
        get => _transbankSistema ?? Detalle.Where(x => x.FormaPagoAka is "C" or "D").Sum(x => x.MontoConSigno);
        set => _transbankSistema = value;
    }

    /// <summary>
    /// La diferencia entre <see cref="EfectivoCierre"/> y <see cref="EfectivoSistema"/>.
    /// </summary>
    public decimal? DiferenciaEfectivo { get; set; }

    /// <summary>
    /// La diferencia entre <see cref="MontoTransbank"/> y <see cref="TransbankSistema"/>.
    /// </summary>
    public decimal? DiferenciaMontoTransbank { get; set; }

    /// <summary>
    /// Número del depósito final cuando hay un tema de regularización.
    /// </summary>
    public string? NumeroDepositoFinal { get; set; }

    /// <summary>
    /// El comentario al momento de cerrar.
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
    public List<ResumenTurnoModel> Resumen { get; init; } = [];

    /// <summary>
    /// El detalle de movimientos del turno.
    /// </summary>
    public List<DetalleTurnoInfo> Detalle { get; init; } = [];
    internal bool Editable { get; set; }
    internal bool PuedeCerrarTurno { get; set; }
    internal bool PuedeReabrirTurno { get; set; }
    internal bool PuedeVisualizarDetalle { get; set; }
    internal bool PuedeVisualizarDetalleCerrado { get; set; }

    internal string Descripcion => $"{FechaInicio} - {Usuario}";

    internal bool ShowRegularizacion => Editable && EstadoDia == EstadoDia.Abierto && Estado != EstadoTurno.Cerrado;

    internal decimal TotalDeposito => Detalle.Where(x => x.Tipo is "DEPOSITO" or "RETIRO").Sum(x => Math.Abs(x.MontoConSigno));

    internal void Update(TurnoCajaInfo turno)
    {
        FechaCierre = turno.FechaCierre;
        EfectivoCierre = turno.EfectivoCierreEjecutivo;
        MontoTransbank = turno.MontoTransbankEjecutivo;
        EfectivoSistema = turno.EfectivoCierreSistema;
        TransbankSistema = turno.MontoTransbankSistema;
        DiferenciaEfectivo = turno.DiferenciaEfectivo;
        DiferenciaMontoTransbank = turno.DiferenciaMontoTransbank;
        EfectivoCierreSupervisor = turno.EfectivoCierreSupervisor;
        MontoTransbankSupervisor = turno.MontoTransbankSupervisor;
        Comentario = turno.Comentario;
        Estado = turno.Estado;
        NumeroDepositoFinal = null;
    }
}
