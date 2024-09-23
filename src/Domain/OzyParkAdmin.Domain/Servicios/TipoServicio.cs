namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El tipo de servicio. 
/// </summary>
public enum TipoServicio
{
    /// <summary>
    /// Solo ida.
    /// </summary>
    SoloIda = 0,

    /// <summary>
    /// Ida y vuelta.
    /// </summary>
    IdaVuelta = 1,

    /// <summary>
    /// Ilimitado.
    /// </summary>
    Ilimitado = 9,
}