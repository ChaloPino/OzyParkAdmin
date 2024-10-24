using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Update;

/// <summary>
/// El manejador de <see cref="UpdateServicio"/>.
/// </summary>
public sealed class UpdateServicioHandler : ServicioStateChangeableHandler<UpdateServicio>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UpdateServicioHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public UpdateServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager, ILogger<UpdateServicioHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteChangeStateAsync(UpdateServicio command, CancellationToken cancellationToken) =>
        await _servicioManager.UpdateAsync(
            command.Id,
            command.CentroCosto,
            command.FranquiciaId,
            command.Aka,
            command.Nombre,
            command.TipoControl,
            command.TipoDistribucion,
            command.TipoServicio,
            command.TipoVigencia,
            command.NumeroVigencia,
            command.NumeroValidez,
            command.NumeroPaxMinimo,
            command.NumeroPaxMaximo,
            command.EsConHora,
            command.EsPorTramos,
            command.EsParaVenta,
            command.Orden,
            command.HolguraInicio,
            command.HolguraFin,
            command.EsParaMovil,
            command.MostrarTramos,
            command.EsParaBuses,
            command.IdaVuelta,
            command.HolguraEntrada,
            command.ControlParental,
            command.ServicioResponsableId,
            command.Politicas,
            command.PlantillaId,
            command.PlantillaDigitalId,
            cancellationToken);
}
