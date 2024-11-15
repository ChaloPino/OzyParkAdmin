using FluentValidation;
using MassTransit;
using MassTransit.Mediator;
using OzyParkAdmin.Application.CategoriasProducto.Validate;
using OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Models;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CategoriasProducto.Validators;

/// <summary>
/// El Validador del formulario para <see cref="CategoriaProductoViewModel"/>
/// </summary>
public sealed class CategoriaProductoViewModelValidator : BaseValidator<CategoriaProductoViewModel>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="CategoriaProductoViewModelValidator"/>.
    /// </summary>
    /// <param name="mediator"></param>
    public CategoriaProductoViewModelValidator(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;

        RuleFor(x => x.FranquiciaId)
            .NotEmpty();

        RuleFor(x => x.Aka)
            .NotEmpty()
            .MaximumLength(50)
            .CustomAsync(ValidateDuplicateAkaAsync);

        RuleFor(x => x.Padre)
            .NotNull();

        RuleFor(x => x.Orden)
            .InclusiveBetween(1, int.MaxValue);

        RuleFor(x => x.Orden)
            .InclusiveBetween(1, Int16.MaxValue);
    }

    private async Task ValidateDuplicateAkaAsync(string aka, ValidationContext<CategoriaProductoViewModel> context, CancellationToken cancellationToken)
    {
        var validateAka = new ValidateCategoriaProductoAka(context.InstanceToValidate.Id, context.InstanceToValidate.FranquiciaId, aka);
        SuccessOrFailure result = await _mediator.SendRequest(validateAka, cancellationToken);

        result.Switch(
            onSuccess: _ => { },
            onFailure: failure => context.AddFailure(failure));
    }

}
