using MassTransit.Mediator;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Validate;

/// <summary>
/// El manejador de <see cref="ValidateServicioAka"/>.
/// </summary>
public sealed class ValidateServicioAkaHandler : MediatorRequestHandler<ValidateServicioAka, SuccessOrFailure>
{
    private readonly ServicioValidator _servicioValidator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ValidateServicioAkaHandler"/>.
    /// </summary>
    /// <param name="servicioValidator"></param>
    public ValidateServicioAkaHandler(ServicioValidator servicioValidator)
    {
        ArgumentNullException.ThrowIfNull(servicioValidator);
        _servicioValidator = servicioValidator;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> Handle(ValidateServicioAka request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _servicioValidator.ValidateAkaAsync(request.ServicioId, request.FranquiciaId, request.Aka, cancellationToken);
    }
}
