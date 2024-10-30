using System.Collections.Immutable;
using System.Text;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa un error de tipo not found.
/// </summary>
/// <param name="ErrorMessage">El mensaje de error.</param>
public readonly record struct NotFound(string? ErrorMessage = null);

/// <summary>
/// Representa un error de tipo conflict, por ejemplo valores duplicados.
/// </summary>
/// <param name="ErrorMessage">El mensaje de error.</param>
public readonly record struct Conflict(string? ErrorMessage = null);

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
public readonly record struct Validation(ImmutableArray<ValidationError> Errors)
{
    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Validation");
        stringBuilder.Append(" { ");

        if (PrintMembers(stringBuilder))
        {
            stringBuilder.Append(' ');
        }

        stringBuilder.Append('}');
        return stringBuilder.ToString();
    }

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Errors = [ ");

        for (int i = 0; i < Errors.Length; i++)
        {
            if (i > 0)
            {
                builder.Append(", ");
            }

            builder.Append(Errors[i]);
        }

        builder.Append(" ]");

        return true;
    }
}

/// <summary>
/// Representa un listado de errores desconocidos.
/// </summary>
/// <param name="Ticket">El ticket asociado para revisar el log.</param>
/// <param name="Errors">El listado de errores desconocidos.</param>
public readonly record struct Unknown(string Ticket, ImmutableArray<string> Errors)
{
    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Unknown");
        stringBuilder.Append(" { ");
        if (PrintMembers(stringBuilder))
        {
            stringBuilder.Append(' ');
        }
        stringBuilder.Append('}');
        return stringBuilder.ToString();
    }

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Ticket = ");
        builder.Append(Ticket);
        builder.Append(", Errors = [ ");

        for (int i = 0; i < Errors.Length; i++)
        {
            if (i > 0)
            {
                builder.Append(", ");
            }

            builder.Append(Errors[i]);
        }

        builder.Append(" ]");

        return true;
    }
}

/// <summary>
/// Representa un éxito.
/// </summary>
public readonly record struct Success;

/// <summary>
/// Representa nada en <see cref="SomeOrNone{T}"/>.
/// </summary>
public readonly record struct None;