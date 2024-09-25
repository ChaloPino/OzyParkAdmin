using FluentValidation;
using MassTransit;
using MassTransit.Mediator;
using OzyParkAdmin.Application.Productos.Validate;
using OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos.Validators;

/// <summary>
/// El validador de <see cref="ProductoViewModel"/>
/// </summary>
public sealed class ProductoViewModelValidator : BaseValidator<ProductoViewModel>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ProductoViewModelValidator"/>.
    /// </summary>
    /// <param name="mediator"></param>
    public ProductoViewModelValidator(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;

        RuleFor(x => x.Sku)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Aka)
            .NotEmpty()
            .MaximumLength(50)
            .CustomAsync(ValidateDuplicateAsync);

        RuleFor(x => x.FranquiciaId)
            .NotEmpty();

        RuleFor(x => x.CentroCosto)
            .NotNull();

        RuleFor(x => x.Categoria)
            .NotNull();

        RuleFor(x => x.CategoriaDespliegue)
            .NotNull();

        RuleFor(x => x.TipoProducto)
            .NotNull();

        RuleFor(x => x.Familia)
            .NotNull();

        RuleFor(x => x.Imagen)
            .NotNull()
            .SetValidator(new CatalogoImagenModelValidator());

        RuleFor(x => x.Orden)
            .InclusiveBetween(1, int.MaxValue);

    }

    private async Task ValidateDuplicateAsync(string aka, ValidationContext<ProductoViewModel> context, CancellationToken cancellationToken)
    {
        var validateAka = new ValidateProductoAka(context.InstanceToValidate.Id, context.InstanceToValidate.FranquiciaId, aka);
        SuccessOrFailure result = await _mediator.SendRequest(validateAka, cancellationToken);

        result.Switch(
            onSuccess: _ => { },
            onFailure: failure => context.AddFailure(failure));
    }
}
