using Microsoft.AspNetCore.Components;
using MudBlazor;
using OzyParkAdmin.Domain.Shared;
using System.Collections.Immutable;

namespace OzyParkAdmin.Shared;

internal static class SnackbarExtensions
{
    public static void AddFailure(this ISnackbar snackbar, Failure failure, string action)
    {
        string message = failure.Match(
            onUnknown: (unknown) => ResolveUnknown(unknown, action),
            onNotFound: (notFound) => ResolveNotFound(notFound, action),
            onConflict: (conflict) => ResolveConflict(conflict, action),
            onForbid: (forbid) => ResolveForbid(forbid, action),
            onUnauthorized: (unauthorized) => ResolveUnauthorized(unauthorized, action),
            onValidation: (validation) => ResolveValidation(validation, action)
        );

        snackbar.Add(new MarkupString(message), Severity.Error);
    }

    private static string ResolveUnknown(Unknown unknown, string action)
    {
        return $"Hubo errores al {action}: {AggregateErrors(unknown.Errors)}";
    }

    private static string ResolveNotFound(NotFound notFound, string action)
    {
        return notFound.ErrorMessage ?? $"No se encontró el elemento al {action}";
    }

    private static string ResolveConflict(Conflict conflict, string action)
    {
        return conflict.ErrorMessage ?? $"Hubo un conflicto al {action}";
    }

    private static string ResolveForbid(Forbid forbid, string action)
    {
        return forbid.ErrorMessage ?? $"Se prohibió el {action}";
    }

    private static string ResolveUnauthorized(Unauthorized unauthorized, string action)
    {
        return unauthorized.ErrorMessage ?? $"Se denegó el permiso al {action}";
    }

    private static string ResolveValidation(Validation validation, string action)
    {
        return $"Hubo errores de validación al {action}: {AggregateErrors(validation.Errors)}";
    }

    private static string AggregateErrors(ImmutableArray<string> errors)
    {
        return errors.Aggregate("<ul>", AggregateError, message => $"{message}</ul>");
    }

    private static string AggregateErrors(ImmutableArray<ValidationError> errors)
    {
        return errors.Aggregate("<ul>", AggregateError, message => $"{message}</ul>");
    }

    private static string AggregateError(string current, string error)
    {
        return $"{current}<li>{error}</li>";
    }

    private static string AggregateError(string current, ValidationError error)
    {
        return $"{current}<li>{error}</li>";
    }
}
