namespace OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
public class DetalleEscenarioCupoExclusionFullInfo
{
    public int EscenarioCupoId { get; set; }
    public int ServicioId { get; set; }
    public string ServicioNombre { get; set; } = string.Empty;
    public int CanalVentaId { get; set; }
    public string CanalVentaNombre { get; set; } = string.Empty;
    public int DiaSemanaId { get; set; }
    public string DiaSemanaNombre { get; set; }
    public TimeSpan? HoraInicio { get; set; } = default!;
    public TimeSpan? HoraFin { get; set; } = default!;
}
