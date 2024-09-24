using FluentValidation;
using FluentValidation.Results;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Shared;

internal static class ValidationContextExtensions
{
    public static void AddFailure<T>(this ValidationContext<T> context, Failure failure) =>
        failure.Switch(
            onUnknown: unknown => context.AddFailure(unknown),
            onNotFound: notFound => context.AddFailure(notFound),
            onConflict: conflict => context.AddFailure(conflict),
            onForbid: forbid => context.AddFailure(forbid),
            onUnauthorized: unauthorized => context.AddFailure(unauthorized),
            onValidation: validation => AddFailure(context, validation));

    private static void AddFailure<T>(this ValidationContext<T> context, Unknown unknown) =>
        unknown.Errors.ToList().ForEach(x => context.AddFailure(new ValidationFailure(string.Empty, x)));

    private static void AddFailure<T>(this ValidationContext<T> context, NotFound notFound) =>
        context.AddFailure(new ValidationFailure(string.Empty, notFound.ErrorMessage ?? "No encontrado"));

    private static void AddFailure<T>(this ValidationContext<T> context, Conflict conflict) =>
        context.AddFailure(new ValidationFailure(string.Empty, conflict.ErrorMessage ?? "Conflicto"));

    private static void AddFailure<T>(this ValidationContext<T> context, Forbid forbid) =>
        context.AddFailure(new ValidationFailure(string.Empty, forbid.ErrorMessage ?? "Sin acceso"));

    private static void AddFailure<T>(this ValidationContext<T> context, Unauthorized unauthorized) =>
        context.AddFailure(new ValidationFailure(string.Empty, unauthorized.ErrorMessage ?? "No autorizado"));

    private static void AddFailure<T>(this ValidationContext<T> context, Validation validation) =>
        validation.Errors.ToList().ForEach(x => context.AddFailure(x));

    private static void AddFailure<T>(this ValidationContext<T> context, ValidationError error) =>
        context.AddFailure(new ValidationFailure(error.Name, error.ErrorMessage));

}
