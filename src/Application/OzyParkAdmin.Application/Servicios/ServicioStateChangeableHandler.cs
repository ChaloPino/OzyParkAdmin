using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios;

/// <summary>
/// El manejador base para todos los cambios de estado del <see cref="Servicio"/>.
/// </summary>
/// <typeparam name="TState">El tipo de cambio de estado.</typeparam>
public abstract class ServicioStateChangeableHandler<TState> : CommandHandler<TState, ServicioFullInfo>
    where TState : class, IServicioStateChangeable
{
    private readonly StateAction _stateAction;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ServicioStateChangeableHandler{TState}"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="logger">El <see cref="ILogger"/>.</param>
    /// <param name="stateAction">La acción de cambio de estado que maneja este manejador. Por defecto es <see cref="StateAction.Update"/></param>
    protected ServicioStateChangeableHandler(IOzyParkAdminContext context, ILogger logger, StateAction stateAction = StateAction.Update)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        Context = context;
        _stateAction = stateAction;
    }

    /// <summary>
    /// El <see cref="IOzyParkAdminContext"/>.
    /// </summary>
    protected IOzyParkAdminContext Context { get; }

    /// <inheritdoc/>
    protected override async Task<ResultOf<ServicioFullInfo>> ExecuteAsync(TState command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        ResultOf<Servicio> result = await ExecuteChangeStateAsync(command, cancellationToken);

        return await result.BindAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Ejecuta el cambio de estado del servicio.
    /// </summary>
    /// <param name="command">El cambio de estado que se desea realizar en el servicio.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado del cambio de estado del servicio.</returns>
    protected abstract Task<ResultOf<Servicio>> ExecuteChangeStateAsync(TState command, CancellationToken cancellationToken);

    private async Task<ResultOf<ServicioFullInfo>> SaveAsync(Servicio servicio, CancellationToken cancellationToken)
    {
        switch (_stateAction)
        {
            case StateAction.Create:
                await Context.AddAsync(servicio, cancellationToken);
                break;
            case StateAction.Update:
                Context.Update(servicio);
                break;
            case StateAction.Delete:
                Context.Remove(servicio);
                break;
        }

        await Context.SaveChangesAsync(cancellationToken);

        return servicio.ToInfo();
    }
}
