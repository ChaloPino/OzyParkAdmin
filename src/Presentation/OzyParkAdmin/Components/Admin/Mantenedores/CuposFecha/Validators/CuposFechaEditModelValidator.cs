using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Validators;

/// <summary>
/// El validador de <see cref="CuposFechaEditModel"/>
/// </summary>
public sealed class CuposFechaEditModelValidator : BaseValidator<CuposFechaEditModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="CuposFechaEditModelValidator"/>.
    /// </summary>
    public CuposFechaEditModelValidator()
    {
        RuleFor(x => x.FechaDate)
                   .NotEmpty();

        RuleFor(x => x.EscenarioCupo)
            .NotNull();

        RuleFor(x => x.Total)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.SobreCupo)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.TopeEnCupo)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.CanalVenta)
            .NotNull();

        RuleFor(x => x.DiaSemana)
            .NotNull();
    }
}
