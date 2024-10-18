using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Cupos;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using System.ComponentModel;

namespace OzyParkAdmin.Domain.CuposFecha;

/// <summary>
/// Contiene la lógica de negocio de <see cref="CupoFecha"/>.
/// </summary>
public sealed class CupoFechaManager : IBusinessLogic
{
    private readonly ICupoFechaRepository _repository;
    private readonly IEscenarioCupoRepository _escenarioCupoRepository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CupoFechaManager"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICupoFechaRepository"/>.</param>
    /// <param name="escenarioCupoRepository">El <see cref="IEscenarioCupoRepository"/>.</param>
    public CupoFechaManager(ICupoFechaRepository repository, IEscenarioCupoRepository escenarioCupoRepository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(escenarioCupoRepository);
        _repository = repository;
        _escenarioCupoRepository = escenarioCupoRepository;
    }

    /// <summary>
    /// Crea varios cupos por fecha.
    /// </summary>
    /// <param name="fechaDesde">La fecha de inicio para generar todos los cupos por fecha.</param>
    /// <param name="fechaHasta">La fecha de fin para generar todos los cupos por fecha.</param>
    /// <param name="escenarioCupoInfo">El escenario de todos los cupos por fecha.</param>
    /// <param name="canalesVenta">Los canales de venta por cupo por fecha.</param>
    /// <param name="diasSemana">Los días de semana por cupo por fecha.</param>
    /// <param name="horaInicio">La hora de inicio para calcular cada cupo por fecha.</param>
    /// <param name="horaTermino">La hora de término para calcular cada cupo por fecha.</param>
    /// <param name="intervaloMinutos">El intérvalo en minutos para calcular cada cupo por fecha.</param>
    /// <param name="total">El total de cupo de todos los cupos por fecha.</param>
    /// <param name="sobreCupo">El sobrecupo de todos los cupos por fecha.</param>
    /// <param name="topeEnCupo">El tope en todos los cupos.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de crear varios cupos por fecha.</returns>
    public async Task<ResultOf<IEnumerable<CupoFecha>>> CreateCuposFechaAsync(
        DateOnly fechaDesde,
        DateOnly fechaHasta,
        EscenarioCupoInfo escenarioCupoInfo,
        IEnumerable<CanalVenta> canalesVenta,
        IEnumerable<DiaSemana> diasSemana,
        TimeSpan horaInicio,
        TimeSpan horaTermino,
        int intervaloMinutos,
        int total,
        int sobreCupo,
        int topeEnCupo,
        CancellationToken cancellationToken)
    {
        EscenarioCupo? escenarioCupo = await _escenarioCupoRepository.FindByIdAsync(escenarioCupoInfo.Id, cancellationToken);

        if (escenarioCupo is null)
        {
            return new ValidationError(nameof(CupoFecha.EscenarioCupo), $"No existe el escenario de cupo {escenarioCupoInfo.Nombre}");
        }

        int seedId = await _repository.MaxIdAsync(cancellationToken);

        return await CreateManyAsync(
             seedId + 1,
             fechaDesde,
             fechaHasta,
             escenarioCupo,
             canalesVenta,
             diasSemana,
             horaInicio,
             horaTermino,
             intervaloMinutos,
             total,
             sobreCupo,
             topeEnCupo,
             cancellationToken);
    }

    /// <summary>
    /// Actualiza un cupo por fecha.
    /// </summary>
    /// <param name="id">El id del cupo por fecha.</param>
    /// <param name="fecha">La fecha del cupo por fecha.</param>
    /// <param name="escenarioCupoInfo">El escenario del cupo por fecha.</param>
    /// <param name="canalVenta">El canal de venta del cupo por fecha.</param>
    /// <param name="diaSemana">El día de semana del cupo por fecha.</param>
    /// <param name="horaInicio">La hora de inicio del cupo por fecha.</param>
    /// <param name="horaFin">La hora de fin del cupo por fecha.</param>
    /// <param name="total">El total de cupo del cupo por fecha.</param>
    /// <param name="sobreCupo">El sobrecupo del cupo por fecha.</param>
    /// <param name="topeEnCupo">El tope en el cupo por fecha.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de actualizar el cupo por fecha.</returns>
    public async Task<ResultOf<CupoFecha>> UpdateCupoFechaAsync(
        int id,
        DateOnly fecha,
        EscenarioCupoInfo escenarioCupoInfo,
        CanalVenta canalVenta,
        DiaSemana diaSemana,
        TimeSpan horaInicio,
        TimeSpan horaFin,
        int total,
        int sobreCupo,
        int topeEnCupo,
        CancellationToken cancellationToken)
    {
        CupoFecha? cupoFecha = await _repository.FindByIdAsync(id, cancellationToken);

        if (cupoFecha is null)
        {
            return new NotFound();
        }

        EscenarioCupo? escenarioCupo = await _escenarioCupoRepository.FindByIdAsync(escenarioCupoInfo.Id, cancellationToken);

        if (escenarioCupo is null)
        {
            return new ValidationError(nameof(Cupo.EscenarioCupo), $"No existe el escenario de cupo {escenarioCupoInfo.Nombre}");
        }

        return await cupoFecha.UpdateAsync(
            fecha,
            escenarioCupo,
            canalVenta,
            diaSemana,
            horaInicio,
            horaFin,
            total,
            sobreCupo,
            topeEnCupo,
            ValidateDuplicityAsync,
            cancellationToken);
    }

