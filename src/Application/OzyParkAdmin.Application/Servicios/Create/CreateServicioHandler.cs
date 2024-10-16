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
    public CreateServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager)
        : base(context, StateAction.Create)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteAsync(CreateServicio request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Context.AttachRange(request.TipoControl, request.TipoDistribucion, request.TipoVigencia);

        // Aplicar lógica de negocios.
        return await _servicioManager.CreateAsync(
            request.CentroCosto,
            request.FranquiciaId,
            request.Aka,
            request.Nombre,
            request.TipoControl,
            request.TipoDistribucion,
            request.TipoServicio,
            request.TipoVigencia,
            request.NumeroVigencia,
            request.NumeroValidez,
            request.NumeroPaxMinimo,
            request.NumeroPaxMaximo,
            request.EsConHora,
            request.EsPorTramos,
            request.EsParaVenta,
            request.Orden,
            request.HolguraInicio,
            request.HolguraFin,
            request.EsParaMovil,
            request.MostrarTramos,
            request.EsParaBuses,
            request.IdaVuelta,
            request.HolguraEntrada,
            request.ControlParental,
            request.ServicioResponsableId,
            request.Politicas,
            request.PlantillaId,
            request.PlantillaDigitalId,
            cancellationToken);
    }
}
