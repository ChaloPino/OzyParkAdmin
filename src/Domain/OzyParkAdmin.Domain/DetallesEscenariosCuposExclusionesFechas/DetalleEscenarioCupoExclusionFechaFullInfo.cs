namespace OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;

/// <summary>
/// DTO que representa la información detallada de una exclusión de cupo por fecha.
/// </summary>
public class DetalleEscenarioCupoExclusionFechaFullInfo
{
    public int EscenarioCupoId { get; set; }
    public int ServicioId { get; set; }
    public string ServicioNombre { get; set; } = string.Empty;
    public int CanalVentaId { get; set; }
    public string CanalVentaNombre { get; set; } = string.Empty;
    public DateTime? FechaExclusion { get; set; } = default!;
    public TimeSpan? HoraInicio { get; set; } = default!;
    public TimeSpan? HoraFin { get; set; } = default!;

}