    private async Task<ResultOf<IEnumerable<CupoFecha>>> CreateManyAsync(
        int seedId,
        DateOnly fechaDesde,
        DateOnly fechaHasta,
        EscenarioCupo escenarioCupo,
        IEnumerable<CanalVenta> canalesVenta,
        IEnumerable<DiaSemana> diasSemana,
        TimeSpan horaInicio,
        TimeSpan horaTermino,
        int intervaloMinutos,
        int total,
        int sobreCupo,
        int topeEnCupo,
        CancellationToken cancellationToken)
    {
        IEnumerable<DateOnly> fechas = CreateFechas(fechaDesde, fechaHasta);
        IEnumerable<(TimeSpan HoraInicio, TimeSpan HoraFin)> horas = CreateHoras(horaInicio, horaTermino, intervaloMinutos);

        IEnumerable<(DateOnly Fecha, EscenarioCupo EscenarioCupo, CanalVenta CanalVenta, DiaSemana DiaSemana, TimeSpan HoraInicio, TimeSpan HoraFin)> cuposToCreate =
            [.. from fecha in fechas
                from canalVenta in canalesVenta
                from diaSemana in diasSemana
                from horario in horas
                select (fecha, escenarioCupo, canalVenta, diaSemana, horario.HoraInicio, horario.HoraFin)];

        IEnumerable<CupoFecha> existentes = await _repository.FindByUniqueKeysAsync(
            [..cuposToCreate.Select(x => (x.Fecha, x.EscenarioCupo, x.CanalVenta, x.DiaSemana, x.HoraInicio))], cancellationToken).ConfigureAwait(false);

        IEnumerable<(DateOnly Fecha, EscenarioCupo EscenarioCupo, CanalVenta CanalVenta, DiaSemana DiaSemana, TimeSpan HoraInicio, TimeSpan HoraFin)> exists =
            [.. existentes.Select(x => (x.Fecha, x.EscenarioCupo, x.CanalVenta, x.DiaSemana, x.HoraInicio, x.HoraFin))];

        cuposToCreate = cuposToCreate.Except(exists);

        List<CupoFecha> nuevosCuposFecha = [];

        foreach (var cupoToCreate in cuposToCreate)
        {
            int id = seedId++;

            var result = CreateCupoFecha(
                id,
                cupoToCreate.Fecha,
                escenarioCupo,
                cupoToCreate.CanalVenta,
                cupoToCreate.DiaSemana,
                cupoToCreate.HoraInicio,
                cupoToCreate.HoraFin,
                total,
                sobreCupo,
                topeEnCupo,
                nuevosCuposFecha);

            if (result.IsFailure(out Failure failure))
            {
                return failure;
            }
        }

        return nuevosCuposFecha;
    }

    private static IEnumerable<DateOnly> CreateFechas(DateOnly fechaDesde, DateOnly fechaHasta)
    {
        DateOnly date = fechaDesde;

        do
        {
            yield return date;
            date = date.AddDays(1);
        } while (date <= fechaHasta);
    }

    private static IEnumerable<(TimeSpan HoraInicio, TimeSpan HoraFin)> CreateHoras(TimeSpan horaInicio, TimeSpan horaTermino, int intervaloMinutos)
    {
        TimeSpan hora = horaInicio;
        while (hora <= horaTermino)
        {
            TimeSpan horaFin = hora.Add(TimeSpan.FromSeconds((intervaloMinutos * 60) - 1));
            yield return (hora, horaFin);
            hora = hora.Add(TimeSpan.FromMinutes(intervaloMinutos));
        }
    }

    private static ResultOf<CupoFecha> CreateCupoFecha(
        int id,
        DateOnly fecha,
        EscenarioCupo escenarioCupo,
        CanalVenta canalVenta,
        DiaSemana diaSemana,
        TimeSpan horaInicio,
        TimeSpan horaFin,
        int total,
        int sobreCupo,
        int topeEnCupo,
        IList<CupoFecha> nuevosCuposFecha)
    {
        var result = CupoFecha.Create(
            id,
            fecha,
            escenarioCupo,
            canalVenta,
            diaSemana,
            horaInicio,
            horaFin,
            total,
            sobreCupo,
            topeEnCupo);

        result.Switch(
            onSuccess: nuevosCuposFecha.Add,
            onFailure: _ => { });

        return result;
    }

    private async Task ValidateDuplicityAsync(
        (int Id,
        DateOnly Fecha,
        EscenarioCupo EscenarioCupo,
        CanalVenta CanalVenta,
        DiaSemana DiaSemana,
        TimeSpan HoraInicio) cupoToValidate,
        IList<ValidationError> errors,
        CancellationToken cancellationToken)
    {
        CupoFecha? cupo = await _repository.FindByUniqueKeyAsync(
            cupoToValidate.Fecha,
            cupoToValidate.EscenarioCupo,
            cupoToValidate.CanalVenta,
            cupoToValidate.DiaSemana,
            cupoToValidate.HoraInicio,
            cancellationToken);

        if (cupo is not null && cupo.Id != cupoToValidate.Id)
        {
            errors.Add(new ValidationError(
                nameof(Cupo),
                $"Ya existe un cupo para la fecha {cupoToValidate.Fecha}, el escenario {cupoToValidate.EscenarioCupo.Nombre}, el canal de venta {cupoToValidate.CanalVenta.Texto}, el día de semana {cupoToValidate.DiaSemana.Aka} y la hora de inicio {cupoToValidate.HoraInicio}."));
        }
    }
}
