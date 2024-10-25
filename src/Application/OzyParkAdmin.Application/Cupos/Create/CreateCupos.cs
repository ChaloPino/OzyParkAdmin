using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.Cupos.Create;

/// <summary>
/// Crea varios cupos.
/// </summary>
/// <param name="FechaEfectiva">La fecha efectiva de todos los cupos.</param>
/// <param name="EscenarioCupo">El escenario de todos los cupos.</param>
/// <param name="CanalesVenta">Los canales de venta por cupo.</param>
/// <param name="DiasSemana">Los días de semana por cupo.</param>
/// <param name="HoraInicio">La hora de inicio de todos los cupos.</param>
/// <param name="HoraTermino">La hora de término de todos los cupos.</param>
/// <param name="IntervaloMinutos">El intérvalo en minutos para crear los cupos.</param>
/// <param name="Total">El total de cupo de todos los cupos.</param>
/// <param name="SobreCupo">El sobrecupo de todos los cupos.</param>
/// <param name="TopeEnCupo">El tope en todos los cupos.</param>
public sealed record CreateCupos(
    DateOnly FechaEfectiva,
    EscenarioCupoInfo EscenarioCupo,
    ImmutableArray<CanalVenta> CanalesVenta,
    ImmutableArray<DiaSemana> DiasSemana,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    int IntervaloMinutos,
    int Total,
    int SobreCupo,
    int TopeEnCupo) : ICommand<List<CupoFullInfo>>;
