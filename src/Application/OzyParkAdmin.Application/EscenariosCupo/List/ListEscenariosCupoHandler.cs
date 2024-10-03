using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.EscenariosCupo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Application.EscenariosCupo.List;

/// <summary>
/// El manejador de <see cref="ListEscenariosCupo"/>.
/// </summary>
public sealed class ListEscenariosCupoHandler : MediatorRequestHandler<ListEscenariosCupo, ResultListOf<EscenarioCupoInfo>>
{
    private readonly IEscenarioCupoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListEscenariosCupoHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IEscenarioCupoRepository"/>.</param>
    public ListEscenariosCupoHandler(IEscenarioCupoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<EscenarioCupoInfo>> Handle(ListEscenariosCupo request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _repository.ListAsync(request.User.GetCentrosCosto(), cancellationToken);
    }
}
