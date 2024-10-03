using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// La información del movimiento de un turno de caja.
/// </summary>
public sealed record MovimientoTurnoInfo
{
    /// <summary>
    /// El correlativo del movimiento.
    /// </summary>
    public int Correlativo { get; set; }

    /// <summary>
    /// El tipo del movimiento.
    /// </summary>
    public TipoMovimientoTurno TipoTurnoMovimiento { get; set; }

    /// <summary>
    /// La fecha del movimiento.
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// El usuario que creó el movimiento.
    /// </summary>
    public UsuarioInfo Usuario { get; set; } = default!;

    /// <summary>
    /// El supervisor que cambió el movimiento.
    /// </summary>
    public UsuarioInfo? Supervisor { get; set; }

    /// <summary>
    /// El número de referencia del movimiento.
    /// </summary>
    public string? NumeroReferencia { get; set; }

    /// <summary>
    /// El estado del movimiento.
    /// </summary>
    public EstadoMovimientoTurno Estado { get; set; }
}