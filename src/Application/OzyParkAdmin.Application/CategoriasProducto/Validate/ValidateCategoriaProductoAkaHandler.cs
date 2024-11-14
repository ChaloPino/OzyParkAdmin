using Microsoft.Extensions.Logging;
using OzyParkAdmin.Application.CategoriasProducto.Search;
using OzyParkAdmin.Application.Productos.Validate;
using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.CategoriasProducto;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace OzyParkAdmin.Application.CategoriasProducto.Validate;

/// <summary>
/// El Manejador de la validacion del Aka <see cref="ValidateCategoriaProductoAka"/>
/// </summary>
public sealed class ValidateCategoriaProductoAkaHandler : CommandHandler<ValidateCategoriaProductoAka>
{
    private readonly ICategoriaProductoRepository _categoriaProductoRepository;


    /// <summary>
    /// Crea una nueva instacia de <see cref="ValidateCategoriaProductoAkaHandler"/>
    /// </summary>
    /// <param name="categoriaProductoRepository">El repositorio de la Categoria de Productos</param>
    /// <param name="logger">El <see cref="ILogger{TCategoryName}"/>.</param>
    public ValidateCategoriaProductoAkaHandler(ICategoriaProductoRepository categoriaProductoRepository, ILogger<ValidateCategoriaProductoAkaHandler> logger)
        : base(logger)
    {
        ArgumentNullException.ThrowIfNull(categoriaProductoRepository);
        _categoriaProductoRepository = categoriaProductoRepository;
    }

    /// <inheritdoc/>
    protected override async Task<SuccessOrFailure> ExecuteAsync(ValidateCategoriaProductoAka command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        bool exist = await _categoriaProductoRepository.ExistAkaAsync(command.CategoriaProductoId, command.FranquiciaId, command.Aka, cancellationToken);
        return exist
            ? new ValidationError(nameof(Producto.Aka), $"Ya existe una categoria de producto con el aka '{command.Aka}'.")
            : new Success();
    }
}
