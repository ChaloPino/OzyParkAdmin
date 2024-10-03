namespace OzyParkAdmin.Domain.Cupos;

/// <summary>
/// La información de cupo por hora.
/// </summary>
/// <param name="HoraInicio">La hora de inicio del cupo.</param>
/// <param name="HoraFin">La hora de fin del cupo.</param>
/// <param name="Total">El total para el horario.</param>
/// <param name="Disponible">La cantidad disponible para el horario.</param>
public sealed record CupoHoraInfo(TimeSpan HoraInicio, TimeSpan HoraFin, int Total, int Disponible)
{
    /// <summary>
    /// Si el horario tiene cupo disponible.
    /// </summary>
    public bool HayCupo => Disponible > 0;
}
