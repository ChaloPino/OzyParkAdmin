using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// El movimiento realizado en un turno de caja.
/// </summary>
public sealed class MovimientoTurno
{
    /// <summary>
    /// El correlativo del movimiento.
    /// </summary>
    public int Correlativo { get; private init; }

    /// <summary>
    /// El tipo del movimiento.
    /// </summary>
    public TipoMovimientoTurno TipoTurnoMovimiento { get; private init; }

    /// <summary>
    /// La fecha del movimiento.
    /// </summary>
    public DateTime Fecha { get; private init; }

    /// <summary>
    /// El usuario que creó el movimiento.
    /// </summary>
    public Usuario Usuario { get; private init; } = default!;

    /// <summary>
    /// El supervisor que cambió el movimiento.
    /// </summary>
    public Usuario? Supervisor { get; private set; }

    /// <summary>
    /// El número de referencia del movimiento.
    /// </summary>
    public string? NumeroReferencia { get; private set; }

    /// <summary>
    /// El estado del movimiento.
    /// </summary>
    public EstadoMovimientoTurno Estado { get; private set; }
}