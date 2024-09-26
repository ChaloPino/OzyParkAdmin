using FluentValidation;
using FluentValidation.Validators;
using MassTransit;
using MassTransit.Mediator;
using OzyParkAdmin.Application.Servicios.Validate;
using OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Validators;

/// <summary>
/// Validador de <see cref="ServicioViewModel"/>.
/// </summary>
public class ServicioViewModelValidator : BaseValidator<ServicioViewModel>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Crea una nueva instancia de <see cref="ServicioViewModelValidator"/>.
    /// </summary>
    /// <param name="mediator">El se <see cref="IMediator"/>.</param>
    public ServicioViewModelValidator(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        _mediator = mediator;

        RuleFor(x => x.CentroCosto)
            .NotNull();

        RuleFor(x => x.FranquiciaId)
            .NotEmpty();

        RuleFor(x => x.Aka)
            .NotEmpty()
            .MaximumLength(20)
            .CustomAsync(ValidateAkaAsync);

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.TipoServicio)
            .NotNull();

        RuleFor(x => x.TipoVigencia)
            .NotNull();

        RuleFor(x => x.NumeroValidez)
            .InclusiveBetween((short)0, short.MaxValue, ComparableComparer<short>.Instance);

        RuleFor(x => x.NumeroVigencia)
            .InclusiveBetween((short)0, short.MaxValue, ComparableComparer<short>.Instance);

        RuleFor(x => x.NumeroPaxMinimo)
            .InclusiveBetween((short)1, short.MaxValue, ComparableComparer<short>.Instance)
            .LessThanOrEqualTo(x => x.NumeroPaxMaximo);

        RuleFor(x => x.NumeroPaxMaximo)
            .InclusiveBetween((short)1, short.MaxValue, ComparableComparer<short>.Instance)
            .GreaterThanOrEqualTo(x => x.NumeroPaxMinimo);

        RuleFor(x => x.TipoControl)
            .NotNull();

        RuleFor(x => x.Orden)
            .InclusiveBetween(1, int.MaxValue, ComparableComparer<int>.Instance);

        RuleFor(x => x.HolguraInicio)
            .LessThanOrEqualTo(x => x.HolguraFin);

        RuleFor(x => x.HolguraFin)
            .GreaterThanOrEqualTo(x => x.HolguraInicio);

        RuleFor(x => x.IdaVuelta)
            .NotNullWhen(x => x.EsParaBuses);

        RuleFor(x => x.HolguraEntrada)
            .InclusiveBetween((byte)0, byte.MaxValue, ComparableComparer<byte>.Instance);

        RuleForEach(x => x.Cajas)
            .SetValidator(new CajaServicioModelValidator());

        RuleForEach(x => x.CentrosCosto)
            .SetValidator(new CentroCostoServicioModelValidator());

        RuleForEach(x => x.Permisos)
            .SetValidator(new PermisoServicioModelValidator());

        RuleForEach(x => x.Tramos)
            .SetValidator(new TramoServicioModelValidator());
    }

    private async Task ValidateAkaAsync(string aka, ValidationContext<ServicioViewModel> context, CancellationToken cancellationToken)
    {
        var validateAka = new ValidateServicioAka(context.InstanceToValidate.Id, context.InstanceToValidate.FranquiciaId, aka);

        SuccessOrFailure result = await _mediator.SendRequest(validateAka, cancellationToken);

        result.Switch(
            onSuccess: _ => { },
            onFailure: failure => context.AddFailure(failure));
    }
}
