using MassTransit.Mediator;
using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Domain.Reportes.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Application.Reportes.Filters;

/// <summary>
/// El manejador de <see cref="LoadFilter"/>.
/// </summary>
public sealed class LoadFilterHandler : MediatorRequestHandler<LoadFilter, ResultListOf<ItemOption>>
{
    private readonly IReportRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="LoadFilterHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IReportRepository"/>.</param>
    public LoadFilterHandler(IReportRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<ItemOption>> Handle(LoadFilter request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.LoadFilterAsync(request.ReportId, request.FilterId, request.Parameters, cancellationToken);
    }
}
