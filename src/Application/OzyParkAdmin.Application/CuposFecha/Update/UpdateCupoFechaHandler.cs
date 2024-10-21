using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.CuposFecha.Update;

/// <summary>
/// El manejador de <see cref="UpdateCupoFecha"/>.
/// </summary>
public sealed class UpdateCupoFechaHandler : CupoFechaStateChangeableHandler<UpdateCupoFecha>
{
    private readonly CupoFechaManager _cupoFechaManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UpdateCupoFechaHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cupoFechaManager">El <see cref="CupoFechaManager"/>.</param>
    public UpdateCupoFechaHandler(IOzyParkAdminContext context, CupoFechaManager cupoFechaManager)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(cupoFechaManager);
        _cupoFechaManager = cupoFechaManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<CupoFecha>> ExecuteAsync(UpdateCupoFecha request, CancellationToken cancellationToken)
    {
        Context.AttachRange(request.CanalVenta, request.DiaSemana);
        return await _cupoFechaManager.UpdateCupoFechaAsync(
            request.Id,
            request.Fecha,
            request.EscenarioCupo,
            request.CanalVenta,
            request.DiaSemana,
            request.HoraInicio,
            request.HoraFin,
            request.Total,
            request.SobreCupo,
            request.TopeEnCupo,
            cancellationToken);
    }
}
