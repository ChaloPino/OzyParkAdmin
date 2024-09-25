using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// El validador de <see cref="Producto"/>
/// </summary>
public sealed class ProductoValidator
{
    private readonly IProductoRepository _repository;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ProductoValidator"/>.
    /// </summary>
    /// <param name="repository">El <see cref="IProductoRepository"/>.</param>
    public ProductoValidator(IProductoRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <summary>
    /// Valida el aka del producto.
    /// </summary>
    /// <param name="productoId">El id del producto a validar.</param>
    /// <param name="franquiciaId">El id de la franquicia.</param>
    /// <param name="aka">El aka del producto.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El resultado de la validación del aka.</returns>
    public async Task<SuccessOrFailure> ValidateAkaAsync(int productoId, int franquiciaId, string? aka, CancellationToken cancellationToken)
    {
        bool exist = await _repository.ExistAkaAsync(productoId, franquiciaId, aka, cancellationToken);

        return exist
            ? new ValidationError(nameof(Producto.Aka), $"Ya existe un producto con el aka '{aka}'.")
            : new Success();
    }
}
