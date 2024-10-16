using MassTransit;
using Org.BouncyCastle.Asn1.Pkcs;
using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo de <see cref="MonthFilter"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="MonthFilterModel"/>.
/// </remarks>
/// <param name="parent">El padre del filtro.</param>
/// <param name="filter">El <see cref="MonthFilter"/> asociado.</param>
/// <param name="row">Fila en que se presentará el filtro.</param>
/// <param name="size">El tamaño del layout.</param>
public sealed class MonthFilterModel(FilterViewModel parent, MonthFilter filter, int row, SizeLayout size) : FilterModel<MonthFilter, DateTime?>(parent, filter, row, size)
{
    /// <summary>
    /// El valor del mes.
    /// </summary>
    public DateTime? Month
    {
        get => Value;
        set => Value = value;
    }

    /// <summary>
    /// El marcador.
    /// </summary>
    public string? Placeholder => Filter.Placeholder;
}
