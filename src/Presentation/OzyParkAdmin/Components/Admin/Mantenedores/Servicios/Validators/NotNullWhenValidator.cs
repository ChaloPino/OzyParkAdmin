using FluentValidation;
using FluentValidation.Validators;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Validators;

/// <summary>
/// Validador de <see cref="NotNullValidator{T, TProperty}"/> con una condición para saber si se evalúa o no.
/// </summary>
/// <typeparam name="T">El tipo de la entidad a evaluar.</typeparam>
/// <typeparam name="TProperty">El tipo de la propiedad a evaluar.</typeparam>
public class NotNullWhenValidator<T, TProperty> : NotNullValidator<T, TProperty>
{
    private readonly Func<T, bool> _canEvaluateFunc;

    /// <summary>
    /// Crea una nueva instancia de <see cref="NotNullWhenValidator{T, TProperty}"/>.
    /// </summary>
    /// <param name="canEvaluateFunc">La función para evaluar si se tiene que validar o no.</param>
    public NotNullWhenValidator(Func<T, bool> canEvaluateFunc)
    {
        _canEvaluateFunc = canEvaluateFunc;
    }

    /// <inheritdoc/>
    public override string Name => "NotNullWhen";

    /// <inheritdoc/>
    public override bool IsValid(ValidationContext<T> context, TProperty value) =>
        !_canEvaluateFunc(context.InstanceToValidate) || base.IsValid(context, value);
}
