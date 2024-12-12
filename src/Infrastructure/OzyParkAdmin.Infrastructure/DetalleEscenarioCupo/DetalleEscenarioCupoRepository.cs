using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Domain.DetallesEscenariosCupos;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.DetallesEscenariosCupos;

/// <summary>
/// El repositorio para manejar los detalles del escenario de cupo.
/// </summary>
public sealed class DetalleEscenarioCupoRepository(OzyParkAdminContext context) : Repository<DetalleEscenarioCupo>(context), IDetalleEscenarioCupoRepository
{
    /// <inheritdoc/>
    public async Task<List<DetalleEscenarioCupoInfo>> ListAsync(int escenarioCupoId, CancellationToken cancellationToken)
    {
        return await EntitySet
            .Where(x => x.EscenarioCupoId == escenarioCupoId)
            .Select(x => new DetalleEscenarioCupoInfo
            {
                EscenarioCupoId = x.EscenarioCupoId,
                HoraMaximaRevalidacion = x.HoraMaximaRevalidacion,
                HoraMaximaVenta = x.HoraMaximaVenta,
                ServicioId = x.ServicioId,
                TopeDiario = x.TopeDiario,
                TopeFlotante = x.TopeFlotante,
                UsaSobreCupo = x.UsaSobreCupo,
                UsaTopeEnCupo = x.UsaTopeEnCupo,
                Servicio = new ServicioInfo
                {
                    Aka = x.Servicio.Aka,
                    Id = x.ServicioId,
                    Nombre = x.Servicio.Nombre,
                }
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);


    }
    public async Task<IEnumerable<DetalleEscenarioCupo>> FindByIdsAsync(int escenarioCupoId, CancellationToken cancellationToken)
    {
        return await EntitySet
            .AsNoTracking()
            .Where(x => x.EscenarioCupoId == escenarioCupoId)
            .ToListAsync(cancellationToken);
    }
    /// <inheritdoc/>
    public async Task AddDetallesAsync(IEnumerable<DetalleEscenarioCupo> detalles, CancellationToken cancellationToken)
    {
        await EntitySet.AddRangeAsync(detalles, cancellationToken).ConfigureAwait(false);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
    /// <inheritdoc/>
    public async Task UpdateDetalleAsync(DetalleEscenarioCupo detalle, CancellationToken cancellationToken)
    {
        EntitySet.Update(detalle);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
    /// <inheritdoc/>
    public async Task RemoveDetallesAsync(IEnumerable<DetalleEscenarioCupo> detalles, CancellationToken cancellationToken)
    {
        EntitySet.RemoveRange(detalles);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
