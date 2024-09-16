namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Franquicia asociada al usuario.
/// </summary>
public sealed class FranquiciaUsuario
{
    /// <summary>
    /// Identificador del usuario.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Identificador de la franquicia.
    /// </summary>
    public int FranquiciaId { get; private set; }

    internal static FranquiciaUsuario Create(Usuario usuario, int franquiciaId) =>
        new() { UserId = usuario.Id, FranquiciaId = franquiciaId };
}