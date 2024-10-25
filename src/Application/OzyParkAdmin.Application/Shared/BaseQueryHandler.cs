using MassTransit.Mediator;
using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Shared;

/// <summary>
/// El manejador base de cualquier <see cref="IQuery{TRespose}"/>
/// </summary>
public abstract class BaseQueryHandler<TQuery, TResponse> : MediatorRequestHandler<TQuery, ResultOf<TResponse>>
    where TQuery : class, IQuery<TResponse>
{
    /// <summary>
    /// El nombre de la consulta.
    /// </summary>
    protected readonly string QueryName;

    /// <summary>
    /// Crea una nueva instancia de un manejador de consultas.
    /// </summary>
    /// <param name="logger">El <see cref="ILogger"/>.</param>
    protected BaseQueryHandler(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Logger = logger;
        QueryName = typeof(TQuery).Name;
    }

    /// <summary>
    /// El <see cref="ILogger"/>.
    /// </summary>
    public ILogger Logger { get; }

    /// <inheritdoc/>
    protected override sealed async Task<ResultOf<TResponse>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        QueryLoggers.LogHandlingQuery(Logger, QueryName);

        try
        {
            var response = await ExecuteAsync(request, cancellationToken);

            return response.Match(
                onSuccess: LogSuccess,
                onFailure: LogFailure);
        }
        catch (Exception ex)
        {
            Guid ticket = Guid.NewGuid();
            //Esto va al log correspondiente
            QueryLoggers.LogHandlingQueryException(Logger, QueryName, ticket, ex);
            //Esto va a dar a Presentacion
            return new Unknown(ticket, [$"Error al procesar {QueryName}"]);
        }
    }

    /// <summary>
    /// Ejecuta la consulta.
    /// </summary>
    /// <param name="query">La consulta a ser ejecutada.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de ejecutar <paramref name="query"/>.</returns>
    protected abstract Task<ResultOf<TResponse>> ExecuteAsync(TQuery query, CancellationToken cancellationToken);

    private ResultOf<TResponse> LogSuccess(TResponse response)
    {
        CommandLoggers.LogCommandHandled(Logger, QueryName);
        LogDetails(response);
        return response;
    }

    private ResultOf<TResponse> LogFailure(Failure failure)
    {
        CommandLoggers.LogHandlingCommandError(Logger, QueryName, failure);
        return failure;
    }

    /// <summary>
    /// Registra el detalle del resultado de la ejecución de <typeparamref name="TQuery"/>.
    /// </summary>
    /// <param name="response">La respuesta de la ejecución de <typeparamref name="TQuery"/> para ser registrada en el log como detalle.</param>
    /// <remarks>
    /// Se puede utilizar la invocación de <see cref="CommandLoggers.LogCommandHandledDetail(ILogger, string, object)"/>,
    /// que registrará a <paramref name="response"/> en <see cref="LogLevel.Debug"/>.
    /// </remarks>
    protected virtual void LogDetails(TResponse? response)
    {
    }
}

/// <summary>
/// El manejador base que cualquier consulta que retorne un elemento de <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TQuery">El tipo de la consulta.</typeparam>
/// <typeparam name="TResponse">El tipo de la respuesta.</typeparam>
/// <param name="logger">El <see cref="ILogger"/>.</param>
public abstract class QueryHandler<TQuery, TResponse>(ILogger logger) : BaseQueryHandler<TQuery, TResponse>(logger)
    where TQuery : class, IQuery<TResponse>
    where TResponse : class
{
    /// <inheritdoc/>
    protected override sealed async Task<ResultOf<TResponse>> ExecuteAsync(TQuery query, CancellationToken cancellationToken)
    {
        TResponse? response = await ExecuteQueryAsync(query, cancellationToken);
        return response is null ? new NotFound() : response;
    }

    /// <summary>
    /// Ejecuta la consulta.
    /// </summary>
    /// <param name="query">La consulta a ser ejecutada.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de ejecutar <paramref name="query"/>.</returns>
    protected abstract Task<TResponse?> ExecuteQueryAsync(TQuery query, CancellationToken cancellationToken);
}

/// <summary>
/// El manejador base que cualquier consulta que retorne una lista paginada de <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TQuery">El tipo de la consulta.</typeparam>
/// <typeparam name="TResponse">El tipo de la respuesta.</typeparam>
/// <param name="logger">El <see cref="ILogger"/>.</param>
public abstract class QueryPagedOfHandler<TQuery, TResponse>(ILogger logger) : BaseQueryHandler<TQuery, PagedList<TResponse>>(logger)
    where TQuery : class, IQueryPagedOf<TResponse>
{
    /// <inheritdoc/>
    protected override sealed async Task<ResultOf<PagedList<TResponse>>> ExecuteAsync(TQuery query, CancellationToken cancellationToken) =>
        await ExecutePagedListAsync(query, cancellationToken);

    /// <summary>
    /// Ejecuta la consulta.
    /// </summary>
    /// <param name="query">La consulta a ser ejecutada.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de ejecutar <paramref name="query"/>.</returns>
    protected abstract Task<PagedList<TResponse>> ExecutePagedListAsync(TQuery query, CancellationToken cancellationToken);
}

/// <summary>
/// El manejador base que cualquier consulta que retorne una lista de <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TQuery">El tipo de la consulta.</typeparam>
/// <typeparam name="TResponse">El tipo de la respuesta.</typeparam>
/// <param name="logger">El <see cref="ILogger"/>.</param>
public abstract class QueryListOfHandler<TQuery, TResponse>(ILogger logger) : BaseQueryHandler<TQuery, List<TResponse>>(logger)
    where TQuery : class, IQueryListOf<TResponse>
{
    /// <inheritdoc/>
    protected override sealed async Task<ResultOf<List<TResponse>>> ExecuteAsync(TQuery query, CancellationToken cancellationToken) =>
        await ExecuteListAsync(query, cancellationToken);

    /// <summary>
    /// Ejecuta la consulta.
    /// </summary>
    /// <param name="query">La consulta a ser ejecutada.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de ejecutar <paramref name="query"/>.</returns>
    protected abstract Task<List<TResponse>> ExecuteListAsync(TQuery query, CancellationToken cancellationToken);
}

/// <summary>
/// El manejador base que cualquier consulta que retorne algún valor de <typeparamref name="TResponse"/> o ninguno.
/// </summary>
/// <typeparam name="TQuery">El tipo de la consulta.</typeparam>
/// <typeparam name="TResponse">El tipo de la respuesta.</typeparam>
/// <param name="logger">El <see cref="ILogger"/>.</param>
public abstract class QuerySomeOfHandler<TQuery, TResponse>(ILogger logger) : BaseQueryHandler<TQuery, SomeOrNone<TResponse>>(logger)
    where TQuery : class, IQuerySomeOf<TResponse>
{
    /// <inheritdoc/>
    protected override async Task<ResultOf<SomeOrNone<TResponse>>> ExecuteAsync(TQuery query, CancellationToken cancellationToken) =>
        await ExecuteSomeAsync(query, cancellationToken);

    /// <summary>
    /// Ejecuta la consulta.
    /// </summary>
    /// <param name="query">La consulta a ser ejecutada.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de ejecutar <paramref name="query"/>.</returns>
    protected abstract Task<SomeOrNone<TResponse>> ExecuteSomeAsync(TQuery query, CancellationToken cancellationToken);
}
