using OzyParkAdmin.Domain.Reportes.Filters;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo de <see cref="HiddenFilter"/>.
/// </summary>
/// <remarks>
/// Crea una nueva instancia de <see cref="HiddenFilterModel"/>.
/// </remarks>
/// <param name="parent">El padre del filtro.</param>
/// <param name="filter">El <see cref="HiddenFilter"/> asociado.</param>
/// <param name="row">Fila en que se presentará el filtro.</param>
/// <param name="size">El tamaño del layout.</param>
public sealed class HiddenFilterModel(FilterViewModel parent, HiddenFilter filter, int row, SizeLayout size) : FilterModel<HiddenFilter, string?>(parent, filter, row, size)
{
}
