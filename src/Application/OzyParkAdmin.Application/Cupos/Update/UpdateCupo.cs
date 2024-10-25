using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;

namespace OzyParkAdmin.Application.Cupos.Update;

/// <summary>
/// Actualiza un cupo.
/// </summary>
/// <param name="Id">El id del cupo a actualizar.</param>
/// <param name="FechaEfectiva">La fecha efectiva del cupo.</param>
/// <param name="EscenarioCupo">El escenario del cupo.</param>
/// <param name="CanalVenta">El canal de venta del cupo.</param>
/// <param name="DiaSemana">El día de semana del cupo.</param>
/// <param name="HoraInicio">La hora de inicio del cupo.</param>
/// <param name="HoraFin">La hora de fin del cupo.</param>
/// <param name="Total">El total del cupo.</param>
/// <param name="SobreCupo">El sobrecupo.</param>
/// <param name="TopeEnCupo">El tope en el cupo.</param>
public sealed record UpdateCupo(
    int Id,
    DateOnly FechaEfectiva,
    EscenarioCupoInfo EscenarioCupo,
    CanalVenta CanalVenta,
    DiaSemana DiaSemana,
    TimeSpan HoraInicio,
    TimeSpan HoraFin,
    int Total,
    int SobreCupo,
    int TopeEnCupo) : ICommand<CupoFullInfo>;
