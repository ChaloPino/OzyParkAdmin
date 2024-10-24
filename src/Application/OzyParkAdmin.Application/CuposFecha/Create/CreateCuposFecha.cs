using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.CuposFecha.Create;

/// <summary>
/// Crea varios cupos por fecha.
/// </summary>
/// <param name="FechaDesde">La fecha de inicio para generar los cupos por fecha.</param>
/// <param name="FechaHasta">La fecha de fin para generar los cupos por fecha.</param>
/// <param name="EscenarioCupo">El escenario de todos los cupos por fecha.</param>
/// <param name="CanalesVenta">Los canales de venta por cupo por fecha.</param>
/// <param name="DiasSemana">Los días de semana por cupo por fecha.</param>
/// <param name="HoraInicio">La hora de inicio de todos los cupos  por fecha.</param>
/// <param name="HoraTermino">La hora de término de todos los cupos  por fecha.</param>
/// <param name="IntervaloMinutos">El intérvalo en minutos para crear los cupos por fecha.</param>
/// <param name="Total">El total de cupo de todos los cupos por fecha.</param>
/// <param name="SobreCupo">El sobrecupo de todos los cupos por fecha.</param>
/// <param name="TopeEnCupo">El tope en todos los cupos.</param>
public sealed record CreateCuposFecha(
    DateOnly FechaDesde,
    DateOnly FechaHasta,
    EscenarioCupoInfo EscenarioCupo,
    ImmutableArray<CanalVenta> CanalesVenta,
    ImmutableArray<DiaSemana> DiasSemana,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    int IntervaloMinutos,
    int Total,
    int SobreCupo,
    int TopeEnCupo) : ICommand;
