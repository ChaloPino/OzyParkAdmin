using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Infrastructure.Shared;
using System.Text;
using System.Text.Encodings.Web;

namespace OzyParkAdmin.Infrastructure.Identity;

/// <summary>
/// Implementa <see cref="IUserEmailSender"/>.
/// </summary>
public sealed class UserEmailSender : IUserEmailSender, IInfrastructure
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IEmailSender<Usuario> _emailSender;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UserEmailSender"/>.
    /// </summary>
    /// <param name="userManager">El <see cref="UserManager{TUser}"/>.</param>
    /// <param name="emailSender">El <see cref="IEmailSender{TUser}"/>.</param>
    public UserEmailSender(UserManager<Usuario> userManager, IEmailSender<Usuario> emailSender)
    {
        ArgumentNullException.ThrowIfNull(userManager);
        ArgumentNullException.ThrowIfNull(emailSender);
        _userManager = userManager;
        _emailSender = emailSender;
    }

    /// <inheritdoc/>
    public async Task SendEmailConfirmationToken(string userName, string confirmationLink, CancellationToken cancellationToken)
    {
        Usuario? usuario = await _userManager.FindByNameAsync(userName);

        if (usuario is not null)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            string callbackUrl = HtmlEncoder.Default.Encode($"{confirmationLink}?userId={usuario.Id}&code={code}");

            await _emailSender.SendConfirmationLinkAsync(usuario, usuario.Email!, callbackUrl);
        }
    }
}
