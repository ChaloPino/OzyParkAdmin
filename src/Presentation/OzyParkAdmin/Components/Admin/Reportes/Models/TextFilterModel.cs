using OzyParkAdmin.Domain.Reportes.Filters;
using System.Text.RegularExpressions;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo de <see cref="TextFilter"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="TextFilterModel"/>.
/// </remarks>
/// <param name="parent">El padre del filtro.</param>
/// <param name="filter">El <see cref="TextFilter"/> asociado.</param>
/// <param name="row">Fila en que se presentará el filtro.</param>
/// <param name="size">El tamaño del layout.</param>
public sealed class TextFilterModel(FilterViewModel parent, TextFilter filter, int row, SizeLayout size) : FilterModel<TextFilter, string?>(parent, filter, row, size)
{
    /// <summary>
    /// El texto.
    /// </summary>
    public string? Text
    {
        get => Value;
        set => Value = value;
    }

    /// <summary>
    /// El marcador.
    /// </summary>
    public string? Placeholder => Filter.Placeholder;

    internal string? Pattern => Filter.Pattern;

    internal int? MaxLength => Filter.MaxLength;

    /// <inheritdoc/>
    protected override string? ValidateValue(string? value)
    {
        if (Pattern is not null && !Regex.IsMatch(value!, Pattern))
        {
            return $"{Label} no es válido.";
        }

        if (MaxLength is not null && value!.Length > MaxLength)
        {
            return $"{Label} debe tener un largo máximo de {MaxLength}.";
        }

        return null;
    }
}
