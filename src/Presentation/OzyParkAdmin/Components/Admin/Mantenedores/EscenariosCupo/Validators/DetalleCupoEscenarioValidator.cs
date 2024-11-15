namespace OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Validators;

using FluentValidation;
using OzyParkAdmin.Components.Admin.Mantenedores.EscenariosCupo.Models;

public class DetalleEscenarioCupoValidator : AbstractValidator<DetalleEscenarioCupoModel>
{
    public DetalleEscenarioCupoValidator()
    {
        RuleFor(x => x.ServicioId).NotEmpty().WithMessage("El servicio es requerido.");
        RuleFor(x => x.TopeDiario).GreaterThanOrEqualTo(0).When(x => x.TopeDiario.HasValue).WithMessage("El tope diario debe ser mayor o igual a 0.");
        RuleFor(x => x.HoraMaximaVenta).NotNull().WithMessage("La hora máxima de venta es requerida.");
        RuleFor(x => x.HoraMaximaRevalidacion).NotNull().WithMessage("La hora máxima de revalidación es requerida.");
    }
}
