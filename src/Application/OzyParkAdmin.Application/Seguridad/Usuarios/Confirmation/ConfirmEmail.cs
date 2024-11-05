using OzyParkAdmin.Application.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Confirmation;

/// <summary>
/// Trata de confirmar el email del usuario.
/// </summary>
/// <param name="UserName">El nombre del usuario.</param>
/// <param name="ConfirmationLink">El link de confirmación.</param>
public sealed record ConfirmEmail(string UserName, string ConfirmationLink) : ICommand;
