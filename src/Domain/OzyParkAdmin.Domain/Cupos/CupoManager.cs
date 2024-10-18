using OzyParkAdmin.Domain.CanalesVenta;
using OzyParkAdmin.Domain.Entidades;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Cupos;

/// <summary>
/// Contiene las lógicas de negocio de <see cref="Cupo"/>.
/// </summary>
public sealed class CupoManager : IBusinessLogic
{
    private readonly ICupoRepository _repository;
    private readonly IEscenarioCupoRepository _escenarioCupoRepository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CupoManager"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICupoRepository"/>.</param>
    /// <param name="escenarioCupoRepository">El <see cref="IEscenarioCupoRepository"/>.</param>
    public CupoManager(ICupoRepository repository, IEscenarioCupoRepository escenarioCupoRepository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(escenarioCupoRepository);
        _repository = repository;
        _escenarioCupoRepository = escenarioCupoRepository;
    }

    /// <summary>
    /// Crea varios cupos.
    /// </summary>
    /// <param name="fechaEfectiva">La fecha efectiva de todos los cupos.</param>
    /// <param name="escenarioCupoInfo">El escenario de todos los cupos.</param>
    /// <param name="canalesVenta">Los canales de venta por cupo.</param>
    /// <param name="diasSemana">Los días de semana por cupo.</param>
    /// <param name="horaInicio">La hora de inicio para calcular cada cupo.</param>
    /// <param name="horaTermino">La hora de término para calcular cada cupo.</param>
    /// <param name="intervaloMinutos">El intérvalo en minutos para calcular cada cupo.</param>
    /// <param name="total">El total de cupo de todos los cupos.</param>
    /// <param name="sobreCupo">El sobrecupo de todos los cupos.</param>
    /// <param name="topeEnCupo">El tope en todos los cupos.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de crear varios cupos.</returns>
    public async Task<ResultOf<IEnumerable<Cupo>>> CreateCuposAsync(
        DateOnly fechaEfectiva,
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
            return new ValidationError(nameof(Cupo.EscenarioCupo), $"No existe el escenario de cupo {escenarioCupoInfo.Nombre}");
        }

        int seedId = await _repository.MaxIdAsync(cancellationToken);

        return await CreateManyAsync(
             seedId + 1,
             fechaEfectiva,
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
    /// Actualiza un cupo.
    /// </summary>
    /// <param name="id">El id del cupo.</param>
    /// <param name="fechaEfectiva">La fecha efectiva del cupo.</param>
    /// <param name="escenarioCupoInfo">El escenario del cupo.</param>
    /// <param name="canalVenta">El canal de venta del cupo.</param>
    /// <param name="diaSemana">El día de semana del cupo.</param>
    /// <param name="horaInicio">La hora de inicio del cupo.</param>
    /// <param name="horaFin">La hora de fin del cupo.</param>
    /// <param name="total">El total de cupo del cupo.</param>
    /// <param name="sobreCupo">El sobrecupo del cupo.</param>
    /// <param name="topeEnCupo">El tope en el cupo.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de actualizar el cupo.</returns>
    public async Task<ResultOf<Cupo>> UpdateCupoAsync(
        int id,
        DateOnly fechaEfectiva,
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
        Cupo? cupo = await _repository.FindByIdAsync(id, cancellationToken);

        if (cupo is null)
        {
            return new NotFound();
        }

        EscenarioCupo? escenarioCupo = await _escenarioCupoRepository.FindByIdAsync(escenarioCupoInfo.Id, cancellationToken);

        if (escenarioCupo is null)
        {
            return new ValidationError(nameof(Cupo.EscenarioCupo), $"No existe el escenario de cupo {escenarioCupoInfo.Nombre}");
        }

        return await cupo.UpdateAsync(
            fechaEfectiva,
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

    private async Task<ResultOf<IEnumerable<Cupo>>> CreateManyAsync(
        int seedId,
        DateOnly fechaEfectiva,
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
        IEnumerable<(TimeSpan HoraInicio, TimeSpan HoraFin)> horas = CreateHoras(horaInicio, horaTermino, intervaloMinutos);
        IEnumerable<(CanalVenta CanalVenta, DiaSemana DiaSemana, TimeSpan HoraInicio, TimeSpan HoraFin)> cuposToCreate = ConstruirInfoToCreate(canalesVenta, diasSemana, horas);

        List<Cupo> cupos = [];

        foreach (var cupoToCreate in cuposToCreate)
        {
            int id = seedId++;

            var result = await CreateCupoAsync(
                id,
                fechaEfectiva,
                escenarioCupo,
                cupoToCreate.CanalVenta,
                cupoToCreate.DiaSemana,
                cupoToCreate.HoraInicio,
                cupoToCreate.HoraFin,
                total,
                sobreCupo,
                topeEnCupo,
                cupos,
                cancellationToken);

            if (result.IsFailure(out Failure failure))
            {
                return failure;
            }
        }

        return cupos;
    }

    private static IEnumerable<(CanalVenta CanalVenta, DiaSemana DiaSemana, TimeSpan HoraInicio, TimeSpan HoraFin)> ConstruirInfoToCreate(IEnumerable<CanalVenta> canalesVenta, IEnumerable<DiaSemana> diasSemana, IEnumerable<(TimeSpan HoraInicio, TimeSpan HoraFin)> horas)
    {
        return from canalVenta in canalesVenta
               from diaSemana in diasSemana
               from horario in horas
               select (canalVenta, diaSemana, horario.HoraInicio, horario.HoraFin);
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

    private async Task<SuccessOrFailure> CreateCupoAsync(
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
        IList<Cupo> cupos,
        CancellationToken cancellationToken)
    {
        var result = await Cupo.CreateAsync(
            id,
            fechaEfectiva,
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

        return result.Match(
            onSuccess: cupo => AddToList(cupo, cupos),
            onFailure: _ => _);
    }

    private static SuccessOrFailure AddToList(Cupo cupo, IList<Cupo> cupos)
    {
        cupos.Add(cupo);
        return new Success();
    }

    private async Task ValidateDuplicityAsync(
        (int Id,
        DateOnly FechaEfectiva,
        EscenarioCupo EscenarioCupo,
        CanalVenta CanalVenta,
        DiaSemana DiaSemana,
        TimeSpan HoraInicio) cupoToValidate,
        IList<ValidationError> errors,
        CancellationToken cancellationToken)
    {
        Cupo? cupo = await _repository.FindByUniqueKeyAsync(
            cupoToValidate.FechaEfectiva,
            cupoToValidate.EscenarioCupo,
            cupoToValidate.CanalVenta,
            cupoToValidate.DiaSemana,
            cupoToValidate.HoraInicio,
            cancellationToken);

        if (cupo is not null && cupo.Id != cupoToValidate.Id)
        {
            errors.Add(new ValidationError(
                nameof(Cupo),
                $"Ya existe un cupo para la fecha {cupoToValidate.FechaEfectiva}, el escenario {cupoToValidate.EscenarioCupo.Nombre}, el canal de venta {cupoToValidate.CanalVenta.Texto}, el día de semana {cupoToValidate.DiaSemana.Aka} y la hora de inicio {cupoToValidate.HoraInicio}."));
        }
    }
}