using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Reportes;

namespace OzyParkAdmin.Application.Reportes.List;

/// <summary>
/// El manejador de <see cref="ListReportes"/>.
/// </summary>
public sealed class ListReportesHandler : QueryListOfHandler<ListReportes, ReportGroupInfo>
{
    private readonly IReportRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListReportesHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IReportRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ListReportesHandler(IReportRepository repository, ILogger<ListReportesHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<ReportGroupInfo>> ExecuteListAsync(ListReportes query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.FindReportGroupsAsync(query.User.GetRoles(), cancellationToken);
    }
}
