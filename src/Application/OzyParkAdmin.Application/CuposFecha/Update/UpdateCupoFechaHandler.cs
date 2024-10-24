using Microsoft.Extensions.Logging;
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
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public UpdateCupoFechaHandler(IOzyParkAdminContext context, CupoFechaManager cupoFechaManager, ILogger<UpdateCupoFechaHandler> logger)
        : base(context, logger)
    {
        ArgumentNullException.ThrowIfNull(cupoFechaManager);
        _cupoFechaManager = cupoFechaManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<CupoFecha>> ExecuteChangeStateAsync(UpdateCupoFecha command, CancellationToken cancellationToken)
    {
        Context.AttachRange(command.CanalVenta, command.DiaSemana);
        return await _cupoFechaManager.UpdateCupoFechaAsync(
            command.Id,
            command.Fecha,
            command.EscenarioCupo,
            command.CanalVenta,
            command.DiaSemana,
            command.HoraInicio,
            command.HoraFin,
            command.Total,
            command.SobreCupo,
            command.TopeEnCupo,
            cancellationToken);
    }
}
