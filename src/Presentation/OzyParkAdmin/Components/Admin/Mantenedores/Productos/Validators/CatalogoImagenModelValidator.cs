using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.Productos.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Productos.Validators;

/// <summary>
/// El validador de <see cref="CatalogoImagenModel"/>.
/// </summary>
public class CatalogoImagenModelValidator : BaseValidator<CatalogoImagenModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="CatalogoImagenModelValidator"/>.
    /// </summary>
    public CatalogoImagenModelValidator()
    {
        RuleFor(x => x.Aka)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Base64)
            .NotEmpty();

        RuleFor(x => x.MimeType)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Tipo)
            .NotEmpty()
            .MaximumLength(20);
    }
}