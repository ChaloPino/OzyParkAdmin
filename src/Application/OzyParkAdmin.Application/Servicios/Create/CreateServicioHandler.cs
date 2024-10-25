using Microsoft.Extensions.Logging;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Servicios.Create;

/// <summary>
/// El manejador de <see cref="CreateServicio"/>.
/// </summary>
public sealed class CreateServicioHandler : ServicioStateChangeableHandler<CreateServicio>
{
    private readonly ServicioManager _servicioManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateServicioHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="servicioManager">El <see cref="ServicioManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CreateServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager, ILogger<CreateServicioHandler> logger)
        : base(context, logger, StateAction.Create)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteChangeStateAsync(CreateServicio command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        Context.AttachRange(command.TipoControl, command.TipoDistribucion, command.TipoVigencia);

        // Aplicar lógica de negocios.
        return await _servicioManager.CreateAsync(
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
}
