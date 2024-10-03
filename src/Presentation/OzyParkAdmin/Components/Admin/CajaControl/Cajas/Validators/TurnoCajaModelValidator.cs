using FluentValidation;
using OzyParkAdmin.Components.Admin.CajaControl.Cajas.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas.Validators;

/// <summary>
/// El validador de <see cref="TurnoCajaModel"/>.
/// </summary>
public sealed class TurnoCajaModelValidator : BaseValidator<TurnoCajaModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="TurnoCajaModelValidator"/>.
    /// </summary>
    public TurnoCajaModelValidator()
    {
        RuleFor(x => x.EfectivoCierreSupervisor)
            .NotNull();

        RuleFor(x => x.MontoTransbankSupervisor)
            .NotNull();

        RuleFor(x => x.Comentario)
            .NotEmpty();
    }
}
