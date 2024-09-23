using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios;

/// <summary>
/// El manejador base para todos los cambios de estado del <see cref="Servicio"/>.
/// </summary>
/// <typeparam name="TState">El tipo de cambio de estado.</typeparam>
public abstract class ServicioStateChangeableHandler<TState> : MediatorRequestHandler<TState, ResultOf<ServicioFullInfo>>
    where TState : class, IServicioStateChangeable
{
    private readonly StateAction _stateAction;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ServicioStateChangeableHandler{TState}"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="stateAction">La acción de cambio de estado que maneja este manejador.</param>
    protected ServicioStateChangeableHandler(IOzyParkAdminContext context, StateAction stateAction = StateAction.Update)
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
    protected sealed override async Task<ResultOf<ServicioFullInfo>> Handle(TState request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        ResultOf<Servicio> result = await ExecuteAsync(request, cancellationToken);

        return await result.MatchResultOfAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Ejecuta el cambio de estado del servicio.
    /// </summary>
    /// <param name="request">El cambio de estado que se desea realizar en el servicio.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado del cambio de estado del servicio.</returns>
    protected abstract Task<ResultOf<Servicio>> ExecuteAsync(TState request, CancellationToken cancellationToken);

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
