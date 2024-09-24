using System.Collections.Immutable;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa un error de tipo not found.
/// </summary>
/// <param name="ErrorMessage">El mensaje de error.</param>
public readonly record struct NotFound(string? ErrorMessage = null);

/// <summary>
/// Representa un error de tipo conflict.
/// </summary>
/// <param name="ErrorMessage">El mensaje de error.</param>
public readonly record struct  Conflict(string? ErrorMessage = null);

/// <summary>
/// Representa un error de tipo forbid.
/// </summary>
/// <param name="ErrorMessage">El mensaje de error.</param>
public readonly record struct Forbid(string? ErrorMessage = null);

/// <summary>
/// Representa un error de unauthorized.
/// </summary>
/// <param name="ErrorMessage">El mensaje de error.</param>
public readonly record struct Unauthorized(string? ErrorMessage = null);

/// <summary>
/// Representa un error de validación.
/// </summary>
/// <param name="Name">El nombre del miembro con error.</param>
/// <param name="ErrorMessage">El mensaje de error.</param>
public readonly record struct ValidationError(string Name, string ErrorMessage);

/// <summary>
/// Representa un conjunto de errores de validación.
/// </summary>
/// <param name="Errors">El listado de errores de validación.</param>
public readonly record struct Validation(ImmutableArray<ValidationError> Errors);

/// <summary>
/// Representa un listado de errores desconocidos.
/// </summary>
/// <param name="Errors">El listado de errores desconocidos.</param>
public readonly record struct Unknown(ImmutableArray<string> Errors);

/// <summary>
/// Representa un éxito.
/// </summary>
public readonly struct Success;