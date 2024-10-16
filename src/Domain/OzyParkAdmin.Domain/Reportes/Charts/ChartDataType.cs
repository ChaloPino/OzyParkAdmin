namespace OzyParkAdmin.Domain.Reportes.Charts;

/// <summary>
/// El tipo de dato del valor y cómo se presentará el valor del dataset del gráfico.
/// </summary>
public enum ChartDataType
{
    /// <summary>
    /// Lista de valores primitivos como números, fechas y textos.
    /// </summary>
    Primitive = 0,

    /// <summary>
    /// Arreglo de valores complejos.
    /// </summary>
    Array = 1,

    /// <summary>
    /// Arreglo de valores complejos pero que se presentarán de forma personalizada, usando los nombres de las columnas del dataTable como nombres de los campos.
    /// </summary>
    CustomArray = 2,

    /// <summary>
    /// Un objeto complejo que tiene las columnas del dataTable como nombres de los campos.
    /// </summary>
    Object = 3,

    /// <summary>
    /// Un objeto complejo cuyos valores de campos serán transformados cada uno en un elemento de un arreglo.
    /// </summary>
    ObjectToArray = 4,
}
