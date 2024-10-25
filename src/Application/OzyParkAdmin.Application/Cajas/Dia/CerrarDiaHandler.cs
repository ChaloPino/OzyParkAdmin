using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Cajas.Dia;

/// <summary>
/// El manejador de <see cref="CerrarDia"/>.
/// </summary>
public sealed class CerrarDiaHandler : CommandHandler<CerrarDia, AperturaCajaInfo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CajaManager _cajaManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CerrarDiaHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cajaManager">El <see cref="CajaManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CerrarDiaHandler(IOzyParkAdminContext context, CajaManager cajaManager, ILogger<CerrarDiaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cajaManager);
        _context = context;
        _cajaManager = cajaManager;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<AperturaCajaInfo>> ExecuteAsync(CerrarDia command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ResultOf<AperturaDia> result = await _cajaManager.CerrarDiaAsync(
            command.DiaId,
            command.User.ToInfo(),
            command.Comentario,
            command.MontoEfectivoParaCierre,
            command.MontoTransbankParaCierre,
            cancellationToken);

        return await result.BindAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<AperturaCajaInfo>> SaveAsync(AperturaDia aperturaDia, CancellationToken cancellationToken)
    {
        _context.Update(aperturaDia);
        await _context.SaveChangesAsync(cancellationToken);
        return aperturaDia.ToInfo();
    }
}
