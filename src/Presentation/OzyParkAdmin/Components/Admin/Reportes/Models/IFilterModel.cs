namespace OzyParkAdmin.Components.Admin.Reportes.Models;

/// <summary>
/// El modelo del filtro.
/// </summary>
public interface IFilterModel
{
    /// <summary>
    /// El id del reporte al que pertenece el filtro.
    /// </summary>
    Guid ReportId { get; }
    /// <summary>
    /// El id del filtro.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// El nombre del filtro.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// La etiqueta del filtro.
    /// </summary>
    string Label { get; }

    /// <summary>
    /// Si el filtro es requerido.
    /// </summary>
    bool IsRequired { get; }

    /// <summary>
    /// El filtro si su valor no es válido.
    /// </summary>
    string? InvalidMessage { get; }

    /// <summary>
    /// En qué fila se desplegará el filtro.
    /// </summary>
    int Row { get; }

    /// <summary>
    /// El tamaño del layout del filtro.
    /// </summary>
    SizeLayout SizeLayout { get; }

    /// <summary>
    /// Los filtros padres.
    /// </summary>
    IEnumerable<int> ParentFilters { get; }

    /// <summary>
    /// Consigue los valores del filtro..
    /// </summary>
    /// <returns>El valor del filtro.</returns>
    object? GetValue();

    /// <summary>
    /// Consigue los valores de los padres de este filtro.
    /// </summary>
    /// <returns></returns>
    string?[] GetParentValues();
}
