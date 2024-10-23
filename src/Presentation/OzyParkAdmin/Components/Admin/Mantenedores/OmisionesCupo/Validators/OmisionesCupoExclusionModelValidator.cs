using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.OmisionesCupo.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.OmisionesCupo.Validators;

/// <summary>
/// El validador de <see cref="OmisionesCupoExclusionModel"/>
/// </summary>
public sealed class OmisionesCupoExclusionModelValidator : BaseValidator<OmisionesCupoExclusionModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="OmisionesCupoExclusionModelValidator"/>.
    /// </summary>
    public OmisionesCupoExclusionModelValidator()
    {
        RuleFor(x => x.EscenariosCupo)
            .NotEmpty();

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
