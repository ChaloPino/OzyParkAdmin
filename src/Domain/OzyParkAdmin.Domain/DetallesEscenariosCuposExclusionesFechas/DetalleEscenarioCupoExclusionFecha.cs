using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;

public class DetalleEscenarioCupoExclusionFecha
{
    /// <summary>
    /// El identificador del escenario de cupo al que pertenece la exclusión.
    /// </summary>
    public int EscenarioCupoId { get; set; }

    /// <summary>
    /// El identificador del servicio asociado a la exclusión.
    /// </summary>
    public int ServicioId { get; set; }

    /// <summary>
    /// El identificador del canal de venta asociado a la exclusión.
    /// </summary>
    public int CanalVentaId { get; set; }

    /// <summary>
    /// La fecha específica de la exclusión.
    /// </summary>
    public DateTime FechaExclusion { get; set; }

    /// <summary>
    /// La hora de inicio de la exclusión (opcional).
    /// </summary>
    public TimeSpan HoraInicio { get; set; }

    /// <summary>
    /// La hora de finalización de la exclusión (opcional).
    /// </summary>
    public TimeSpan HoraFin { get; set; }

    /// <summary>
    /// El escenario de cupo al que pertenece la exclusión.
    /// </summary>
    public EscenarioCupo EscenarioCupo { get; set; } = default!;

    /// <summary>
    /// El servicio asociado a la exclusión.
    /// </summary>
    public Servicio Servicio { get; set; } = default!;

    /// <summary>
    /// El canal de venta asociado a la exclusión.
    /// </summary>
    public CanalVenta CanalVenta { get; set; } = default!;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DetalleEscenarioCupoExclusionFecha"/> con los parámetros especificados.
    /// </summary>
    /// <param name="escenarioCupoId">El identificador del escenario de cupo al que pertenece la exclusión.</param>
    /// <param name="servicioId">El identificador del servicio asociado a la exclusión.</param>
    /// <param name="canalVentaId">El identificador del canal de venta asociado a la exclusión.</param>
    /// <param name="fechaExclusion">La fecha específica de la exclusión.</param>
    /// <param name="horaInicio">La hora de inicio de la exclusión (opcional).</param>
    /// <param name="horaFin">La hora de finalización de la exclusión (opcional).</param>
    /// <returns>Una nueva instancia de <see cref="DetalleEscenarioCupoExclusionFecha"/>.</returns>
    public static DetalleEscenarioCupoExclusionFecha Create(
        int escenarioCupoId,
        int servicioId,
        int canalVentaId,
        DateTime? fechaExclusion,
        TimeSpan? horaInicio,
        TimeSpan? horaFin)
    {
        return new DetalleEscenarioCupoExclusionFecha
        {
            EscenarioCupoId = escenarioCupoId,
            ServicioId = servicioId,
            CanalVentaId = canalVentaId,
            FechaExclusion = fechaExclusion!.Value,
            HoraInicio = horaInicio!.Value,
            HoraFin = horaFin!.Value
        };
    }

    /// <summary>
    /// Actualiza las propiedades de la exclusión con los valores proporcionados.
    /// </summary>
    /// <param name="servicioId">El identificador del nuevo servicio asociado a la exclusión.</param>
    /// <param name="canalVentaId">El identificador del nuevo canal de venta asociado a la exclusión.</param>
    /// <param name="fechaExclusion">La nueva fecha específica de la exclusión.</param>
    /// <param name="horaInicio">La nueva hora de inicio de la exclusión (opcional).</param>
    /// <param name="horaFin">La nueva hora de finalización de la exclusión (opcional).</param>
    public void Update(
        int servicioId,
        int canalVentaId,
        DateTime fechaExclusion,
        TimeSpan? horaInicio,
        TimeSpan? horaFin)
    {
        ServicioId = servicioId;
        CanalVentaId = canalVentaId;
        FechaExclusion = fechaExclusion;
        HoraInicio = horaInicio!.Value;
        HoraFin = horaFin!.Value;
    }

    /// <summary>
    /// Actualiza las propiedades de la exclusión utilizando otra instancia de <see cref="DetalleEscenarioCupoExclusionFecha"/>.
    /// </summary>
    /// <param name="nuevaExclusion">La instancia de <see cref="DetalleEscenarioCupoExclusionFecha"/> que contiene los nuevos valores.</param>
    public void Update(DetalleEscenarioCupoExclusionFecha nuevaExclusion)
    {
        ServicioId = nuevaExclusion.ServicioId;
        CanalVentaId = nuevaExclusion.CanalVentaId;
        FechaExclusion = nuevaExclusion.FechaExclusion;
        HoraInicio = nuevaExclusion.HoraInicio;
        HoraFin = nuevaExclusion.HoraFin;
    }
}
