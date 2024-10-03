namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// La informaci�n completa de la apertura de una caja.
/// </summary>
public sealed record AperturaCajaInfo
{
    /// <summary>
    /// El id de la apertura
    /// </summary>
    public Guid? Id { get; set; }
    /// <summary>
    /// El id de la caja.
    /// </summary>
    public int CajaId { get; set; }

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
    public string CentroCosto { get; set; } = default!;

    /// <summary>
    /// La franquicia asociada a la caja.
    /// </summary>
    public string Franquicia { get; set; } = default!;

    /// <summary>
    /// El punto de venta asociado a la caja.
    /// </summary>
    public string PuntoVenta { get; set; } = default!;

    /// <summary>
    /// El d�a de apertura de la caja.
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
    /// El monto de las tarjetas de cr�dito y d�bito calculado en cierre.
    /// </summary>
    public decimal? MontoTransbankCierre { get; set; }

    /// <summary>
    /// Usuario que realiz� la apertura del d�a.
    /// </summary>
    public string? Usuario { get; set; }

    /// <summary>
    /// El supervisor que realiz� el cierre del d�a.
    /// </summary>
    public string? Supervisor { get; set; }

    /// <summary>
    /// Los comentarios de cierre.
    /// </summary>
    public string? Comentario { get; set; }

    /// <summary>
    /// El �ltimo turno de la apertura de caja.
    /// </summary>
    public Guid? UltimoTurnoId { get; set; }

    /// <summary>
    /// El estado del �ltimo turno.
    /// </summary>
    public EstadoTurno? UltimoTurnoEstado { get; set; }

    /// <summary>
    /// La fecha de apertura del �ltimo turno.
    /// </summary>
    public DateTime? UltimoTurnoFechaApertura { get; set; }
}