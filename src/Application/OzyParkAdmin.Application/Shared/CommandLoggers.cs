using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Shared;
internal static partial class CommandLoggers
{
    [LoggerMessage(EventId = 2000, Level = LogLevel.Information, Message = "Manejando el commando '{CommandName}'")]
    public static partial void LogHandlingCommand(ILogger logger, string commandName);

    [LoggerMessage(EventId = 2001, Level = LogLevel.Information, Message = "Se manejó exitosamente el commando '{CommandName}'")]
    public static partial void LogCommandHandled(ILogger logger, string commandName);

    [LoggerMessage(EventId = 2002, Level = LogLevel.Debug, Message = "Detalle de manejo del commando '{CommandName}': {Detail}")]
    public static partial void LogCommandHandledDetail(ILogger logger, string commandName, object detail);

    [LoggerMessage(EventId = 2003, Level = LogLevel.Error, Message = "Hubo un problema al manejar el commando '{CommandName}': {Failure}")]
    public static partial void LogHandlingCommandError(ILogger logger, string commandName, Failure failure);

    [LoggerMessage(EventId = 2004, Level = LogLevel.Error, Message = "Hubo un problema al manejar el commando '{CommandName}'. Ticket relacionado: {Ticket}")]
    public static partial void LogHandlingCommandException(ILogger logger, string commandName, Guid ticket, Exception exception);
}
