using System.Data;

namespace OzyParkAdmin.Domain.Reportes.Excel;

/// <summary>
/// Contiene la configuración para pintar un elemento en Excel.
/// </summary>
public interface IExcelColumnFormattable : IConditionable
{
    /// <summary>
    /// El id del elemento.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// El nombre del elemento.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// La cabecera del elemento.
    /// </summary>
    string? Header { get; }

    /// <summary>
    /// El tipo de dato del elemento.
    /// </summary>
    DbType Type { get; }

    /// <summary>
    /// El orden de despliegue del elemento.
    /// </summary>
    int Order { get; }

    /// <summary>
    /// El tipo de agregación del elemento.
    /// </summary>
    AggregationType? AggregationType { get; }

    /// <summary>
    /// El formato de excel actual del elemento.
    /// </summary>
    ExcelFormat? CurrentExcelFormat { get; }

    /// <summary>
    /// Si el elemento es de tipo <see cref="string"/> o <see cref="Guid"/>.
    /// </summary>
    /// <returns><c>true</c> si es de tipo <see cref="string"/> o <see cref="Guid"/>; en caso contrario <c>false</c>.</returns>
    bool IsStringAndGuidType();
}
