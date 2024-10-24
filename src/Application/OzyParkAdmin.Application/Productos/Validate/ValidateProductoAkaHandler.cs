using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Productos.Validate;

/// <summary>
/// Manejador de <see cref="ValidateProductoAka"/>.
/// </summary>
public sealed class ValidateProductoAkaHandler : CommandHandler<ValidateProductoAka>
{
    private readonly ProductoValidator _validator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ValidateProductoAkaHandler"/>.
    /// </summary>
    /// <param name="validator">El <see cref="ProductoValidator"/>.</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ValidateProductoAkaHandler(ProductoValidator validator, ILogger<ValidateProductoAkaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(validator);
        _validator = validator;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(ValidateProductoAka command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return await _validator.ValidateAkaAsync(command.ProductoId, command.FranquiciaId, command.Aka, cancellationToken);
    }
}
