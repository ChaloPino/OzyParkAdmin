using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Validators;

public sealed class DetalleEscenarioCupoExclusionFechaValidator : AbstractValidator<DetalleEscenarioCupoExclusionFechaModel>
{
    public DetalleEscenarioCupoExclusionFechaValidator()
    {
        RuleFor(x => x.FechaExclusion)
            .NotEmpty().WithMessage("La fecha de exclusión es obligatoria.");

        RuleFor(x => x.ServicioId)
            .GreaterThan(0).WithMessage("Debe seleccionar un servicio válido.");

        RuleFor(x => x.CanalVentaId)
            .GreaterThan(0).WithMessage("Debe seleccionar un canal de venta válido.");

        RuleFor(x => x.HoraInicio)
            .NotEmpty().WithMessage("La hora de inicio es obligatoria.")
            .LessThan(x => x.HoraFin).WithMessage("La hora de inicio debe ser menor que la hora de fin.");

        RuleFor(x => x.HoraFin)
            .NotEmpty().WithMessage("La hora de fin es obligatoria.");
    }
}