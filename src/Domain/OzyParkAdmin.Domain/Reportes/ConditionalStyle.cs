namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// El estilo condicional.
/// </summary>
public enum ConditionalStyle
{
    /// <summary>
    /// Estilo predeterminado.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Estilo de éxito.
    /// </summary>
    Success = 1,

    /// <summary>
    /// Estilo de advertencia.
    /// </summary>
    Warning = 2,

    /// <summary>
    /// Estilo de error.
    /// </summary>
    Danger = 3,

    /// <summary>
    /// Estilo de información.
    /// </summary>
    Info = 4,

    /// <summary>
    /// Estilo claro.
    /// </summary>
    Light = 5,

    /// <summary>
    /// Estilo oscuro.
    /// </summary>
    Dark = 6,
}
