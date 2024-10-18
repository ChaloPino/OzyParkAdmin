using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.ExclusionesCupo.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.ExclusionesCupo.Validators;

/// <summary>
/// El validador de <see cref="FechasExcluidasCupoModel"/>
/// </summary>
public sealed class FechasExcluidasCupoModelValidator : BaseValidator<FechasExcluidasCupoModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="FechasExcluidasCupoModelValidator"/>.
    /// </summary>
    public FechasExcluidasCupoModelValidator()
    {
        RuleFor(x => x.CentroCosto)
            .NotNull();

        RuleFor(x => x.CanalesVenta)
            .NotEmpty();

        RuleFor(x => x.RangoFechas.Start)
            .NotEmpty()
            .LessThanOrEqualTo(x => x.RangoFechas.End);

        RuleFor(x => x.RangoFechas.End)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.RangoFechas.Start);
    }
}
