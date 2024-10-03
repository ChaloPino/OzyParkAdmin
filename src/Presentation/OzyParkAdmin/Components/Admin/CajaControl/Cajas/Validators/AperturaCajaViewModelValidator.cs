using FluentValidation;
using OzyParkAdmin.Components.Admin.CajaControl.Cajas.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.CajaControl.Cajas.Validators;

/// <summary>
/// El validador de <see cref="AperturaCajaViewModel"/>
/// </summary>
public sealed class AperturaCajaViewModelValidator : BaseValidator<AperturaCajaViewModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="AperturaCajaViewModelValidator"/>.
    /// </summary>
    public AperturaCajaViewModelValidator()
    {
        RuleFor(x => x.Comentario)
            .NotEmpty();
    }
}
