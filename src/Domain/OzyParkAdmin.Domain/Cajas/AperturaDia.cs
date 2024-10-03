using OzyParkAdmin.Domain.Seguridad.Usuarios;
using System.ComponentModel;

namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// La apertura diaria de una caja.
/// </summary>
public sealed class AperturaDia
{
    private readonly List<AperturaTurno> _turnos = [];

    /// <summary>
    /// El id de una apertura diaria de caja.
    /// </summary>
    public Guid Id { get; private init; }

    /// <summary>
    /// La caja de la apertura.
    /// </summary>
    public Caja Caja { get; private init; }

    /// <summary>
    /// El día de apertura de la caja.
    /// </summary>
    public DateOnly DiaApertura { get; private init; }

    /// <summary>
    /// La fecha de apertura de la caja.
    /// </summary>
    public DateTime FechaApertura { get; private init; }

    /// <summary>
    /// El usuario que aperturó la caja.
    /// </summary>
    public Usuario Usuario { get; private init; } = default!;

    /// <summary>
    /// El supervisor que cerró el día de la caja.
    /// </summary>
    public Usuario? Supervisor { get; private set; } = default!;

    /// <summary>
    /// La fecha de cierre.
    /// </summary>
    public DateTime? FechaCierre { get; private set; }

    /// <summary>
    /// El efectivo de cierre.
    /// </summary>
    public decimal? EfectivoCierre { get; private set; }

    /// <summary>
    /// El monto de tarjetas de crédito y débito al cierre.
    /// </summary>
    public decimal? MontoTransbankCierre { get; private set; }

    /// <summary>
    /// El estado de la apertura de la caja.
    /// </summary>
    public EstadoDia Estado { get; private set; }

    /// <summary>
    /// El comentario al cierre.
    /// </summary>
    public string? Comentario { get; private set; }

    /// <summary>
    /// Los turnos de cada día.
    /// </summary>
    public IEnumerable<AperturaTurno> Turnos => _turnos;

    /// <summary>
    /// Cierra el día de apertura de la caja.
    /// </summary>
    /// <param name="supervisor">El supervisor que realiza el cierre.</param>
    /// <param name="comentario">El comentario de cierre.</param>
    /// <param name="montoEfectivoParaCierre">El monto efectivo para el cierre.</param>
    /// <param name="montoTransbankParaCierre">El mnonto de tarjetas de crédito y débito para el cierre.</param>
    internal void Cerrar(Usuario supervisor, string comentario, decimal montoEfectivoParaCierre, decimal montoTransbankParaCierre)
    {
        Comentario = comentario;
        Supervisor = supervisor;
        FechaCierre = DateTime.Now;
        EfectivoCierre = montoEfectivoParaCierre;
        MontoTransbankCierre = montoTransbankParaCierre;
        Estado = EstadoDia.Cerrado;

    }

    /// <summary>
    /// Reapertura un día.
    /// </summary>
    internal void Reabrir()
    {
        FechaCierre = null;
        Comentario = null;
        Supervisor = null;
        EfectivoCierre = null;
        MontoTransbankCierre = null;
        Estado = EstadoDia.Abierto;
    }

    internal AperturaTurno? FindTurno(Guid turnoId) =>
        _turnos.Find(x => x.Id == turnoId);
}
