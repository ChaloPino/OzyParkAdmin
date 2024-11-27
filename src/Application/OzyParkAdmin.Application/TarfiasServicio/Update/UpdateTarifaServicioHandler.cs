using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.TarifasServicio;

namespace OzyParkAdmin.Application.TarfiasServicio.Update;

/// <summary>
/// El manejador de <see cref="UpdateTarifaServicio"/>
/// </summary>
public sealed class UpdateTarifaServicioHandler : CommandHandler<UpdateTarifaServicio, TarifaServicioFullInfo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly TarifaServicioManager _manager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="IOzyParkAdminContext"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="manager">El <see cref="TarifaServicioManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public UpdateTarifaServicioHandler(IOzyParkAdminContext context, TarifaServicioManager manager, ILogger<UpdateTarifaServicioHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(manager);
        _context = context;
        _manager = manager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<TarifaServicioFullInfo>> ExecuteAsync(UpdateTarifaServicio command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        _context.AttachRange(
            command.Moneda,
            command.CanalVenta,
            command.TipoDia,
            command.TipoHorario,
            command.TipoSegmentacion);

        var result = await _manager.UpdateAsync(
            command.InicioVigencia,
            command.Moneda,
            command.Servicio,
            command.Tramo,
            command.GrupoEtario,
            command.CanalVenta,
            command.TipoDia,
            command.TipoHorario,
            command.TipoSegmentacion,
            command.ValorAfecto,
            command.ValorExento,
            cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<TarifaServicioFullInfo>> SaveAsync(TarifaServicio tarifaServicio, CancellationToken cancellationToken)
    {
        _context.Update(tarifaServicio);
        await _context.SaveChangesAsync(cancellationToken);
        return tarifaServicio.ToFullInfo();
    }
}
