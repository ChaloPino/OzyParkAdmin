using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Confirmation;

/// <summary>
/// El manejador de <see cref="ConfirmEmail"/>.
/// </summary>
public sealed class ConfirmEmailHandler : CommandHandler<ConfirmEmail>
{
    private readonly IUserEmailSender _emailSender;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ConfirmEmailHandler"/>.
    /// </summary>
    /// <param name="emailSender">El <see cref="IUserEmailSender"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ConfirmEmailHandler(IUserEmailSender emailSender, ILogger<ConfirmEmailHandler> logger)
        :  base(logger)
    {
        ArgumentNullException.ThrowIfNull(emailSender);
        _emailSender = emailSender;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(ConfirmEmail command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        await _emailSender.SendEmailConfirmationToken(command.UserName, command.ConfirmationLink, cancellationToken);
        return new Success();
    }
}
