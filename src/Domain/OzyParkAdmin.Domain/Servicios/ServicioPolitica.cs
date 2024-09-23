
namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Las políticas de un servicio.
/// </summary>
public sealed class ServicioPolitica
{
    /// <summary>
    /// Las políticas
    /// </summary>
    public string Politicas { get; private set; } = string.Empty;

    internal static ServicioPolitica Create(string politicas)
    {
        return new() { Politicas = politicas.Trim() };
    }

    internal void Update(string politicas) =>
        Politicas = politicas.Trim();
}