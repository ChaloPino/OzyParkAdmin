using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;

public class EscenarioCupoDataResponse
{
    public EscenarioCupoModel EscenarioCupo { get; set; }
    public List<DetalleEscenarioCupoInfo> Detalles { get; set; }
    public List<DetalleEscenarioCupoExclusionFechaFullInfo> ExclusionesPorFecha { get; set; }
}

