using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusiones;
using OzyParkAdmin.Domain.DetallesEscenariosCuposExclusionesFechas;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.ExclusionesCupo;

/// <summary>
/// Lógica de negocios para gestionar los <see cref="EscenarioCupo"/>.
/// Esta clase proporciona métodos para crear y actualizar escenarios de cupo, así como para gestionar sus detalles y exclusionesFecha de fechas asociadas.
/// </summary>
public sealed class EscenarioCupoManager : IBusinessLogic
{
    private readonly ICentroCostoRepository _centroCostoRepository;
    private readonly IEscenarioCupoRepository _escenarioCupoRepository;
    private readonly IZonaRepository _zonaRepository;
    private readonly DetalleEscenarioCupoManager _detalleEscenarioCupoManager;
    private readonly DetalleEscenarioCupoExclusionFechaManager _exclusionManager;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="EscenarioCupoManager"/> con las dependencias necesarias.
    /// </summary>
    public EscenarioCupoManager(
        IEscenarioCupoRepository escenarioCupoRepository,
        ICentroCostoRepository centroCostoRepository,
        IZonaRepository zonaRepository,
        DetalleEscenarioCupoManager detalleEscenarioCupoManager,
        DetalleEscenarioCupoExclusionFechaManager exclusionManager)
    {
        ArgumentNullException.ThrowIfNull(escenarioCupoRepository);
        ArgumentNullException.ThrowIfNull(centroCostoRepository);
        ArgumentNullException.ThrowIfNull(zonaRepository);
        ArgumentNullException.ThrowIfNull(detalleEscenarioCupoManager);
        ArgumentNullException.ThrowIfNull(exclusionManager);

        _centroCostoRepository = centroCostoRepository;
        _escenarioCupoRepository = escenarioCupoRepository;
        _zonaRepository = zonaRepository;
        _detalleEscenarioCupoManager = detalleEscenarioCupoManager;
        _exclusionManager = exclusionManager;
    }
    /// <summary>
    /// Busca si tiene un cupo asociado y por ende, de tenerlo no se puede eliminar
    /// </summary>
    public async Task<bool> CanDelete(int id, CancellationToken cancellationToken)
    {
        return await _escenarioCupoRepository.HasCupoRelated(id, cancellationToken);
    }

    /// <summary>
    /// Crea un nuevo escenario de cupo con sus detalles y exclusionesFecha asociadas.
    /// </summary>
    public async Task<ResultOf<EscenarioCupo>> CreateEscenarioCupoAsync(
        CentroCostoInfo centroCostoInfo,
        ZonaInfo? zonaInfo,
        string nombre,
        bool esHoraInicio,
        int minutosAntes,
        bool esActivo,
        IEnumerable<DetalleEscenarioCupoInfo> detalles,
        IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> exclusiones,
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

            // Crear el escenario de cupo con un ID nuevo
            var lastId = await _escenarioCupoRepository.GetLastIdAsync(cancellationToken) + 1;
            var escenarioCupoResult = EscenarioCupo.Create(lastId, centroCosto, zona, nombre, esHoraInicio, minutosAntes, esActivo, detalles, exclusiones);

            if (escenarioCupoResult.IsSuccess(out var escenarioCupo))
            {
                return escenarioCupo;
            }
        }

        return new Failure(); // Caso genérico de error
    }

    /// <summary>
    /// Crea un nuevo escenario de cupo con sus detalles y exclusionesFecha asociadas.
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
        IEnumerable<DetalleEscenarioCupoInfo> detalles,
        IEnumerable<DetalleEscenarioCupoExclusionFechaFullInfo> exclusionesFecha,
        IEnumerable<DetalleEscenarioCupoExclusionFullInfo> exclusiones,
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

            // Crear el escenario de cupo con un ID nuevo
            var lastId = await _escenarioCupoRepository.GetLastIdAsync(cancellationToken) + 1;
            var escenarioCupoResult = EscenarioCupo.CreateOrUpdate(lastId, centroCosto, zona, nombre, esHoraInicio, minutosAntes, esActivo, detalles, exclusionesFecha, exclusiones);

            if (escenarioCupoResult.IsSuccess(out var escenarioCupo))
            {
                return escenarioCupo;
            }
        }

        return new Failure();
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
