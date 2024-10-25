using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Validate;

/// <summary>
/// El manejador de <see cref="ValidateServicioAka"/>.
/// </summary>
public sealed class ValidateServicioAkaHandler : CommandHandler<ValidateServicioAka>
{
    private readonly ServicioValidator _servicioValidator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ValidateServicioAkaHandler"/>.
    /// </summary>
    /// <param name="servicioValidator">El <see cref="ServicioValidator"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ValidateServicioAkaHandler(ServicioValidator servicioValidator, ILogger<ValidateServicioAkaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(servicioValidator);
        _servicioValidator = servicioValidator;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(ValidateServicioAka command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return await _servicioValidator.ValidateAkaAsync(command.ServicioId, command.FranquiciaId, command.Aka, cancellationToken);
    }
}
