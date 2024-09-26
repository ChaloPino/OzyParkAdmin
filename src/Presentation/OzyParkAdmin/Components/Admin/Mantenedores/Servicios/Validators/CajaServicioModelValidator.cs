using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Validators;

/// <summary>
/// El validador de <see cref="CajaServicioModel"/>.
/// </summary>
public sealed class CajaServicioModelValidator : BaseValidator<CajaServicioModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="CajaServicioModelValidator"/>.
    /// </summary>
    public CajaServicioModelValidator()
    {
        RuleFor(x => x.Caja)
            .NotNull();
    }
}
