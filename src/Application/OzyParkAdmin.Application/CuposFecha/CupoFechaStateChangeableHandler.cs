using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Productos;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha;

/// <summary>
/// El manejador base para todos los cambios de estado del <see cref="CupoFecha"/>.
/// </summary>
/// <typeparam name="TState">El tipo de cambio de estado.</typeparam>
public abstract class CupoFechaStateChangeableHandler<TState> : CommandHandler<TState, CupoFechaFullInfo>
    where TState : class, ICupoFechaStateChangeable
{
    private readonly StateAction _stateAction;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ProductoStateChangeableHandler{TState}"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="logger">El <see cref="ILogger"/>.</param>
    /// <param name="stateAction">La acción de cambio de estado que maneja este manejador.</param>
    protected CupoFechaStateChangeableHandler(IOzyParkAdminContext context, ILogger logger, StateAction stateAction = StateAction.Update)
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
    protected override async Task<ResultOf<CupoFechaFullInfo>> ExecuteAsync(TState command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        ResultOf<CupoFecha> result = await ExecuteChangeStateAsync(command, cancellationToken);

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
    protected abstract Task<ResultOf<CupoFecha>> ExecuteChangeStateAsync(TState command, CancellationToken cancellationToken);

    private async Task<ResultOf<CupoFechaFullInfo>> SaveAsync(CupoFecha cupoFecha, CancellationToken cancellationToken)
    {
        switch (_stateAction)
        {
            case StateAction.Create:
                await Context.AddAsync(cupoFecha, cancellationToken);
                break;
            case StateAction.Update:
                Context.Update(cupoFecha);
                break;
            case StateAction.Delete:
                Context.Remove(cupoFecha);
                break;
        }

        await Context.SaveChangesAsync(cancellationToken);

        return cupoFecha.ToFullInfo();
    }
}
