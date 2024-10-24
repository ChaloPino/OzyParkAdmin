using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Reportes.Find;

/// <summary>
/// El manejador de <see cref="FindReporte"/>.
/// </summary>
public sealed class FindReporteHandler : BaseQueryHandler<FindReporte, Report>
{
    private readonly IReportRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="FindReporteHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IReportRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public FindReporteHandler(IReportRepository repository, ILogger<FindReporteHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(logger);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultOf<Report>> ExecuteAsync(FindReporte query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.FindReportByAkaAsync(query.Aka, query.User.GetRoles(), cancellationToken);
    }
}
