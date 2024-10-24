using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Application.Reportes.Filters;

/// <summary>
/// El manejador de <see cref="LoadFilter"/>.
/// </summary>
public sealed class LoadFilterHandler : QueryListOfHandler<LoadFilter, ItemOption>
{
    private readonly IReportRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="LoadFilterHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IReportRepository"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public LoadFilterHandler(IReportRepository repository, ILogger<LoadFilterHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<List<ItemOption>> ExecuteListAsync(LoadFilter query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await _repository.LoadFilterAsync(query.ReportId, query.FilterId, query.Parameters, cancellationToken);
    }
}
