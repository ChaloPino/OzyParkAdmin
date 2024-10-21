using MassTransit.Mediator;
using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;

namespace OzyParkAdmin.Application.CuposFecha.Delete;

/// <summary>
/// Elimina varios cupos por fecha.
/// </summary>
/// <param name="FechaDesde">La fecha de inicio para eliminar los cupos por fecha.</param>
/// <param name="FechaHasta">La fecha de fin para eliminar los cupos por fecha.</param>
/// <param name="EscenarioCupo">El escenario de todos los cupos por fecha.</param>
/// <param name="CanalesVenta">Los canales de venta por cupo por fecha.</param>
/// <param name="DiasSemana">Los días de semana por cupo por fecha.</param>
/// <param name="HoraInicio">La hora de inicio de todos los cupos  por fecha.</param>
/// <param name="HoraTermino">La hora de término de todos los cupos  por fecha.</param>
/// <param name="IntervaloMinutos">El intérvalo en minutos para eliminar los cupos por fecha.</param>
public sealed record DeleteCuposFecha(
    DateOnly FechaDesde,
    DateOnly FechaHasta,
    EscenarioCupoInfo EscenarioCupo,
    ImmutableArray<CanalVenta> CanalesVenta,
    ImmutableArray<DiaSemana> DiasSemana,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    int IntervaloMinutos) : Request<SuccessOrFailure>;
