using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Cupos.Validators;

/// <summary>
/// El validador de <see cref="CuposModel"/>
/// </summary>
public sealed class CuposModelValidator : BaseValidator<CuposModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="CuposModelValidator"/>.
    /// </summary>
    public CuposModelValidator()
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

        RuleFor(x => x.CanalesVenta)
            .NotEmpty();

        RuleFor(x => x.DiasSemana)
            .NotEmpty();

        RuleFor(x => x.HoraInicio)
            .NotNull()
            .LessThanOrEqualTo(x => x.HoraTermino);

        RuleFor(x => x.HoraTermino)
            .NotNull()
            .GreaterThanOrEqualTo(x => x.HoraInicio);

        RuleFor(x => x.IntervaloMinutos)
            .NotEmpty()
            .GreaterThanOrEqualTo(10);

        RuleFor(x => x.CanalVenta)
            .NotNull().WithMessage("Debe seleccionar al menos un canal de venta.");

        RuleFor(x => x.DiaSemana)
            .NotNull().WithMessage("Debe seleccionar al menos un día de semana.");

    }
}
