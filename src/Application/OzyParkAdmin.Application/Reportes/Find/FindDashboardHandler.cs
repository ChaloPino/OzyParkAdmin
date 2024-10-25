using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Charts;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Reportes.Find;

/// <summary>
/// El manejador de <see cref="FindDashboard"/>
/// </summary>
public sealed class FindDashboardHandler : BaseQueryHandler<FindDashboard, ChartReport>
{
    private readonly IReportRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="FindDashboardHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IReportRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public FindDashboardHandler(IReportRepository repository, ILogger<FindDashboardHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<ChartReport>> ExecuteAsync(FindDashboard query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.FindDashboardAsync(query.User, cancellationToken).ConfigureAwait(false);
    }
}
