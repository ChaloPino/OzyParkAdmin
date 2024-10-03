using MudBlazor;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas.Models;

/// <summary>
/// El view model de la apertura de caja.
/// </summary>
public sealed record AperturaCajaViewModel
{
    /// <summary>
    /// El id de la caja.
    /// </summary>
    public int CajaId { get; set; }

    /// <summary>
    /// El id de la apertura de caja
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// El aka de la caja.
    /// </summary>
    public string Aka { get; set; } = string.Empty;

    /// <summary>
    /// El nombre de la caja.
    /// </summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// El nombre del equipo asociado a la caja.
    /// </summary>
    public string? Equipo { get; set; } = string.Empty;

    /// <summary>
    /// El centro de costo asociado a la caja.
    /// </summary>
    public string CentroCosto { get; set; } = string.Empty;

    /// <summary>
    /// La franquicia asociada a la caja.
    /// </summary>
    public string Franquicia { get; set; } = string.Empty;

    /// <summary>
    /// El punto de venta asociado a la caja.
    /// </summary>
    public string PuntoVenta { get; set; } = string.Empty;

    /// <summary>
    /// El día de apertura de la caja.
    /// </summary>
    public DateOnly? DiaApertura { get; set; }

    /// <summary>
    /// La fecha de apertura de la caja.
    /// </summary>
    public DateTime? FechaApertura { get; set; }

    /// <summary>
    /// El estado de la apertura de la caja.
    /// </summary>
    public EstadoDia? Estado { get; set; }

    /// <summary>
    /// La fecha de cierre de la caja.
    /// </summary>
    public DateTime? FechaCierre { get; set; }

    /// <summary>
    /// El efectivo calculado en cierre.
    /// </summary>
    public decimal? EfectivoCierre { get; set; }

    /// <summary>
    /// El monto de las tarjetas de crédito y débito calculado en cierre.
    /// </summary>
    public decimal? MontoTransbankCierre { get; set; }

    /// <summary>
    /// Usuario que realizó la apertura del día.
    /// </summary>
    public string? Usuario { get; set; }

    /// <summary>
    /// El supervisor que realizó el cierre del día.
    /// </summary>
    public string? Supervisor { get; set; }

    /// <summary>
    /// Los comentarios de cierre.
    /// </summary>
    public string? Comentario { get; set; }

    /// <summary>
    /// Último turno de la apertura del día.
    /// </summary>
    public Guid? UltimoTurnoId { get; set; }

    /// <summary>
    /// Estado del último turno.
    /// </summary>
    public EstadoTurno? UltimoTurnoEstado { get; set; }

    /// <summary>
    /// Fecha de apertura del último turno.
    /// </summary>
    public DateTime? UltimoTurnoFechaApertura { get; internal set; }

    internal TimeSpan? HoraApertura => FechaApertura?.TimeOfDay;

    internal Color UltimoTurnoEstadoColor =>
        UltimoTurnoEstado switch
        {
            EstadoTurno.Abierto => Color.Error,
            EstadoTurno.Pendiente => Color.Warning,
            EstadoTurno.Cerrado => Color.Success,
            _ => Color.Default,
        };

    internal Color EstadoColor =>
        Estado switch
        {
            EstadoDia.Abierto => Color.Error,
            EstadoDia.Cerrado => Color.Success,
            _ => Color.Default,
        };

    /// <summary>
    /// Los turnos del día.
    /// </summary>
    public List<TurnoCajaModel> Turnos { get; set; } = [];

    /// <summary>
    /// Los servicios vendidos en el día.
    /// </summary>
    public List<ServicioDiaInfo> Servicios { get; set; } = [];

    internal decimal MontoEfectivoParaCierre =>
        Estado == EstadoDia.Abierto
        ? Turnos.Sum(turno => turno.EfectivoCierreSupervisor) ?? 0
        : EfectivoCierre ?? 0;

    internal decimal MontoTransbankParaCierre =>
        Estado == EstadoDia.Abierto
        ? Turnos.Sum(turno => turno.MontoTransbankSupervisor) ?? 0
        : MontoTransbankCierre ?? 0;

    internal decimal MontoVoucher => Turnos.Sum(x => x.MontoVoucher) ?? 0;

    internal decimal TotalDeposito => Turnos.Sum(x => x.TotalDeposito);

    internal bool DiaAbiertoTurnosCerrados => Estado == EstadoDia.Abierto && (Turnos.Count == 0 || Turnos.TrueForAll(x => x.Estado == EstadoTurno.Cerrado));

    internal bool Editable { get; set; }

    internal bool PuedeCerrarDia { get; set; }

    internal bool PuedeReabrirDia { get; set; }

    internal void Update(AperturaCajaInfo apertura)
    {
        FechaCierre = apertura.FechaCierre;
        Supervisor = apertura.Supervisor;
        EfectivoCierre = apertura.EfectivoCierre;
        MontoTransbankCierre = apertura.MontoTransbankCierre;
        Comentario = apertura.Comentario;
        Estado = apertura.Estado;

        foreach (var turno in Turnos)
        {
            turno.EstadoDia = apertura.Estado!.Value;
        }
    }

    internal void TryUpdateLastShift(TurnoCajaModel turno)
    {
        if (turno.Id == UltimoTurnoId)
        {
            UltimoTurnoEstado = turno.Estado;
        }
    }
}
