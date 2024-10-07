namespace OzyParkAdmin.Application.Shared;

/// <summary>
/// Permite conseguir la IP del cliente.
/// </summary>
public interface IClientIpService
{
    /// <summary>
    /// Consigue la IP del cliente.
    /// </summary>
    /// <returns>La IP del cliente o <c>0.0.0.0</c> si es que no lo encuentra.</returns>
    public string GetIpClient();
}