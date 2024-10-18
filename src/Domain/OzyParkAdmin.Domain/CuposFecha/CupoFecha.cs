using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.CuposFecha;

/// <summary>
/// La entidad de cupo por fecha.
/// </summary>
public sealed class CupoFecha
{
    /// <summary>
    /// El id del cupo por fecha.
    /// </summary>
    public int Id { get; private init; }

    /// <summary>
    /// La fecha del cupo.
    /// </summary>
    public DateOnly Fecha { get; private init; }

    /// <summary>
    /// El escenario de cupo asociado al cupo por fecha.
    /// </summary>
    public EscenarioCupo EscenarioCupo { get; private set; } = default!;

    /// <summary>
    /// El canal de venta asociado al cupo por fecha.
    /// </summary>
    public CanalVenta CanalVenta { get; private set; } = default!;

    /// <summary>
    /// El día de semana asociado al cupo por fecha.
    /// </summary>
    public DiaSemana DiaSemana { get; private set; } = default!;

    /// <summary>
    /// La hora de inicio del cupo por fecha.
    /// </summary>
    public TimeSpan HoraInicio { get; private set; }

    /// <summary>
    /// La hora de fin del cupo por fecha.
    /// </summary>
    public TimeSpan HoraFin { get; private set; }

    /// <summary>
    /// El total del cupo por fecha.
    /// </summary>
    public int Total { get; private set; }

    /// <summary>
    /// El sobrecupo.
    /// </summary>
    public int SobreCupo { get; private set; }

    /// <summary>
    /// El tope que se tiene en el cupo por fecha.
    /// </summary>
    public int TopeEnCupo { get; private set; }

    internal static ResultOf<CupoFecha> Create(
        int id,
        DateOnly fecha,
        EscenarioCupo escenarioCupo,
        CanalVenta canalVenta,
        DiaSemana diaSemana,
        TimeSpan horaInicio,
        TimeSpan horaFin,
        int total,
        int sobreCupo,
        int topeEnCupo)
    {
        List<ValidationError> errors = [];
        ValidateHora(horaInicio, horaFin, errors);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new CupoFecha()
        {
            Id = id,
            Fecha = fecha,
            EscenarioCupo = escenarioCupo,
            CanalVenta = canalVenta,
            DiaSemana = diaSemana,
            HoraInicio = horaInicio,
            HoraFin = horaFin,
            Total = total,
            SobreCupo = sobreCupo,
            TopeEnCupo = topeEnCupo,
        };
    }

    private static void ValidateHora(TimeSpan horaInicio, TimeSpan horaFin, List<ValidationError> errors)
    {
        if (horaInicio > horaFin)
        {
            errors.Add(new ValidationError(nameof(HoraInicio), "La hora de inicio debe ser menor o igual a la hora de fin."));
        }
    }

    internal async Task<ResultOf<CupoFecha>> UpdateAsync(
        DateOnly fecha,
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
        await validateDuplicate((Id, fecha, escenarioCupo, canalVenta, diaSemana, horaInicio), errors, cancellationToken);

        EscenarioCupo = escenarioCupo;
        CanalVenta = canalVenta;
        DiaSemana = diaSemana;
        HoraInicio = horaInicio;
        HoraFin = horaFin;
        Total = total;
        SobreCupo = sobreCupo;
        TopeEnCupo = topeEnCupo;

        return this;
    }
}
