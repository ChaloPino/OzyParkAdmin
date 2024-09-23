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
    public UpdateServicioHandler(IOzyParkAdminContext context, ServicioManager servicioManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(servicioManager);
        _servicioManager = servicioManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Servicio>> ExecuteAsync(UpdateServicio request, CancellationToken cancellationToken) =>
        await _servicioManager.UpdateAsync(
            request.Id,
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
