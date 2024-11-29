namespace OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
public static class DetalleEscenarioCupoExclusionExtensions
{
    public static DetalleEscenarioCupoExclusionFullInfo ToFullInfo(this DetalleEscenarioCupoExclusion exclusion)
    {
        return new DetalleEscenarioCupoExclusionFullInfo
        {
            EscenarioCupoId = exclusion.EscenarioCupoId,
            ServicioId = exclusion.ServicioId,
            CanalVentaId = exclusion.CanalVentaId,
            DiaSemanaId = exclusion.DiaSemanaId,
            HoraInicio = exclusion.HoraInicio,
            HoraFin = exclusion.HoraFin,
            ServicioNombre = exclusion.Servicio?.Nombre ?? string.Empty,
            CanalVentaNombre = exclusion.CanalVenta?.Nombre ?? string.Empty
        };
    }
}
