using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Validators;

/// <summary>
/// El validador de <see cref="EscenarioCupoModel"/>.
/// </summary>
public class EscenarioCupoValidator : BaseValidator<EscenarioCupoModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="EscenarioCupoValidator"/>.
    /// </summary>
    public EscenarioCupoValidator()
    {
        RuleFor(x => x.CentroCosto)
           .NotNull();

        RuleFor(x => x.Nombre)
            .MinimumLength(5)
            .MaximumLength(50)
              .NotNull();

        RuleFor(x => x.MinutosAntes)
            .GreaterThanOrEqualTo(0);

    }
}
