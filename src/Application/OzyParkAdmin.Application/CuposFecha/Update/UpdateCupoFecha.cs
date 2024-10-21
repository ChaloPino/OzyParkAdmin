using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Application.CuposFecha.Update;

/// <summary>
/// Actualiza un cupo por fecha.
/// </summary>
/// <param name="Id">El id del cupo por fecha a actualizar.</param>
/// <param name="Fecha">La fecha del cupo a actualizar.</param>
/// <param name="EscenarioCupo">El escenario del cupo por fecha.</param>
/// <param name="CanalVenta">El canal de venta del cupo por fecha.</param>
/// <param name="DiaSemana">El día de semana del cupo por fecha.</param>
/// <param name="HoraInicio">La hora de inicio del cupo por fecha.</param>
/// <param name="HoraFin">La hora de fin del cupo por fecha.</param>
/// <param name="Total">El total del cupo por fecha.</param>
/// <param name="SobreCupo">El sobrecupo.</param>
/// <param name="TopeEnCupo">El tope en el cupo por fecha.</param>
public sealed record UpdateCupoFecha(
    int Id,
    DateOnly Fecha,
    EscenarioCupoInfo EscenarioCupo,
    CanalVenta CanalVenta,
    DiaSemana DiaSemana,
    TimeSpan HoraInicio,
    TimeSpan HoraFin,
    int Total,
    int SobreCupo,
    int TopeEnCupo) : ICupoFechaStateChangeable;
