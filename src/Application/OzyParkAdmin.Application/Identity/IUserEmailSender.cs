namespace OzyParkAdmin.Application.Identity;

/// <summary>
/// Servicio que envía los mensajes por correo electrónico del usuario.
/// </summary>
public interface IUserEmailSender
{
    /// <summary>
    /// Envía el mensaje de confirmación de correo electrónico.
    /// </summary>
    /// <param name="userName">El nombre del usuario.</param>
    /// <param name="confirmationLink">La url del la confirmación.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>La tarea.</returns>
    Task SendEmailConfirmationToken(string userName, string confirmationLink, CancellationToken cancellationToken);
}
