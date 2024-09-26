using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos.Validators;

/// <summary>
/// El validador de <see cref="ProductoParteModel"/>
/// </summary>
public class ProductoParteModelValidator : BaseValidator<ProductoParteModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="ProductoParteModelValidator"/>.
    /// </summary>
    public ProductoParteModelValidator()
    {
        RuleFor(x => x.Parte)
            .NotNull();

        RuleFor(x => x.Cantidad)
            .NotEmpty()
            .GreaterThan(0M);
    }
}
