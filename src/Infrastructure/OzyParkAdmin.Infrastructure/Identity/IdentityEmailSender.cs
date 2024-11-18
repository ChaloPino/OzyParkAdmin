using Microsoft.AspNetCore.Identity;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Infrastructure.Notification;
using System.Net.Http.Json;

namespace OzyParkAdmin.Infrastructure.Identity;
internal sealed class IdentityEmailSender : IEmailSender<Usuario>
{
    private readonly HttpClient _client;

    public IdentityEmailSender(HttpClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
    }

    public async Task SendConfirmationLinkAsync(Usuario user, string email, string confirmationLink)
    {
        const string subject = "Confirmacion de cuenta";
        string message = $"Favor <a href=\"{ confirmationLink }\">confirme su correo</a>.";

        var notificationRequest = new NotificationRequest(
            [
                new EmailNotificationRequest(
                    "IDENTITY",
                    [
                        new(email, user.FriendlyName),
                    ],
                    [],
                    [],
                    null,
                    new Dictionary<string, DataItem>
                    {
                        ["Subject"] = new DataItem(subject, "string"),
                        ["Message"] = new DataItem(message, "string"),
                    },
                    []
                )
            ],
            []);
        await _client.PostAsJsonAsync("/notification", notificationRequest);
    }

    public Task SendPasswordResetCodeAsync(Usuario user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }

    public async Task SendPasswordResetLinkAsync(Usuario user, string email, string resetLink)
    {
        const string subject = "Reseteo de contraseña";
        string message = $"Favor <a href=\"{resetLink}\">resetee su contraseña</a>.";

        var notificationRequest = new NotificationRequest(
            [
                new EmailNotificationRequest(
                    "IDENTITY",
                    [
                        new(email, user.FriendlyName),
                    ],
                    [],
                    [],
                    null,
                    new Dictionary<string, DataItem>
                    {
                        ["Subject"] = new DataItem(subject, "string"),
                        ["Message"] = new DataItem(message, "string"),
                    },
                    []
                )
            ],
            []);
        await _client.PostAsJsonAsync("/notification", notificationRequest);
    }
}
