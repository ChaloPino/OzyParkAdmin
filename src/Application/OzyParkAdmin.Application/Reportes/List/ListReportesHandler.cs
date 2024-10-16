using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Reportes;

namespace OzyParkAdmin.Application.Reportes.List;

/// <summary>
/// El manejador de <see cref="ListReportes"/>.
/// </summary>
public sealed class ListReportesHandler : MediatorRequestHandler<ListReportes, ResultListOf<ReportGroupInfo>>
{
    private readonly IReportRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListReportesHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IReportRepository"/>.</param>
    public ListReportesHandler(IReportRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<ReportGroupInfo>> Handle(ListReportes request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.FindReportGroupsAsync(request.User.GetRoles(), cancellationToken);
    }
}
