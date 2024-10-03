namespace OzyParkAdmin.Domain.Entidades;

/// <summary>
/// La entidad de día de semana.
/// </summary>
/// <param name="Id">El id del día de semana.</param>
/// <param name="Aka">El aka del día de semana.</param>
public sealed record DiaSemana(int Id, string Aka)
{
    /// <summary>
    /// El día de semana.
    /// </summary>
    public DayOfWeek DayOfWeek => Id == 7 ? DayOfWeek.Sunday : (DayOfWeek)Id;
}
