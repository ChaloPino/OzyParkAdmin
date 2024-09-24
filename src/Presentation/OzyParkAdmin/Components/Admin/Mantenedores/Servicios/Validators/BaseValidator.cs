using FluentValidation;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Validators;

/// <summary>
/// Validador base.
/// </summary>
/// <typeparam name="T">El tipo del elemento a validar.</typeparam>
public abstract class BaseValidator<T> : AbstractValidator<T>
{
    /// <summary>
    /// Crea una nueva instancia de <see cref="BaseValidator{T}"/>.
    /// </summary>
    protected BaseValidator()
    {
    }

    /// <summary>
    /// Función que permite adaptar la validación entre Fluent validation y la validación de MudBlazor.
    /// </summary>
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<T>.CreateWithOptions((T)model, x => x.IncludeProperties(propertyName)));

        if (result.IsValid)
        {
            return [];
        }

        return result.Errors.Select(e => e.ErrorMessage);
    };
}
