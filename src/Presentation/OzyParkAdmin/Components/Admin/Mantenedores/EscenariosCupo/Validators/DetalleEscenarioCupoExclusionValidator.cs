using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;

namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Validators;

/// <summary>
/// Validador para la clase <see cref="DetalleEscenarioCupoExclusionModel"/>.
/// Se asegura de que todos los campos requeridos sean válidos antes de proceder con la operación.
/// </summary>
public sealed class DetalleEscenarioCupoExclusionValidator : AbstractValidator<DetalleEscenarioCupoExclusionModel>
{
    /// <summary>
    /// Crea una nueva instancia del validador para <see cref="DetalleEscenarioCupoExclusionModel"/>.
    /// Define las reglas de validación necesarias.
    /// </summary>
    public DetalleEscenarioCupoExclusionValidator()
    {
        RuleFor(x => x.ServicioId)
            .GreaterThan(0).WithMessage("El Servicio es requerido");

        RuleFor(x => x.CanalVentaId)
            .GreaterThan(0).WithMessage("El Canal de Venta es requerido");

        RuleFor(x => x.DiaSemanaId)
            .GreaterThan(0).WithMessage("El Día de la semana es requerido");

        RuleFor(x => x.HoraInicio)
            .NotNull().WithMessage("La hora de inicio es requerida");

        RuleFor(x => x.HoraFin)
            .GreaterThanOrEqualTo(x => x.HoraInicio).When(x => x.HoraFin.HasValue).WithMessage("La hora de fin debe ser mayor o igual a la hora de inicio");
    }
}
