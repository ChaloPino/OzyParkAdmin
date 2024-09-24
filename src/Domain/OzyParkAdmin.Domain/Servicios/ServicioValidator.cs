using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Contiene funciones de validación de la información de un servicio.
/// </summary>
public sealed class ServicioValidator
{
    private readonly IServicioRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ServicioValidator"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IServicioRepository"/>.</param>
    public ServicioValidator(IServicioRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <summary>
    /// Valida el aka de un servicio.
    /// </summary>
    /// <param name="servicioId">El id del servicio a validar.</param>
    /// <param name="franquiciaId">El id de la franquicia.</param>
    /// <param name="aka">El aka del servicio.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de validar el aka del servicio.</returns>
    public async Task<SuccessOrFailure> ValidateAkaAsync(int servicioId, int franquiciaId, string? aka, CancellationToken cancellationToken)
    {
        Servicio? servicio = await _repository.FindByAkaAsync(franquiciaId, aka, cancellationToken);

        if (servicio is not null && servicio.Id != servicioId)
        {
            return new ValidationError("Aka", $"Ya existe un servicio con el aka '{aka}'.");
        }

        return new Success();
    }
}
