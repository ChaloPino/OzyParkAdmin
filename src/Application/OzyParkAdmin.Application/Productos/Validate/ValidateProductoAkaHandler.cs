using MassTransit.Mediator;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Validate;

/// <summary>
/// Manejador de <see cref="ValidateProductoAka"/>.
/// </summary>
public sealed class ValidateProductoAkaHandler : MediatorRequestHandler<ValidateProductoAka, SuccessOrFailure>
{
    private readonly ProductoValidator _validator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ValidateProductoAkaHandler"/>.
    /// </summary>
    /// <param name="validator">El <see cref="ProductoValidator"/>.</param>
    public ValidateProductoAkaHandler(ProductoValidator validator)
    {
        ArgumentNullException.ThrowIfNull(validator);
        _validator = validator;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> Handle(ValidateProductoAka request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _validator.ValidateAkaAsync(request.ProductoId, request.FranquiciaId, request.Aka, cancellationToken);
    }
}
