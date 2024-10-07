using Microsoft.AspNetCore.Builder;

namespace OzyParkAdmin.Infrastructure.Middlewares;

/// <summary>
/// Permite registrar el <see cref="ClientIpMiddleware"/>.
/// </summary>
public static class ClientIpMiddlewareExtensions
{

    /// <summary>
    /// Registra el <see cref="ClientIpMiddleware"/>.
    /// </summary>
    /// <param name="app">La <see cref="IApplicationBuilder"/>.</param>
    /// <returns>El mismo <paramref name="app"/> de tal forma que se puedan invocar operaciones en cadena.</returns>
    public static IApplicationBuilder UseClientIp(this IApplicationBuilder app) =>
        app.UseMiddleware<ClientIpMiddleware>();

}
