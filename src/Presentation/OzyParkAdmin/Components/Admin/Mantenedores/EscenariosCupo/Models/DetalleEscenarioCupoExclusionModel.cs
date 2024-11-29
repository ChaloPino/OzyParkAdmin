namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;

public class DetalleEscenarioCupoExclusionModel
{
    public int EscenarioCupoId { get; set; }
    public int ServicioId { get; set; }
    public string ServicioNombre { get; set; }
    public int DiaSemanaId { get; set; }
    public string DiaSemanaNombre { get; set; }
    public int CanalVentaId { get; set; }
    public string CanalVentaNombre { get; set; }
    public TimeSpan? HoraInicio { get; set; }
    public TimeSpan? HoraFin { get; set; }
}
