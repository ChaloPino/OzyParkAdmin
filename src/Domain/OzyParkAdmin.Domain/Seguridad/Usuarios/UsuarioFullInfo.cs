using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.Seguridad.Roles;

namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Información del usuario.
/// </summary>
public sealed record UsuarioFullInfo
{
    /// <summary>
    /// Id del usuario.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// Nombre completo del usuario.
    /// </summary>
    public required string FriendlyName { get; set; }

    /// <summary>
    /// Dirección de correo electrónico del usuario.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// El rut del usuario.
    /// </summary>
    public string? Rut { get; set; }

    /// <summary>
    /// Si el usuario está bloqueado.
    /// </summary>
    public bool IsLockedout { get; set; }

    /// <summary>
    /// Roles asociados al usuario.
    /// </summary>
    public List<Rol> Roles { get; set; } = [];

    /// <summary>
    /// Franquicias asociadas al usuario.
    /// </summary>
    public List<FranquiciaInfo> Franquicias { get; set; } = [];

    /// <summary>
    /// Centros de costo asociados al usuario.
    /// </summary>
    public List<CentroCostoInfo> CentrosCosto { get; set; } = [];
}
