using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.TarifasProducto.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasProducto.Validators;

/// <summary>
/// El validador de <see cref="TarifaProductoViewModel"/>
/// </summary>
public sealed class TarifaProductoViewModelValidator : BaseValidator<TarifaProductoViewModel>
{
    /// <summary>
    /// El validador de <see cref="TarifaProductoViewModel"/>.
    /// </summary>
    public TarifaProductoViewModelValidator()
    {
        RuleFor(x => x.ValorAfecto)
            .GreaterThan(-1);

        RuleFor(x => x.ValorExento)
            .GreaterThan(-1);
    }
}
