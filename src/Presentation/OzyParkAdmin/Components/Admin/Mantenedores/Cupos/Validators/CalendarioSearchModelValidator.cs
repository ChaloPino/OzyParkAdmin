using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Validators;

/// <summary>
/// El validador de <see cref="CalendarioSearchModel"/>
/// </summary>
public sealed class CalendarioSearchModelValidator : BaseValidator<CalendarioSearchModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="CalendarioSearchModelValidator"/>.
    /// </summary>
    public CalendarioSearchModelValidator()
    {
        RuleFor(x => x.CentroCosto)
            .NotNull();

        RuleFor(x => x.CanalVenta)
            .NotNull();

        RuleFor(x => x.Alcance)
            .NotNull();

        RuleFor(x => x.Servicio)
            .NotNull();
    }
}
