namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;

public class DetalleEscenarioCupoModel
{
    public int ServicioId { get; set; }
    public int? TopeDiario { get; set; }
    public bool UsaSobreCupo { get; set; }
    public TimeSpan HoraMaximaVenta { get; set; }
    public TimeSpan HoraMaximaRevalidacion { get; set; }
    public bool UsaTopeEnCupo { get; set; }
    public bool TopeFlotante { get; set; }
}