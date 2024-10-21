using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// Contiene la lógica de negocios para la caja.
/// </summary>
public class CajaManager : IBusinessLogic
{
    private readonly ICajaRepository _repository;
    private readonly IUsuarioRepository _usuarioRepository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CajaManager"/>.
    /// </summary>
    /// <param name="repository">El <see cref="ICajaRepository"/>.</param>
    /// <param name="usuarioRepository">El <see cref="IUsuarioRepository"/>.</param>
    public CajaManager(ICajaRepository repository, IUsuarioRepository usuarioRepository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(usuarioRepository);
        _repository = repository;
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Cierra el día de una caja.
    /// </summary>
    /// <param name="diaId">El id del día de apertura de la caja.</param>
    /// <param name="supervisorInfo">El usuario supervisor que cierra el día.</param>
    /// <param name="comentario">El comentario del supervisor.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <param name="montoEfectivoParaCierre">El monto total de efectivo calculado de todos los turnos para el cierre.</param>
    /// <param name="montoTransbankParaCierre">El monto total de tarjetas de crédito y de débito calulado para el cierre.</param>
    /// <returns>El resultado del cierre del día.</returns>
    public async Task<ResultOf<AperturaDia>> CerrarDiaAsync(Guid diaId, UsuarioInfo supervisorInfo, string comentario, decimal montoEfectivoParaCierre, decimal montoTransbankParaCierre, CancellationToken cancellationToken)
    {
        AperturaDia? apertura = await _repository.FindAperturaDiaAsync(diaId, cancellationToken);

        if (apertura is null)
        {
            return new NotFound();
        }

        Usuario? supervisor = await _usuarioRepository.FindByIdAsync(supervisorInfo.Id, cancellationToken);

        if (supervisor is null)
        {
            return new ValidationError(nameof(AperturaTurno.Supervisor), $"No existe el supervisor {supervisorInfo.FriendlyName}.");
        }

        apertura.Cerrar(supervisor, comentario, montoEfectivoParaCierre, montoTransbankParaCierre);
        return apertura;

    }

    /// <summary>
    /// Cierra el turno de una caja.
    /// </summary>
    /// <param name="diaId">El id del dia de la caja.</param>
    /// <param name="turnoId">El id del turno a cerrar.</param>
    /// <param name="supervisorInfo">El supervisor que está cerrando el turno.</param>
    /// <param name="regularizacionEfectivo">El monto de regularización en efectivo.</param>
    /// <param name="regularizacionMontoTransbank">El monto de regularización de Transbank.</param>
    /// <param name="comentario">El comentario de cierre.</param>
    /// <param name="movimientos">Los movimientos del turno.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado del cierre del turno.</returns>
    public async Task<ResultOf<AperturaDia>> CerrarTurnoAsync(Guid diaId, Guid turnoId, UsuarioInfo supervisorInfo, decimal regularizacionEfectivo, decimal regularizacionMontoTransbank, string comentario, IEnumerable<DetalleTurnoInfo> movimientos, CancellationToken cancellationToken)
    {
        AperturaDia? apertura = await _repository.FindAperturaDiaAsync(diaId, cancellationToken);

        if (apertura is null)
        {
            return new NotFound();
        }

        AperturaTurno? turno =  apertura.FindTurno(turnoId);

        if (turno is null)
        {
            return new NotFound();
        }

        Usuario? supervisor = await _usuarioRepository.FindByIdAsync(supervisorInfo.Id, cancellationToken);

        if (supervisor is null)
        {
            return new ValidationError(nameof(AperturaTurno.Supervisor), $"No existe el supervisor {supervisorInfo.FriendlyName}.");
        }

        turno.Cerrar(regularizacionEfectivo, regularizacionMontoTransbank, supervisor, comentario, movimientos);

        return apertura;
    }

    /// <summary>
    /// Reapertura el día de una caja.
    /// </summary>
    /// <param name="diaId">El id del día a reaperturar.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de la reapertura del día.</returns>
    public async Task<ResultOf<AperturaDia>> ReabrirDiaAsync(Guid diaId, CancellationToken cancellationToken)
    {
        AperturaDia? apertura = await _repository.FindAperturaDiaAsync(diaId, cancellationToken);

        if (apertura is null)
        {
            return new NotFound();
        }

        apertura.Reabrir();
        return apertura;
    }

    /// <summary>
    /// Reabre el turno de una caja.
    /// </summary>
    /// <param name="diaId">El id del dia de la caja.</param>
    /// <param name="turnoId">El id del turno a reabrir.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de la reapertura del turno.</returns>
    public async Task<ResultOf<AperturaDia>> ReabrirTurnoAsync(Guid diaId, Guid turnoId, CancellationToken cancellationToken)
    {
        AperturaDia? apertura = await _repository.FindAperturaDiaAsync(diaId, cancellationToken);

        if (apertura is null)
        {
            return new NotFound();
        }

        AperturaTurno? turno = apertura.FindTurno(turnoId);

        if (turno is null)
        {
            return new NotFound();
        }

        turno.Reabrir();
        return apertura;
    }
}
