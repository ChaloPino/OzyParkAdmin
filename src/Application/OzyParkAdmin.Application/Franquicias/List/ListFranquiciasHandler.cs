using MassTransit.Mediator;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Domain.Franquicias;

namespace OzyParkAdmin.Application.Franquicias.List;

/// <summary>
/// El manejador de <see cref="ListFranquicias"/>.
/// </summary>
public sealed class ListFranquiciasHandler : MediatorRequestHandler<ListFranquicias, ResultListOf<Franquicia>>
{
    private readonly IFranquiciaRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ListFranquiciasHandler"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IFranquiciaRepository"/></param>
    public ListFranquiciasHandler(IFranquiciaRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <inheritdoc/>
    protected override async Task<ResultListOf<Franquicia>> Handle(ListFranquicias request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        int[]? franquiciaIds = request.User.GetFranquicias();
        return await _repository.ListFranquiciasAsync(franquiciaIds, cancellationToken);
    }
}
