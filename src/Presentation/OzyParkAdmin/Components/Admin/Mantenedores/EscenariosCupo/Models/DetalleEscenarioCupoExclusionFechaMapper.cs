using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;

public static class DetalleEscenarioCupoExclusionFechaMapper
{
    /// <summary>
    /// Convierte una instancia de <see cref="DetalleEscenarioCupoExclusionFecha"/> a <see cref="DetalleEscenarioCupoExclusionFechaModel"/>.
    /// </summary>
    public static DetalleEscenarioCupoExclusionFechaModel FromEntity(this DetalleEscenarioCupoExclusionFecha entity)
    {
        return new DetalleEscenarioCupoExclusionFechaModel
        {
            EscenarioCupoId = entity.EscenarioCupoId,
            ServicioId = entity.ServicioId,
            CanalVentaId = entity.CanalVentaId,
            FechaExclusion = entity.FechaExclusion,
            HoraInicio = entity.HoraInicio,
            HoraFin = entity.HoraFin,
            ServicioNombre = entity.Servicio.Nombre,
            CanalVentaNombre = entity.CanalVenta.Nombre
        };
    }

}
