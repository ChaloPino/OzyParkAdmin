using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.ExclusionesCupo;

/// <summary>
/// Lógica de negocios para gestionar los <see cref="EscenarioCupo"/>.
/// Esta clase proporciona métodos para crear, actualizar y marcar escenarios de cupo.
/// </summary>
public sealed class EscenarioCupoManager : IBusinessLogic
{
    private readonly ICentroCostoRepository _centroCostoRepository;
    private readonly IEscenarioCupoRepository _escenarioCupoRepository;
    private readonly IZonaRepository _zonaRepository;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="EscenarioCupoManager"/> con las dependencias necesarias.
    /// </summary>
    public EscenarioCupoManager(
        IEscenarioCupoRepository escenarioCupoRepository,
        ICentroCostoRepository centroCostoRepository,
        IZonaRepository zonaRepository)
    {
        ArgumentNullException.ThrowIfNull(escenarioCupoRepository);
        ArgumentNullException.ThrowIfNull(centroCostoRepository);
        ArgumentNullException.ThrowIfNull(zonaRepository);

        _escenarioCupoRepository = escenarioCupoRepository;
        _centroCostoRepository = centroCostoRepository;
        _zonaRepository = zonaRepository;
    }

    /// <summary>
    /// Crea un nuevo escenario de cupo con la información básica.
    /// </summary>
    public async Task<ResultOf<EscenarioCupo>> CreateAsync(
        CentroCostoInfo centroCostoInfo,
        ZonaInfo? zonaInfo,
        string nombre,
        bool esHoraInicio,
        int minutosAntes,
        bool esActivo,
        CancellationToken cancellationToken)
    {
        // Validar CentroCosto y Zona
        var zonaResult = await ValidateCentroCostoAndZonaAsync(centroCostoInfo, zonaInfo, cancellationToken);
        if (zonaResult.IsFailure(out var failure))
        {
            return failure;
        }

        if (zonaResult.IsSuccess(out var entidades))
        {
            var (centroCosto, zona) = entidades;

            // Crear el escenario de cupo
            var lastId = await _escenarioCupoRepository.GetLastIdAsync(cancellationToken) + 1;
            return EscenarioCupo.Create(lastId, centroCosto, zona, nombre, esHoraInicio, minutosAntes, esActivo);
        }

        return new Failure();
    }

    /// <summary>
    /// Actualiza la información básica de un escenario de cupo existente.
    /// </summary>
    public async Task<ResultOf<EscenarioCupo>> UpdateAsync(
        int id,
        EscenarioCupo existente,
        CentroCostoInfo centroCostoInfo,
        ZonaInfo? zonaInfo,
        string nombre,
        bool esHoraInicio,
        int minutosAntes,
        bool esActivo,
        CancellationToken cancellationToken)
    {
        // Validar CentroCosto y Zona
        var zonaResult = await ValidateCentroCostoAndZonaAsync(centroCostoInfo, zonaInfo, cancellationToken);
        if (zonaResult.IsFailure(out var failure))
        {
            return failure;
        }

        if (zonaResult.IsSuccess(out var entidades))
        {
            var (centroCosto, zona) = entidades;

            // Actualizar el escenario de cupo
            return EscenarioCupo.Update(existente, centroCosto, zona, nombre, esHoraInicio, minutosAntes, esActivo);
        }

        return new Failure();
    }

    /// <summary>
    /// Marca un escenario de cupo como eliminado.
    /// </summary>
    public ResultOf<EscenarioCupo> MarkAsDeleted(EscenarioCupo existente, bool tieneCuposAsociados)
    {
        if (tieneCuposAsociados)
        {
            return new ValidationError(nameof(EscenarioCupo), "El escenario de cupo tiene cupos asociados y no puede eliminarse.");
        }

        existente.MarcarComoEliminado();

        return existente;
    }

    /// <summary>
    /// Valida que el centro de costo y la zona sean válidos.
    /// </summary>
    private async Task<ResultOf<(CentroCosto CentroCosto, Zona? Zona)>> ValidateCentroCostoAndZonaAsync(
        CentroCostoInfo centroCostoInfo,
        ZonaInfo? zonaInfo,
        CancellationToken cancellationToken)
    {
        var centroCosto = await _centroCostoRepository.FindByIdAsync(centroCostoInfo.Id, cancellationToken);
        if (centroCosto is null)
        {
            return new ValidationError(nameof(EscenarioCupo.CentroCosto), $"No existe el centro de costo '{centroCostoInfo.Descripcion}'.");
        }

        if (zonaInfo is not null)
        {
            var zona = await _zonaRepository.FindByIdAsync(zonaInfo.Id, cancellationToken);
            if (zona is null)
            {
                return new ValidationError(nameof(EscenarioCupo.Zona), $"No existe la zona '{zonaInfo.Descripcion}'.");
            }

            return (centroCosto, zona);
        }

        return (centroCosto, null);
    }
}
