namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;

/// <summary>
/// Modelo para Detalle de Exclusión por Fecha de EscenarioCupo.
/// </summary>
public sealed class DetalleEscenarioCupoExclusionFechaModel
{
    /// <summary>
    /// Id del Escenario de Cupo asociado.
    /// </summary>
    public int EscenarioCupoId { get; set; }

    /// <summary>
    /// Id del Servicio asociado.
    /// </summary>
    public int ServicioId { get; set; }

    /// <summary>
    /// Nombre del Servicio asociado.
    /// </summary>
    public string ServicioNombre { get; set; } = string.Empty;

    /// <summary>
    /// Id del Canal de Venta asociado.
    /// </summary>
    public int CanalVentaId { get; set; }

    /// <summary>
    /// Nombre del Canal de Venta asociado.
    /// </summary>
    public string CanalVentaNombre { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de la exclusión.
    /// </summary>
    public DateTime? FechaExclusion { get; set; }

    /// <summary>
    /// Hora de inicio de la exclusión.
    /// </summary>
    public TimeSpan? HoraInicio { get; set; }

    /// <summary>
    /// Hora de fin de la exclusión.
    /// </summary>
    public TimeSpan? HoraFin { get; set; }


}
