namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Contiene la información del usuario.
/// </summary>
public sealed record UsuarioInfo
{
    /// <summary>
    /// El Id del usuario.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// El nombre del usuario.
    /// </summary>
    public string UserName { get; init; } = string.Empty;

    /// <summary>
    /// El nombre completo del usuario.
    /// </summary>
    public string FriendlyName { get; init; } = string.Empty;
}
