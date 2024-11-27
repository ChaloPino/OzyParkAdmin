using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.TarifasServicio.Validators;

/// <summary>
/// El validador de <see cref="TarifaServicioCreateModel"/>.
/// </summary>
public class TarifaServicioCreateModelValidator : BaseValidator<TarifaServicioCreateModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="TarifaServicioCreateModelValidator"/>.
    /// </summary>
    public TarifaServicioCreateModelValidator()
    {
        RuleFor(x => x.Moneda)
            .NotNull();

        RuleFor(x => x.InicioVigencia)
            .NotNull();

        RuleFor(x => x.HoraVigencia)
            .NotNull();

        RuleFor(x => x.Servicio)
            .NotNull();

        RuleFor(x => x.CanalesVenta)
            .NotEmpty();

        RuleFor(x => x.TiposDia)
            .NotEmpty();

        RuleFor(x => x.TiposHorario)
            .NotEmpty();

        RuleFor(x => x.TiposSegmentacion)
            .NotEmpty();

        RuleFor(x => x.ValorAfecto)
            .GreaterThan(-1);

        RuleFor(x => x.ValorExento)
            .GreaterThan(-1);
    }
}
