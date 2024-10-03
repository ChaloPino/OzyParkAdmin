using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Validators;

/// <summary>
/// El validador de <see cref="CupoViewModel"/>.
/// </summary>
public class CupoViewModelValidator : BaseValidator<CupoViewModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="CupoViewModelValidator"/>.
    /// </summary>
    public CupoViewModelValidator()
    {
        RuleFor(x => x.FechaEfectivaDate)
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
