using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Validators;

/// <summary>
/// El validador de <see cref="TramoServicioModelValidator"/>
/// </summary>
public sealed class TramoServicioModelValidator : BaseValidator<TramoServicioModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="TramoServicioModelValidator"/>
    /// </summary>
    public TramoServicioModelValidator()
    {
        RuleFor(x => x.CentroCosto);


        RuleFor(x => x.Tramo)
            .NotNull();

        RuleFor(x => x.Nombre)
            .MaximumLength(150);

        RuleFor(x => x.CantidadPersmisos)
            .GreaterThan(0);
    }
}
