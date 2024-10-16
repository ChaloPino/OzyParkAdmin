namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// Los tipos de agregaciones que puede tener un valor.
/// </summary>
public enum AggregationType
{
    /// <summary>
    /// Suma de valores.
    /// </summary>
    Sum = 1,

    /// <summary>
    /// Conteo de valores.
    /// </summary>
    Count = 2,

    /// <summary>
    /// Promedio de valores.
    /// </summary>
    Average = 3,

    /// <summary>
    /// Valor mínimo.
    /// </summary>
    Min = 4,

    /// <summary>
    /// Valor máximo.
    /// </summary>
    Max = 5,
}
