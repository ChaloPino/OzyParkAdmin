using MassTransit.Mediator;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Charts;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Reportes.Find;

/// <summary>
/// El manejador de <see cref="FindDashboard"/>
/// </summary>
public sealed class FindDashboardHandler : MediatorRequestHandler<FindDashboard, ResultOf<ChartReport>>
{
    private readonly IReportRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="FindDashboardHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IReportRepository"/>.</param>
    public FindDashboardHandler(IReportRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<ChartReport>> Handle(FindDashboard request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.FindDashboardAsync(request.User, cancellationToken).ConfigureAwait(false);
    }
}
