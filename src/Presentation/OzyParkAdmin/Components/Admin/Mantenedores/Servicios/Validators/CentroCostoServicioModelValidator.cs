using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Validators;

/// <summary>
/// El validador de <see cref="CentroCostoServicioModel"/>
/// </summary>
public sealed class CentroCostoServicioModelValidator : BaseValidator<CentroCostoServicioModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="CentroCostoServicioModelValidator"/>.
    /// </summary>
    public CentroCostoServicioModelValidator()
    {
        RuleFor(x => x.CentroCosto)
            .NotNull();

        RuleFor(x => x.Nombre)
            .MaximumLength(150);
    }
}
