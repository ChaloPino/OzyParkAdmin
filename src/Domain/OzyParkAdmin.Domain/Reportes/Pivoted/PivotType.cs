namespace OzyParkAdmin.Domain.Reportes.Pivoted;

/// <summary>
/// El tipo de del miembro pivote.
/// </summary>
public enum PivotType
{
    /// <summary>
    /// Si es un miembro para una fila.
    /// </summary>
    Row = 1,

    /// <summary>
    /// Si es un miembro para una columna.
    /// </summary>
    Column = 2,

    /// <summary>
    /// Si es un miembro para los valores.
    /// </summary>
    Value = 3,
}
