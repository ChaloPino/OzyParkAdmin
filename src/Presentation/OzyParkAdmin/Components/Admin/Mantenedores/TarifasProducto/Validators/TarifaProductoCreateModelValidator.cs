using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.TarifasProductos.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasProducto.Validators;

/// <summary>
/// El validador de <see cref="TarifaProductoCreateModel"/>.
/// </summary>
public class TarifaProductoCreateModelValidator : BaseValidator<TarifaProductoCreateModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="TarifaProductoCreateModelValidator"/>.
    /// </summary>
    public TarifaProductoCreateModelValidator()
    {
        RuleFor(x => x.Moneda)
            .NotNull();

        RuleFor(x => x.InicioVigencia)
            .NotNull();

        RuleFor(x => x.HoraVigencia)
            .NotNull();

        RuleFor(x => x.Producto)
            .NotNull();

        RuleFor(x => x.CanalesVenta)
            .NotEmpty();

        RuleFor(x => x.TiposDia)
            .NotEmpty();

        RuleFor(x => x.TiposHorario)
            .NotEmpty();

        RuleFor(x => x.ValorAfecto)
            .GreaterThan(-1);

        RuleFor(x => x.ValorExento)
            .GreaterThan(-1);
    }
}
