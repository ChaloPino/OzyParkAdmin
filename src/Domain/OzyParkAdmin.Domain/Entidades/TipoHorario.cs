namespace OzyParkAdmin.Domain.Entidades;

/// <summary>
/// El tipo de horario.
/// </summary>
/// <param name="Id">El id del tipo de horario.</param>
/// <param name="Aka">El aka del tipo de horario.</param>
/// <param name="Descripcion">La descripción del tipo de horario.</param>
/// <param name="HoraDesde">La hora desde.</param>
/// <param name="HoraHasta">La hora hasta.</param>
/// <param name="EsActivo">Si está activo.</param>
public sealed record TipoHorario(int Id, string Aka, string Descripcion, TimeSpan HoraDesde, TimeSpan HoraHasta, bool EsActivo);
