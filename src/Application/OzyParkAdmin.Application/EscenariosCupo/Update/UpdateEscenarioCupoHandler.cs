using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application;
using OzyParkAdmin.Application.EscenariosCupo.Update;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.ExclusionesCupo;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.EscenariosCupo;

/// <summary>
/// Handler encargado de la actualización de un escenario de cupo existente.
/// </summary>
public sealed class UpdateEscenarioCupoHandler : CommandHandler<UpdateEscenarioCupo, EscenarioCupoFullInfo>
{
    private readonly IOzyParkAdminContext _context;
    private readonly IEscenarioCupoRepository _escenarioCupoRepository;
    private readonly EscenarioCupoManager _escenarioCupoManager;
    private readonly DetalleEscenarioCupoManager _detalleEscenarioCupoManager;

    public UpdateEscenarioCupoHandler(
        IOzyParkAdminContext context,
        ILogger<UpdateEscenarioCupoHandler> logger,
        IEscenarioCupoRepository escenarioCupoRepository,
        EscenarioCupoManager escenarioCupoManager,
        DetalleEscenarioCupoManager detalleEscenarioCupoManager)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(escenarioCupoRepository);
        ArgumentNullException.ThrowIfNull(escenarioCupoManager);
        ArgumentNullException.ThrowIfNull(detalleEscenarioCupoManager);

        _context = context;
        _escenarioCupoRepository = escenarioCupoRepository;
        _escenarioCupoManager = escenarioCupoManager;
        _detalleEscenarioCupoManager = detalleEscenarioCupoManager;
    }

    protected override async Task<ResultOf<EscenarioCupoFullInfo>> ExecuteAsync(UpdateEscenarioCupo command, CancellationToken cancellationToken)
    {
        // Verificar que el escenario exista
        var escenarioCupo = await _escenarioCupoRepository.FindByIdAsync(command.Id, cancellationToken);
        if (escenarioCupo is null)
        {
            return new ValidationError(nameof(EscenarioCupo), $"El escenario con ID {command.Id} no existe.");
        }

        // Actualizar el escenario usando el EscenarioCupoManager
        var updateEscenarioCupoResult = await _escenarioCupoManager.UpdateAsync(
            id: command.Id,
            escenarioExistente: escenarioCupo,
            centroCostoInfo: command.CentroCosto,
            zonaInfo: command.ZonaInfo,
            nombre: command.Nombre,
            esHoraInicio: command.EsHoraInicio,
            minutosAntes: command.MinutosAntes,
            esActivo: command.EsActivo,
            nuevosDetalles: command.Detalles,
            cancellationToken: cancellationToken);

        if (updateEscenarioCupoResult.IsFailure(out var failure))
        {
            return failure;
        }

        var updateDetallesEscenarioCupoResult = await _detalleEscenarioCupoManager.UpdateDetallesAsync(
            command.Id,
            command.Detalles,
            cancellationToken);

        if (updateDetallesEscenarioCupoResult.IsFailure(out var detailFailure))
        {
            return detailFailure;
        }

        await updateDetallesEscenarioCupoResult.BindAsync(
           onSuccess: SaveDetailsAsync,
           onFailure: failure => failure,
           cancellationToken: cancellationToken);


        return await updateEscenarioCupoResult.BindAsync(
         onSuccess: SaveAsync,
         onFailure: failure => failure,
         cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<IEnumerable<DetalleEscenarioCupoInfo>>> SaveDetailsAsync(IEnumerable<DetalleEscenarioCupo> detallesEscenarioCupo, CancellationToken cancellationToken)
    {
        _context.UpdateRange(detallesEscenarioCupo);
        await _context.SaveChangesAsync(cancellationToken);
        return detallesEscenarioCupo.Select(x => x.ToInfo()).ToList();
    }

    private async Task<ResultOf<EscenarioCupoFullInfo>> SaveAsync(EscenarioCupo escenarioCUpo, CancellationToken cancellationToken)
    {
        _context.Update(escenarioCUpo);
        await _context.SaveChangesAsync(cancellationToken);
        return escenarioCUpo.ToFullInfo();
    }
}
