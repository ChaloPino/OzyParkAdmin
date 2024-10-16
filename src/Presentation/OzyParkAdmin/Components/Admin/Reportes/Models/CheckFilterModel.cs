using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo de <see cref="CheckFilter"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="CheckFilterModel"/>.
/// </remarks>
/// <param name="parent">El padre del filtro.</param>
/// <param name="filter">El <see cref="CheckFilter"/> asociado.</param>
/// <param name="row">Fila en que se presentará el filtro.</param>
/// <param name="size">El tamaño del layout.</param>
public sealed class CheckFilterModel(FilterViewModel parent, CheckFilter filter, int row, SizeLayout size) : FilterModel<CheckFilter, bool?>(parent, filter, row, size)
{
    /// <summary>
    /// El valor de check.
    /// </summary>
    public bool? Checked 
    {
        get => Value;
        set => Value = value;
    }
}
