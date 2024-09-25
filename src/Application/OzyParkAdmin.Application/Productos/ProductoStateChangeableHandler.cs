using MassTransit.Mediator;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos;

/// <summary>
/// El manejador base para todos los cambios de estado del <see cref="Producto"/>.
/// </summary>
/// <typeparam name="TState">El tipo de cambio de estado.</typeparam>
public abstract class ProductoStateChangeableHandler<TState> : MediatorRequestHandler<TState, ResultOf<ProductoFullInfo>>
    where TState : class, IProductoStateChangeable
{
    private readonly StateAction _stateAction;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ProductoStateChangeableHandler{TState}"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="stateAction">La acción de cambio de estado que maneja este manejador.</param>
    protected ProductoStateChangeableHandler(IOzyParkAdminContext context, StateAction stateAction = StateAction.Update)
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
    protected sealed override async Task<ResultOf<ProductoFullInfo>> Handle(TState request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        ResultOf<Producto> result = await ExecuteAsync(request, cancellationToken);

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
    protected abstract Task<ResultOf<Producto>> ExecuteAsync(TState request, CancellationToken cancellationToken);

    private async Task<ResultOf<ProductoFullInfo>> SaveAsync(Producto producto, CancellationToken cancellationToken)
    {
        switch (_stateAction)
        {
            case StateAction.Create:
                await Context.AddAsync(producto, cancellationToken);
                break;
            case StateAction.Update:
                Context.Update(producto);
                break;
            case StateAction.Delete:
                Context.Remove(producto);
                break;
        }

        await Context.SaveChangesAsync(cancellationToken);

        return producto.ToFullInfo();
    }
}
