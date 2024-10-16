namespace OzyParkAdmin.Domain.Reportes.Excel;

/// <summary>
/// La categoría de formato de Excel.
/// </summary>
public class ExcelCategoryFormat
{
    /// <summary>
    /// El id de la categoría de formato de Excel.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Si puede usar posiciones decimales.
    /// </summary>
    public required bool CanUseDecimalPositions { get; init; }

    /// <summary>
    /// Si puede usar separador de miles.
    /// </summary>
    public required bool CanUseThousandsSeparator { get; init; }

    /// <summary>
    /// Si puede usar símbolos.
    /// </summary>
    public required bool CanUseSymbol { get; init; }

    /// <summary>
    /// El formato de la categoría.
    /// </summary>
    public string? Format { get; init; }
}
