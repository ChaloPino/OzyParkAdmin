using MassTransit.Mediator;
using OzyParkAdmin.Domain.CuposFecha;
using OzyParkAdmin.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Application.CuposFecha.Update;

/// <summary>
/// El manejador de <see cref="UpdateCuposFecha"/>
/// </summary>
public sealed class UpdateCuposFechaHandler : MediatorRequestHandler<UpdateCuposFecha, SuccessOrFailure>
{
    private readonly IOzyParkAdminContext _context;
    private readonly CupoFechaManager _cupoFechaManager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="UpdateCupoFechaHandler"/>
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="cupoFechaManager">El <see cref="CupoFechaManager"/>.</param>
    public UpdateCuposFechaHandler(IOzyParkAdminContext context, CupoFechaManager cupoFechaManager)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cupoFechaManager);
        _context = context;
        _cupoFechaManager = cupoFechaManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> Handle(UpdateCuposFecha request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _context.AttachRange(request.CanalVenta, request.DiaSemana);

        ResultOf<IEnumerable<CupoFecha>> result = await _cupoFechaManager.UpdateCuposFechaAsync(
            request.Fecha,
            request.EscenarioCupo,
            request.CanalVenta,
            request.DiaSemana,
            request.Total,
            request.Sobrecupo,
            request.TopeEnCupo,
            cancellationToken);

        return await result.MatchAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<SuccessOrFailure> SaveAsync(IEnumerable<CupoFecha> cuposFecha, CancellationToken cancellationToken)
    {
        if (cuposFecha.Count() < 30)
        {
            _context.UpdateRange(cuposFecha);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _context.BulkUpdateAsync(cuposFecha, cancellationToken);
        }

        return new Success();
    }
}
