namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// El operador para los tipos de estilo condicionales.
/// </summary>
public enum OperatorStyleType
{
    /// <summary>
    /// Operador de igual a.
    /// </summary>
    Equal = 1,

    /// <summary>
    /// Operador de no igual a.
    /// </summary>
    NotEqual = 2,

    /// <summary>
    /// Operador de mayor a.
    /// </summary>
    GreaterThan = 3,

    /// <summary>
    /// Operador de menor a.
    /// </summary>
    LessThan = 4,

    /// <summary>
    /// Operador de mayor o igual a.
    /// </summary>
    GreaterThanOrEqual = 5,

    /// <summary>
    /// Operador de menor o igual a.
    /// </summary>
    LessThanOrEqual = 6,

    /// <summary>
    /// Operador de entre.
    /// </summary>
    Between = 7,
}
