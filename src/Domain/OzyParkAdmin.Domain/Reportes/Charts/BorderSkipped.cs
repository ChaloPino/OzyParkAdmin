namespace OzyParkAdmin.Domain.Reportes.Charts;

/// <summary>
/// El tipo de borde saltado.
/// </summary>
public enum BorderSkipped
{
    /// <summary>
    /// Al comienzo.
    /// </summary>
    Start = 0,

    /// <summary>
    /// Al final.
    /// </summary>
    End = 1,

    /// <summary>
    /// En el medio.
    /// </summary>
    Middle = 2,

    /// <summary>
    /// Abajo.
    /// </summary>
    Bottom = 3,

    /// <summary>
    /// A la izquierda.
    /// </summary>
    Left = 4,

    /// <summary>
    /// Arriba.
    /// </summary>
    Top = 5,

    /// <summary>
    /// A la derecha.
    /// </summary>
    Right = 6,

    /// <summary>
    /// Sin saltado.
    /// </summary>
    False = 7,
}
