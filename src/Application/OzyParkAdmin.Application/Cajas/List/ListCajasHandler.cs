using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Cajas;

namespace OzyParkAdmin.Application.Cajas.List;

/// <summary>
/// El manejador de <see cref="ListCajas"/>.
/// </summary>
public sealed class ListCajasHandler : QueryListOfHandler<ListCajas, CajaInfo>
{
    private readonly ICajaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListCajasHandler"/>.
    /// </summary>
    /// <param name="repostitory">El <see cref="ICajaRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListCajasHandler(ICajaRepository repostitory, ILogger<ListCajasHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repostitory);
        _repository = repostitory;
    }

    /// <inheritdoc/>
    protected override async Task<List<CajaInfo>> ExecuteListAsync(ListCajas query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.ListCajasAsync(query.User.GetCentrosCosto(), cancellationToken);
    }
}
