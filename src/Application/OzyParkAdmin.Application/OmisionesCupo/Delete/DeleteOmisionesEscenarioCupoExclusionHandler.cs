using MassTransit.Mediator;
using Microsoft.Win32.SafeHandles;
using OzyParkAdmin.Domain.OmisionesCupo;
using OzyParkAdmin.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Application.OmisionesCupo.Delete;

/// <summary>
/// El manejador de <see cref="DeleteOmisionesEscenarioCupoExclusion"/>
/// </summary>
public sealed class DeleteOmisionesEscenarioCupoExclusionHandler : MediatorRequestHandler<DeleteOmisionesEscenarioCupoExclusion, SuccessOrFailure>
{
    private readonly IOzyParkAdminContext _context;
    private readonly IIgnoraEscenarioCupoExclusionRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DeleteOmisionesEscenarioCupoExclusionHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="repository">El <see cref="IIgnoraEscenarioCupoExclusionRepository"/>.</param>
    public DeleteOmisionesEscenarioCupoExclusionHandler(IOzyParkAdminContext context, IIgnoraEscenarioCupoExclusionRepository repository)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(repository);
        _context = context;
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> Handle(DeleteOmisionesEscenarioCupoExclusion request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var keys = request.Omisiones.Select(x => (x.EscenarioCupo.Id, x.CanalVenta.Id, x.FechaIgnorada));
        var omisiones = await _repository.FindByKeysAsync(keys, cancellationToken);

        if (omisiones.Count() < 30)
        {
            _context.RemoveRange(omisiones);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await _context.BulkDeleteAsync(omisiones, cancellationToken);
        }

        return new Success();
    }
}
