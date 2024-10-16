namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// El <see cref="ListFilter"/> que puede ser padre de un filtro.
/// </summary>
public sealed class ParentFilter
{
    /// <summary>
    /// El id del filtro padre.
    /// </summary>
    public int ListFilterId { get; private init; }
}
