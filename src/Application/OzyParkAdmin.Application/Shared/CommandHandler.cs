using MassTransit.Mediator;
using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Shared;

/// <summary>
/// El manejador base de <see cref="ICommand{TResponse}"/>.
/// </summary>
/// <typeparam name="TCommand">El tipo del comando.</typeparam>
/// <typeparam name="TResponse">El tipo de la respuesta.</typeparam>
public abstract class CommandHandler<TCommand, TResponse> : MediatorRequestHandler<TCommand, ResultOf<TResponse>>
    where TCommand : class, ICommand<TResponse>
{
    /// <summary>
    /// El nombre del comando.
    /// </summary>
    protected readonly string CommandName;

    /// <summary>
    /// Crea una nueva instancia de un manejador de comandos.
    /// </summary>
    /// <param name="logger">El <see cref="ILogger"/>.</param>
    protected CommandHandler(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Logger = logger;
        CommandName = typeof(TCommand).Name;
    }

    /// <summary>
    /// El <see cref="ILogger"/>.
    /// </summary>
    protected ILogger Logger { get; }

    /// <inheritdoc/>
    protected override sealed async Task<ResultOf<TResponse>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        CommandLoggers.LogHandlingCommand(Logger, CommandName);

        try
        {
            var result = await ExecuteAsync(request, cancellationToken);

            return result.Match(
                onSuccess: LogSuccess,
                onFailure: LogFailure);
        }
        catch (Exception ex)
        {
            Guid ticket = Guid.NewGuid();
            CommandLoggers.LogHandlingCommandException(Logger, CommandName, ticket, ex);
            return new Unknown(ticket, [$"Error al procesar {CommandName}"]);
        }
    }

    /// <summary>
    /// Ejecuta el comando.
    /// </summary>
    /// <param name="command">El comando a ser ejecutado.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de ejecutar <paramref name="command"/>.</returns>
    protected abstract Task<ResultOf<TResponse>> ExecuteAsync(TCommand command, CancellationToken cancellationToken);

    private ResultOf<TResponse> LogSuccess(TResponse response)
    {
        CommandLoggers.LogCommandHandled(Logger, CommandName);
        LogDetails(response);
        return response;
    }

    private ResultOf<TResponse> LogFailure(Failure failure)
    {
        CommandLoggers.LogHandlingCommandError(Logger, CommandName, failure);
        return failure;
    }

    /// <summary>
    /// Registra el detalle del resultado de la ejecución de <typeparamref name="TCommand"/>.
    /// </summary>
    /// <param name="response">La respuesta de la ejecución de <typeparamref name="TCommand"/> para ser registrada en el log como detalle.</param>
    /// <remarks>
    /// Se puede utilizar la invocación de <see cref="CommandLoggers.LogCommandHandledDetail(ILogger, string, object)"/>,
    /// que registrará a <paramref name="response"/> en <see cref="LogLevel.Debug"/>.
    /// </remarks>
    protected virtual void LogDetails(TResponse? response)
    {
    }
}

/// <summary>
/// El manejador base de <see cref="ICommand"/>.
/// </summary>
/// <typeparam name="TCommand">El tipo del comando.</typeparam>
public abstract class CommandHandler<TCommand> : MediatorRequestHandler<TCommand, SuccessOrFailure>
    where TCommand : class, ICommand
{
    /// <summary>
    /// El nombre del comando.
    /// </summary>
    protected readonly string CommandName;

    /// <summary>
    /// Crea una nueva instancia de un manejador de comandos.
    /// </summary>
    /// <param name="logger">El <see cref="ILogger"/>.</param>
    protected CommandHandler(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Logger = logger;
        CommandName = typeof(TCommand).Name;
    }

    /// <summary>
    /// El <see cref="ILogger"/>.
    /// </summary>
    protected ILogger Logger { get; }

    /// <inheritdoc/>
    protected override sealed async Task<SuccessOrFailure> Handle(TCommand request, CancellationToken cancellationToken)
    {
        CommandLoggers.LogHandlingCommand(Logger, CommandName);

        try
        {
            var result = await ExecuteAsync(request, cancellationToken);

            return result.Match(
                onSuccess: LogSuccess,
                onFailure: LogFailure);
        }
        catch (Exception ex)
        {
            Guid ticket = Guid.NewGuid();
            CommandLoggers.LogHandlingCommandException(Logger, CommandName, ticket, ex);
            return new Unknown(ticket, [$"Error al procesar {CommandName}"]);
        }
    }

    /// <summary>
    /// Ejecuta el comando.
    /// </summary>
    /// <param name="command">El comando a ser ejecutado.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de ejecutar <paramref name="command"/>.</returns>
    protected abstract Task<SuccessOrFailure> ExecuteAsync(TCommand command, CancellationToken cancellationToken);

    private SuccessOrFailure LogSuccess(Success success)
    {
        CommandLoggers.LogCommandHandled(Logger, CommandName);
        return success;
    }

    private SuccessOrFailure LogFailure(Failure failure)
    {
        CommandLoggers.LogHandlingCommandError(Logger, CommandName, failure);
        return failure;
    }
}