using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Validators;

/// <summary>
/// El validador de <see cref="CupoFechaViewModel"/>
/// </summary>
public sealed class CupoFechaViewModelValidator : BaseValidator<CupoFechaViewModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="CupoFechaViewModelValidator"/>.
    /// </summary>
    public CupoFechaViewModelValidator()
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

        RuleFor(x => x.HoraInicioTime)
            .NotNull()
            .LessThanOrEqualTo(x => x.HoraFinTime);

        RuleFor(x => x.HoraFinTime)
            .NotNull()
            .GreaterThanOrEqualTo(x => x.HoraInicioTime);
    }
}
