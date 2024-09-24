using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Validate;

/// <summary>
/// El manejador de <see cref="ValidateAka"/>.
/// </summary>
public sealed class ValidateAkaHandler : MediatorRequestHandler<ValidateAka, SuccessOrFailure>
{
    private readonly ServicioValidator _servicioValidator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ValidateAkaHandler"/>.
    /// </summary>
    /// <param name="servicioValidator"></param>
    public ValidateAkaHandler(ServicioValidator servicioValidator)
    {
        ArgumentNullException.ThrowIfNull(servicioValidator);
        _servicioValidator = servicioValidator;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> Handle(ValidateAka request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _servicioValidator.ValidateAkaAsync(request.ServicioId, request.FranquiciaId, request.Aka, cancellationToken);
    }
}
