using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;

namespace OzyParkAdmin.Application.EscenariosCupo;

public static class DetalleEscenarioCupoExclusionFechaExtensions
{
    public static DetalleEscenarioCupoExclusionFechaFullInfo ToFullInfo(this DetalleEscenarioCupoExclusionFecha exclusion)
    {
        return new DetalleEscenarioCupoExclusionFechaFullInfo
        {
            EscenarioCupoId = exclusion.EscenarioCupoId,
            ServicioId = exclusion.ServicioId,
            CanalVentaId = exclusion.CanalVentaId,
            FechaExclusion = exclusion.FechaExclusion,
            HoraInicio = exclusion.HoraInicio,
            HoraFin = exclusion.HoraFin,
            ServicioNombre = exclusion.Servicio?.Nombre ?? string.Empty,
            CanalVentaNombre = exclusion.CanalVenta?.Nombre ?? string.Empty
        };
    }
}