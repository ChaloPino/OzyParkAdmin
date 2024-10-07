using Microsoft.AspNetCore.Http;

namespace OzyParkAdmin.Infrastructure.Middlewares;

/// <summary>
/// El middleware para conseguir la IP del cliente.
/// </summary>
public class ClientIpMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    /// <summary>
    /// Consigue la IP del cliente.
    /// </summary>
    /// <param name="context">El <see cref="HttpContext"/>.</param>
    /// <returns>Una tarea asíncrona.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        context.Items["ClientIp"] = (context.Connection.RemoteIpAddress?.ToString());

        await _next(context);
    }
}
