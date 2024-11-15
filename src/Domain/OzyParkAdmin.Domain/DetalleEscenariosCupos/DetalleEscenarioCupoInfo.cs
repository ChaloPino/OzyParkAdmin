using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Domain.DetallesEscenariosCupos;
public class DetalleEscenarioCupoInfo
{
    public int EscenarioCupoId { get; set; }
    public ServicioInfo Servicio { get; set; }
    public int ServicioId { get; set; }
    public int? TopeDiario { get; set; } = 0;
    public bool UsaSobreCupo { get; set; }
    public TimeSpan? HoraMaximaVenta { get; set; }
    public TimeSpan? HoraMaximaRevalidacion { get; set; }
    public bool UsaTopeEnCupo { get; set; }
    public bool TopeFlotante { get; set; }
}
