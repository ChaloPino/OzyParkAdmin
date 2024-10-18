using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Cupos;

/// <summary>
/// La entidad del cupo.
/// </summary>
public sealed class Cupo
{
    /// <summary>
    /// El id del cupo.
    /// </summary>
    public int Id { get; private init; }

    /// <summary>
    /// El escenario de cupo asociado al cupo.
    /// </summary>
    public EscenarioCupo EscenarioCupo { get; private set; } = default!;

    /// <summary>
    /// El canal de venta asociado al cupo.
    /// </summary>
    public CanalVenta CanalVenta { get; private set; } = default!;

    /// <summary>
    /// El día de semana asociado al cupo.
    /// </summary>
    public DiaSemana DiaSemana { get; private set; } = default!;

    /// <summary>
    /// La hora de inicio del cupo.
    /// </summary>
    public TimeSpan HoraInicio { get; private set; }

    /// <summary>
    /// La hora de fin del cupo.
    /// </summary>
    public TimeSpan HoraFin { get; private set; }

    /// <summary>
    /// El total del cupo.
    /// </summary>
    public int Total { get; private set; }

    /// <summary>
    /// El sobrecupo.
    /// </summary>
    public int SobreCupo { get; private set; }

    /// <summary>
    /// El tope que se tiene en el cupo.
    /// </summary>
    public int TopeEnCupo { get; private set; }

    /// <summary>
    /// La fecha efectiva desde cuando se puede usar el cupo.
    /// </summary>
    public DateOnly FechaEfectiva { get; private set; }

    /// <summary>
    /// Última fecha en que se modificó el cupo.
    /// </summary>
    public DateTime UltimaModificacion { get; private set; } = DateTime.Now;

    internal static async Task<ResultOf<Cupo>> CreateAsync(
        int id,
        DateOnly fechaEfectiva,
        EscenarioCupo escenarioCupo,
        CanalVenta canalVenta,
        DiaSemana diaSemana,
        TimeSpan horaInicio,
        TimeSpan horaFin,
        int total,
        int sobreCupo,
        int topeEnCupo,
        Func<(int Id, DateOnly FechaEfectiva, EscenarioCupo EscenarioCupo, CanalVenta CanalVenta, DiaSemana DiaSemana, TimeSpan HoraInicio), IList<ValidationError>, CancellationToken, Task> validateDuplicate,
        CancellationToken cancellationToken)
    {
        List<ValidationError> errors = [];
        ValidateHora(horaInicio, horaFin, errors);
        await validateDuplicate((id, fechaEfectiva, escenarioCupo, canalVenta, diaSemana, horaInicio), errors, cancellationToken);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Cupo()
        {
            Id = id,
            EscenarioCupo = escenarioCupo,
            CanalVenta = canalVenta,
            DiaSemana = diaSemana,
            HoraInicio = horaInicio,
            HoraFin = horaFin,
            Total = total,
            SobreCupo = sobreCupo,
            TopeEnCupo = topeEnCupo,
            FechaEfectiva = fechaEfectiva
        };
    }

    private static void ValidateHora(TimeSpan horaInicio, TimeSpan horaFin, List<ValidationError> errors)
    {
        if (horaInicio > horaFin)
        {
            errors.Add(new ValidationError(nameof(HoraInicio), "La hora de inicio debe ser menor o igual a la hora de fin."));
        }
    }

    internal async Task<ResultOf<Cupo>> UpdateAsync(
        DateOnly fechaEfectiva,
        EscenarioCupo escenarioCupo,
        CanalVenta canalVenta,
        DiaSemana diaSemana,
        TimeSpan horaInicio,
        TimeSpan horaFin,
        int total,
        int sobreCupo,
        int topeEnCupo,
        Func<(int id, DateOnly FechaEfectiva, EscenarioCupo EscenarioCupo, CanalVenta CanalVenta, DiaSemana DiaSemana, TimeSpan HoraInicio), IList<ValidationError>, CancellationToken, Task> validateDuplicate,
        CancellationToken cancellationToken)
    {
        List<ValidationError> errors = [];
        ValidateHora(horaInicio, horaFin, errors);
        await validateDuplicate((Id, fechaEfectiva, escenarioCupo, canalVenta, diaSemana, horaInicio), errors, cancellationToken);

        EscenarioCupo = escenarioCupo;
        CanalVenta = canalVenta;
        DiaSemana = diaSemana;
        HoraInicio = horaInicio;
        HoraFin = horaFin;
        Total = total;
        SobreCupo = sobreCupo;
        TopeEnCupo = topeEnCupo;
        FechaEfectiva = fechaEfectiva;
        UltimaModificacion = DateTime.Now;

        return this;
    }
}
