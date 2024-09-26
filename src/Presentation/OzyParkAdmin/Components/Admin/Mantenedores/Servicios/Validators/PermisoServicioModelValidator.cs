using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Validators;

/// <summary>
/// El validador de <see cref="PermisoServicioModel"/>
/// </summary>
public sealed class PermisoServicioModelValidator : BaseValidator<PermisoServicioModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="PermisoServicioModelValidator"/>.
    /// </summary>
    public PermisoServicioModelValidator()
    {
        RuleFor(x => x.Tramo)
            .NotNull();

        RuleFor(x => x.CentroCosto)
            .NotNull();
    }
}
