using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Validate;

/// <summary>
/// El manejador de <see cref="ValidateProductoSku"/>.
/// </summary>
public sealed class ValidateProductoSkuHandler : CommandHandler<ValidateProductoSku>
{
    private readonly ProductoValidator _validator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ValidateProductoSkuHandler"/>.
    /// </summary>
    /// <param name="validator">El <see cref="ProductoValidator"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ValidateProductoSkuHandler(ProductoValidator validator, ILogger<ValidateProductoSkuHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(validator);
        _validator = validator;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(ValidateProductoSku command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return await _validator.ValidateSkuAsync(command.ProductoId, command.FranquiciaId, command.Sku, cancellationToken);
    }
}
