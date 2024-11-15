using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.EscenariosCupo;
using OzyParkAdmin.Domain.ExclusionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.EscenariosCupo.Create;

/// <summary>
/// El manejador de <see cref="CreateEscenarioCupo"/>.
/// </summary>
public sealed class CreateEscenarioCupoHandler : CommandHandler<CreateEscenarioCupo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly EscenarioCupoManager _manager;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CreateEscenarioCupoHandler"/>.
    /// </summary>
    /// <param name="context">El <see cref="IOzyParkAdminContext"/>.</param>
    /// <param name="manager">El <see cref="EscenarioCupoManager"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public CreateEscenarioCupoHandler(IOzyParkAdminContext context, EscenarioCupoManager manager, ILogger<CreateEscenarioCupoHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(manager);
        _context = context;
        _manager = manager;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(CreateEscenarioCupo command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        // Invocar el manager para crear el escenario de cupo junto con los detalles
        var result = await _manager.CreateEscenarioCupoAsync(
            command.CentroCosto,
            command.ZonaInfo,
            command.Nombre,
            command.EsHoraInicio,
            command.MinutosAntes,
            command.EsActivo,
            command.Detalles,
            cancellationToken);

        return await result.MatchAsync<SuccessOrFailure>(
                 async (escenarioCupo, cancellationToken) => await Task.FromResult(new Success()),
                 failure => failure,
                 cancellationToken);

    }

    /// <summary>
    /// Guarda los escenarios de cupo y sus detalles asociados en la base de datos.
    /// </summary>
    private async Task<SuccessOrFailure> SaveAsync(IEnumerable<EscenarioCupo> escenarioCupos, CancellationToken cancellationToken)
    {
        // Validar duplicados antes de iniciar la transacción
        foreach (var escenarioCupo in escenarioCupos)
        {
            if (escenarioCupo.DetallesEscenarioCupo != null)
            {
                var duplicados = escenarioCupo.DetallesEscenarioCupo
                    .GroupBy(d => new { d.EscenarioCupoId, d.ServicioId })
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicados.Any())
                {
                    return new ValidationError("DetallesEscenarioCupo",
                        "Se encontraron duplicados en los detalles de los escenarios.");
                }
            }
        }

        // Iniciar transacción
        await _context.BeginTransactionAsync(cancellationToken);
        try
        {
            var escenariosCupoList = escenarioCupos.ToList();

            // Extraer detalles de los escenarios
            var todosLosDetalles = escenariosCupoList
                .Where(e => e.DetallesEscenarioCupo != null)
                .SelectMany(e => e.DetallesEscenarioCupo)
                .ToList();

            // Vaciar los detalles antes de insertar los escenarios
            foreach (var escenario in escenariosCupoList)
            {
                escenario.DetallesEscenarioCupo.Clear();
            }

            // Insertar escenarios
            if (escenariosCupoList.Count <= 30)
            {
                await _context.AddRangeAsync(escenariosCupoList, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                await _context.BulkInsertAsync(escenariosCupoList, cancellationToken);
            }

            // Asignar nuevamente los IDs de los escenarios a los detalles
            foreach (var detalle in todosLosDetalles)
            {
                detalle.UpdateEscenarioId(detalle.EscenarioCupoId);
            }

            // Insertar detalles
            if (todosLosDetalles.Count > 30)
            {
                await _context.BulkInsertAsync(todosLosDetalles, cancellationToken);
            }
            else
            {
                await _context.AddRangeAsync(todosLosDetalles, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            // Confirmar transacción
            await _context.CommitTransactionAsync(cancellationToken);
            return new Success();
        }
        catch (Exception ex)
        {
            // Revertir transacción en caso de error
            await _context.RollbackTransactionAsync(cancellationToken);
            return new Failure();
        }
    }




}
