using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Claim del usuario.
/// </summary>
public class ClaimUsuario
{
    /// <summary>
    /// Identificador único del claim.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// El identificador del usuario.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Tipo del claim,
    /// </summary>
    public string ClaimType { get; private set; } = string.Empty;

    /// <summary>
    /// Valor del claim.
    /// </summary>
    public string ClaimValue { get; private set; } = string.Empty;

    internal static ClaimUsuario Create(Usuario usuario, Claim claim) =>
        new()
        {
            Id = usuario.Claims.Count() + 1,
            UserId = usuario.Id,
            ClaimType = claim.Type,
            ClaimValue = claim.Value,
        };

    internal void Replace(Claim claim)
    {
        ClaimType = claim.Type;
        ClaimValue = claim.Value;
    }

    /// <summary>
    /// Convierte la entidad en una instance the <see cref="Claim"/>.
    /// </summary>
    /// <returns>Una instancia de <see cref="Claim"/></returns>
    public Claim ToClaim() =>
        new(ClaimType, ClaimValue);
}