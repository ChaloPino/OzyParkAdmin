﻿using FluentValidation;
using Nextended.Core.Extensions;
using OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Models;
using OzyParkAdmin.Shared;

namespace OzyParkAdmin.Components.Admin.Mantenedores.CuposFecha.Validators;

/// <summary>
/// El validador de <see cref="CuposFechaDeleteModel"/>.
/// </summary>
public sealed class CuposFechaDeleteModelValidator : BaseValidator<CuposFechaDeleteModel>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="CuposFechaDeleteModelValidator"/>.
    /// </summary>
    public CuposFechaDeleteModelValidator()
    {
        RuleFor(x => x.RangoFechas.Start)
            .NotEmpty()
            .LessThanOrEqualTo(x => x.RangoFechas.End);

        RuleFor(x => x.RangoFechas.End)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.RangoFechas.Start);
        RuleFor(x => x.EscenarioCupo)
            .NotNull();

        RuleFor(x => x.CanalesVenta)
            .NotEmpty();

        RuleFor(x => x.DiasSemana)
            .NotEmpty();

        RuleFor(x => x.HoraInicio)
            .NotNull()
            .LessThanOrEqualTo(x => x.HoraTermino);

        RuleFor(x => x.HoraTermino)
            .NotNull()
            .GreaterThanOrEqualTo(x => x.HoraInicio);
    }
}
