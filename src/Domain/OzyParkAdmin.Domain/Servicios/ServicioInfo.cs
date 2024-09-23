namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Información del servicio.
/// </summary>
public class ServicioInfo
{
    /// <summary>
    /// El id del servicio.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// El aka del servicio.
    /// </summary>
    public string Aka { get; init; } = string.Empty;

    /// <summary>
    /// El nombre del servicio.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;
}
