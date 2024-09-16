using Microsoft.AspNetCore.Identity;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Contiene métodos de extensión para <see cref="IdentityResult"/>.
/// </summary>
public static class IdentityResultExtensions
{
    /// <summary>
    /// Convierte un <see cref="IdentityResult"/> con errores a <see cref="Failure"/>.
    /// </summary>
    /// <param name="result">El <see cref="IdentityResult"/> a convertir.</param>
    /// <returns>Un <see cref="Failure"/>.</returns>
    public static Failure ToFailure(this IdentityResult result)
    {
        return result.Errors.Select(ToValidationError).ToArray();
    }

    private static ValidationError ToValidationError(IdentityError error)
    {
        return new ValidationError(error.Code, error.Description);
    }
}
