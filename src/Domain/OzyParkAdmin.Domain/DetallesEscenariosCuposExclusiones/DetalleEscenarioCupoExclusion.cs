using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Servicios;

namespace OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
public class DetalleEscenarioCupoExclusion
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
    /// Día de la semana 
    /// </summary>
    public int DiaSemanaId { get; set; }

    /// <summary>
    /// La hora de inicio de la exclusión (opcional).
    /// </summary>
    public TimeSpan HoraInicio { get; set; }

    /// <summary>
    /// La hora de finalización de la exclusión (opcional).
    /// </summary>
    public TimeSpan? HoraFin { get; set; }

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
    ///El día de semana de la exclusión.
    /// </summary>
    public DiaSemana DiaSemana { get; set; } = default!;


    /// <summary>
    /// Crea una nueva instancia de <see cref="DetalleEscenarioCupoExclusion"/> con los parámetros especificados.
    /// </summary>
    /// <param name="escenarioCupoId">El identificador del escenario de cupo al que pertenece la exclusión.</param>
    /// <param name="servicioId">El identificador del servicio asociado a la exclusión.</param>
    /// <param name="canalVentaId">El identificador del canal de venta asociado a la exclusión.</param>
    /// <param name="diaSemanaId">La fecha específica de la exclusión.</param>
    /// <param name="horaInicio">La hora de inicio de la exclusión (opcional).</param>
    /// <param name="horaFin">La hora de finalización de la exclusión (opcional).</param>
    /// <returns>Una nueva instancia de <see cref="DetalleEscenarioCupoExclusion"/>.</returns>
    public static DetalleEscenarioCupoExclusion Create(
        int escenarioCupoId,
        int servicioId,
        int canalVentaId,
        int diaSemanaId,
        TimeSpan? horaInicio,
        TimeSpan? horaFin)
    {
        return new DetalleEscenarioCupoExclusion
        {
            EscenarioCupoId = escenarioCupoId,
            ServicioId = servicioId,
            CanalVentaId = canalVentaId,
            DiaSemanaId = diaSemanaId,
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
        int diaSemanaId,
        TimeSpan? horaInicio,
        TimeSpan? horaFin)
    {
        ServicioId = servicioId;
        CanalVentaId = canalVentaId;
        DiaSemanaId = diaSemanaId;
        HoraInicio = horaInicio!.Value;
        HoraFin = horaFin!.Value;
    }

    /// <summary>
    /// Actualiza las propiedades de la exclusión utilizando otra instancia de <see cref="DetalleEscenarioCupoExclusion"/>.
    /// </summary>
    /// <param name="nuevaExclusion">La instancia de <see cref="DetalleEscenarioCupoExclusion"/> que contiene los nuevos valores.</param>
    public void Update(DetalleEscenarioCupoExclusion nuevaExclusion)
    {
        ServicioId = nuevaExclusion.ServicioId;
        CanalVentaId = nuevaExclusion.CanalVentaId;
        DiaSemanaId = nuevaExclusion.DiaSemanaId;
        HoraInicio = nuevaExclusion.HoraInicio;
        HoraFin = nuevaExclusion.HoraFin;
    }
}
