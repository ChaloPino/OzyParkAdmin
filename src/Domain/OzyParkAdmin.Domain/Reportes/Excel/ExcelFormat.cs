namespace OzyParkAdmin.Domain.Reportes.Excel;

/// <summary>
/// El formato de Excel.
/// </summary>
public sealed class ExcelFormat
{
    private ExcelFormat() { }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ExcelFormat"/>.
    /// </summary>
    /// <param name="id">El identificador del formato Excel.</param>
    /// <param name="format">El formato Excel.</param>
    public ExcelFormat(int id, string format)
    {
        Id = id;
        Format = format;
        IsBuiltIn = false;
    }

    /// <summary>
    /// El id del formato Excel.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// La categoría a la que pertenece el formato Excel.
    /// </summary>
    public ExcelCategoryFormat Category { get; private set; } = default!;

    /// <summary>
    /// El formato Excel.
    /// </summary>
    public string Format { get; private set; } = default!;

    /// <summary>
    /// Si es un formato de paquete.
    /// </summary>
    public bool IsBuiltIn { get; private set; }

    /// <summary>
    /// El formato nativo.
    /// </summary>
    public string? NativeFormat { get; private set; }

    /// <summary>
    /// El despliegue del formato Excel.
    /// </summary>
    public string Display => !string.IsNullOrEmpty(Format) ? $"{Category.Id}({Format})" : Category.Id;
}
