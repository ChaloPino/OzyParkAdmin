using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.ExclusionesCupo;

/// <summary>
/// La lógica de negocios de <see cref="EscenarioCupo"/>.
/// </summary>
public sealed class EscenarioCupoManager : IBusinessLogic
{
    private readonly ICentroCostoRepository _centroCostoRepository;
    private readonly IEscenarioCupoRepository _escenarioCupoRepository;
    private readonly IZonaRepository _zonaRepository;
    private readonly DetalleEscenarioCupoManager _detalleEscenarioCupoManager;

    public EscenarioCupoManager(
        IEscenarioCupoRepository escenarioCupoRepository,
        ICentroCostoRepository centroCostoRepository,
        IZonaRepository zonaRepository,
        DetalleEscenarioCupoManager detalleEscenarioCupoManager)
    {
        _centroCostoRepository = centroCostoRepository ?? throw new ArgumentNullException(nameof(centroCostoRepository));
        _escenarioCupoRepository = escenarioCupoRepository ?? throw new ArgumentNullException(nameof(escenarioCupoRepository));
        _zonaRepository = zonaRepository ?? throw new ArgumentNullException(nameof(zonaRepository));
        _detalleEscenarioCupoManager = detalleEscenarioCupoManager ?? throw new ArgumentNullException(nameof(detalleEscenarioCupoManager));
    }

    /// <summary>
    /// Crea un nuevo escenario de cupo con sus detalles.
    /// </summary>
    public async Task<ResultOf<EscenarioCupo>> CreateEscenarioCupoAsync(
        CentroCostoInfo centroCostoInfo,
        ZonaInfo? zonaInfo,
        string nombre,
        bool esHoraInicio,
        int minutosAntes,
        bool esActivo,
        IEnumerable<DetalleEscenarioCupoInfo> detalles,
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

            // Crear el escenario cupo
            var lastId = await _escenarioCupoRepository.GetLastIdAsync(cancellationToken) + 1;
            var escenarioCupoResult = EscenarioCupo.Create(lastId, centroCosto, zona, nombre, esHoraInicio, minutosAntes, esActivo, Enumerable.Empty<DetalleEscenarioCupoInfo>());

            if (escenarioCupoResult.IsSuccess(out var escenarioCupo))
            {
                try
                {
                    await _escenarioCupoRepository.AddAsync(escenarioCupo, cancellationToken);
                    await _escenarioCupoRepository.SaveChangesAsync(cancellationToken);

                    // Guardar detalles asociados
                    var saveDetailsResult = await SaveDetallesAsync(escenarioCupo, detalles, cancellationToken);
                    if (saveDetailsResult.IsFailure(out var detailFailure))
                    {
                        // Revertir si los detalles fallan
                        await _escenarioCupoRepository.RemoveAsync(escenarioCupo, cancellationToken);
                        await _escenarioCupoRepository.SaveChangesAsync(cancellationToken);
                        return detailFailure;
                    }

                    return escenarioCupo;
                }
                catch
                {
                    // En caso de error, devolver Failure genérico
                    return new Failure();
                }
            }
        }

        return new Failure(); // Caso genérico de error
    }

    /// <summary>
    /// Actualiza un escenario de cupo existente.
    /// </summary>
    /// <param name="id">ID del escenario de cupo a actualizar.</param>
    /// <param name="escenarioExistente">Instancia del escenario de cupo existente.</param>
    /// <param name="centroCostoInfo">Información del centro de costo.</param>
    /// <param name="zonaInfo">Información de la zona asociada.</param>
    /// <param name="nombre">Nuevo nombre del escenario.</param>
    /// <param name="esHoraInicio">Indica si el escenario usa hora de inicio.</param>
    /// <param name="minutosAntes">Cantidad de minutos antes permitidos.</param>
    /// <param name="esActivo">Estado del escenario (activo o inactivo).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El resultado de la actualización, incluyendo errores si aplica.</returns>
    public async Task<ResultOf<EscenarioCupo>> UpdateAsync(
        int id,
        EscenarioCupo escenarioExistente,
        CentroCostoInfo centroCostoInfo,
        ZonaInfo? zonaInfo,
        string nombre,
        bool esHoraInicio,
        int minutosAntes,
        bool esActivo,
        IEnumerable<DetalleEscenarioCupoInfo> nuevosDetalles,
        CancellationToken cancellationToken)
    {
        // Validar Centro de Costo y Zona
        var zonaResult = await ValidateCentroCostoAndZonaAsync(centroCostoInfo, zonaInfo, cancellationToken);
        if (zonaResult.IsFailure(out var failure))
        {
            return failure;
        }

        var (centroCosto, zona) = zonaResult.IsSuccess(out var entidades) ? entidades : default;

        // Actualizar las propiedades y los detalles del escenario
        var updateResult = await escenarioExistente.UpdateAsync(
            nombre: nombre,
            centroCosto: centroCosto,
            zona: zona,
            esHoraInicio: esHoraInicio,
            minutosAntes: minutosAntes,
            esActivo: esActivo,
            nuevosDetalles: nuevosDetalles,
            validateDuplicate: async (args, errors, ct) =>
            {
                var (updateId, updateCentroCosto, updateZona, updateNombre) = args;

                // Agrega validación de duplicados o cualquier lógica personalizada aquí
                if (await _escenarioCupoRepository.ExistsWithSimilarNameAsync(updateNombre, updateId, ct))
                {
                    errors.Add(new ValidationError(nameof(EscenarioCupo.Nombre), $"Ya existe un escenario con el nombre '{updateNombre}'."));
                }
            },
            cancellationToken: cancellationToken);

        if (updateResult.IsFailure(out var updateFailure))
        {
            return updateFailure;
        }

        try
        {
            // Guardar los cambios en el repositorio
            await _escenarioCupoRepository.UpdateAsync(escenarioExistente, cancellationToken);
            await _escenarioCupoRepository.SaveChangesAsync(cancellationToken);

            return updateResult;
        }
        catch (Exception ex)
        {
            return new Failure();
        }
    }



    private async Task<SuccessOrFailure> SaveDetallesAsync(
        EscenarioCupo escenarioCupo,
        IEnumerable<DetalleEscenarioCupoInfo> detalles,
        CancellationToken cancellationToken)
    {
        if (!detalles.Any())
        {
            return new Success();
        }

        // Validar duplicados
        var duplicados = detalles
            .GroupBy(d => new { d.ServicioId, d.HoraMaximaVenta, d.HoraMaximaRevalidacion })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicados.Any())
        {
            return new ValidationError("DetallesEscenarioCupo", "Existen duplicados en los detalles del escenario.");
        }

        // Crear detalles utilizando el método en DetalleEscenarioCupoManager
        var createResult = await _detalleEscenarioCupoManager.CreateDetallesAsync(
            escenarioCupo.Id,
            detalles,
            cancellationToken);

        return createResult.IsSuccess(out _) ? new Success() : new Failure();
    }

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
