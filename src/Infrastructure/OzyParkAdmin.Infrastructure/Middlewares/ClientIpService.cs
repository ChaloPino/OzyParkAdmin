using Microsoft.AspNetCore.Http;
using OzyParkAdmin.Application.Shared;

namespace OzyParkAdmin.Infrastructure.Middlewares;

/// <summary>
/// Permite conseguir la IP del cliente.
/// </summary>
public sealed class ClientIpService : IClientIpService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ClientIpService"/>.
    /// </summary>
    /// <param name="httpContextAccessor">El <see cref="IHttpContextAccessor"/>.</param>
    public ClientIpService(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Consigue la IP del cliente.
    /// </summary>
    /// <returns>La IP del cliente o <c>0.0.0.0</c> si es que no lo encuentra.</returns>
    public string GetIpClient()
    {
        return _httpContextAccessor.HttpContext?.Items["ClientIp"]?.ToString() ?? "0.0.0.0";
    }
}
