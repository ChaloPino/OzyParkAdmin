using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Cupos;

/// <summary>
/// La información de cabecera de un cupo por fecha.
/// </summary>
/// <param name="Fecha">La fecha.</param>
/// <param name="Horario">El horario.</param>
public sealed record CupoFechaInfo(DateTime Fecha, ImmutableArray<CupoHoraInfo> Horario)
{
    /// <summary>
    /// Si la fecha tiene cupos por horario.
    /// </summary>
    public bool HayHorarios => Horario.Length > 0;
}
