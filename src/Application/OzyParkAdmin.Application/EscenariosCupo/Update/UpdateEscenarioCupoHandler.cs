using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application;
using OzyParkAdmin.Application.EscenariosCupo.Update;
using OzyParkAdmin.Application.Shared;
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

    public UpdateEscenarioCupoHandler(
        IOzyParkAdminContext context,
        ILogger<UpdateEscenarioCupoHandler> logger,
        IEscenarioCupoRepository escenarioCupoRepository,
        EscenarioCupoManager escenarioCupoManager)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(escenarioCupoRepository);
        ArgumentNullException.ThrowIfNull(escenarioCupoManager);

        _context = context;
        _escenarioCupoRepository = escenarioCupoRepository;
        _escenarioCupoManager = escenarioCupoManager;
    }

    protected override async Task<ResultOf<EscenarioCupoFullInfo>> ExecuteAsync(UpdateEscenarioCupo command, CancellationToken cancellationToken)
    {
        // Verificar que el escenario exista
        var escenarioCupo = await _escenarioCupoRepository.FindByIdAsync(command.Id, cancellationToken);
        if (escenarioCupo is null)
        {
            return new ValidationError(nameof(EscenarioCupo), $"El escenario con ID {command.Id} no existe.");
        }

        // Actualizar los datos básicos del escenario de cupo
        var updateEscenarioCupoResult = await _escenarioCupoManager.UpdateAsync(
            id: command.Id,
            existente: escenarioCupo,
            centroCostoInfo: command.CentroCosto,
            zonaInfo: command.ZonaInfo,
            nombre: command.Nombre,
            esHoraInicio: command.EsHoraInicio,
            minutosAntes: command.MinutosAntes,
            esActivo: command.EsActivo,
            cancellationToken: cancellationToken);

        if (updateEscenarioCupoResult.IsFailure(out var failure))
        {
            return failure;
        }

        return await updateEscenarioCupoResult.BindAsync(
            onSuccess: SaveAsync,
            onFailure: failure => failure,
            cancellationToken: cancellationToken);
    }

    private async Task<ResultOf<EscenarioCupoFullInfo>> SaveAsync(EscenarioCupo escenarioCupo, CancellationToken cancellationToken)
    {
        _context.Update(escenarioCupo);
        await _context.SaveChangesAsync(cancellationToken);
        return escenarioCupo.ToFullInfo();
    }
}
