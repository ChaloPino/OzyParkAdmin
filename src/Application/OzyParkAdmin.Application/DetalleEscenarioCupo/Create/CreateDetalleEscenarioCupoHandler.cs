﻿using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.DetalleEscenarioCupo.Create;

/// <summary>
/// Handler encargado de la creación de detalles para un escenario de cupo.
/// </summary>
public sealed class CreateDetalleEscenarioCupoHandler : CommandHandler<CreateDetalleEscenarioCupo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly DetalleEscenarioCupoManager _detalleCupoManager;

    /// <summary>
    /// Constructor de la clase <see cref="CreateDetalleEscenarioCupoHandler"/>.
    /// </summary>
    /// <param name="context">Contexto de datos de la aplicación.</param>
    /// <param name="logger">Logger de la clase.</param>
    /// <param name="detalleCupoManager">Manager de detalles del escenario de cupo.</param>
    public CreateDetalleEscenarioCupoHandler(
        IOzyParkAdminContext context,
        ILogger<CreateDetalleEscenarioCupoHandler> logger,
        DetalleEscenarioCupoManager detalleCupoManager)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(detalleCupoManager);

        _context = context;
        _detalleCupoManager = detalleCupoManager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(CreateDetalleEscenarioCupo command, CancellationToken cancellationToken)
    {
        var result = await _detalleCupoManager.SyncDetallesAsync(command.EscenarioCupoId, command.Detalles, cancellationToken);

        var (nuevas, actualizar, eliminar) = result;

        if (eliminar.Any())
        {
            if (eliminar.Count() > 30)
            {
                await _context.BulkDeleteAsync(eliminar, cancellationToken);
            }
            else
            {
                _context.RemoveRange(eliminar);
            }
        }

        if (actualizar.Any())
        {
            _context.UpdateRange(actualizar);
        }

        if (nuevas.Any())
        {
            if (nuevas.Count() > 30)
            {
                await _context.BulkInsertAsync(nuevas, cancellationToken);
            }
            else
            {
                await _context.AddRangeAsync(nuevas, cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}
