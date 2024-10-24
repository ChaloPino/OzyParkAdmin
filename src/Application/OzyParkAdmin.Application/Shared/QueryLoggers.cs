using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Shared;
internal static partial class QueryLoggers
{
    [LoggerMessage(EventId = 1000, Level = LogLevel.Information, Message = "Manejando la consulta '{QueryName}'")]
    public static partial void LogHandlingQuery(ILogger logger, string queryName);

    [LoggerMessage(EventId = 1001, Level = LogLevel.Information, Message = "Se manejó exitosamente la consulta '{QueryName}'")]
    public static partial void LogQueryHandled(ILogger logger, string queryName);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Debug, Message = "Detalle del manejo de la consulta '{QueryName}': {Response}")]
    public static partial void LogQueryHandledDetail(ILogger logger, string queryName, object response);

    [LoggerMessage(EventId = 1003, Level = LogLevel.Error, Message = "Hubo un problema al manejar la consulta '{QueryName}': {Failure}")]
    public static partial void LogHandingQueryError(ILogger logger, string queryName, Failure failure);

    [LoggerMessage(EventId = 1004, Level = LogLevel.Error, Message = "Hubo un problema al manejar la consulta '{QueryName}'. Ticket relacionado: {Ticket}")]
    public static partial void LogHandlingQueryException(ILogger logger, string queryName, Guid ticket, Exception exception);
}
