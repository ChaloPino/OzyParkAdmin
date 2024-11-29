using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio.Validators;

/// <summary>
/// El validador de <see cref="TarifaServicioViewModel"/>
/// </summary>
public sealed class TarifaServicioViewModelValidator : BaseValidator<TarifaServicioViewModel>
{
    /// <summary>
    /// El validador de <see cref="TarifaServicioViewModel"/>.
    /// </summary>
    public TarifaServicioViewModelValidator()
    {
        RuleFor(x => x.ValorAfecto)
            .GreaterThan(-1);

        RuleFor(x => x.ValorExento)
            .GreaterThan(-1);
    }
}
