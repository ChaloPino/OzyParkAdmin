using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos;

/// <summary>
/// El manejador base para todos los cambios de estado del <see cref="Producto"/>.
/// </summary>
/// <typeparam name="TState">El tipo de cambio de estado.</typeparam>
public abstract class ProductoStateChangeableHandler<TState> : CommandHandler<TState, ProductoFullInfo>
    where TState : class, IProductoStateChangeable
{
    private readonly StateAction _stateAction;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ProductoStateChangeableHandler{TState}"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="logger">El <see cref="ILogger"/>.</param>
    /// <param name="stateAction">La acción de cambio de estado que maneja este manejador. Valor por defecto es el update.</param>
    protected ProductoStateChangeableHandler(IOzyParkAdminContext context, ILogger logger, StateAction stateAction = StateAction.Update)
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
    protected override async Task<ResultOf<ProductoFullInfo>> ExecuteAsync(TState command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        ResultOf<Producto> result = await ExecuteChangeStateAsync(command, cancellationToken);

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
    protected abstract Task<ResultOf<Producto>> ExecuteChangeStateAsync(TState command, CancellationToken cancellationToken);

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
