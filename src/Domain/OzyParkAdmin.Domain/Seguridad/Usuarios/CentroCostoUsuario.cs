namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Centro de costo asociado al usuario.
/// </summary>
public sealed class CentroCostoUsuario
{
    /// <summary>
    /// Identificador del usuario.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Identificador del centro de costo.
    /// </summary>
    public int CentroCostoId { get; private set; }

    internal static CentroCostoUsuario Create(Usuario usuario, int centroCostoId)
    {
        return new() { UserId = usuario.Id, CentroCostoId = centroCostoId };
    }
}