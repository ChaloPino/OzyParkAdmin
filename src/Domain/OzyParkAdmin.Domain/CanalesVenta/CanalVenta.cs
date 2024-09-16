namespace OzyParkAdmin.Domain.CanalesVenta;

/// <summary>
/// Entidad canal de venta.
/// </summary>
public sealed class CanalVenta
{
    /// <summary>
    /// Identificador único del canal de venta.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Aka del canal de venta.
    /// </summary>
    public string Aka { get; private set; } = string.Empty;

    /// <summary>
    /// Nombre del canal de venta.
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// Nombre visual del canal de venta.
    /// </summary>
    public string Texto { get; private set; } = string.Empty;

    /// <summary>
    /// Si el canal de venta está activo o no.
    /// </summary>
    public bool EsActivo { get; private set; }

    /// <summary>
    /// Crea un nuevo canal de venta.
    /// </summary>
    /// <param name="id">Identificador único.</param>
    /// <param name="aka">Aka.</param>
    /// <param name="nombre">Nombre.</param>
    /// <param name="texto">Texto.</param>
    /// <returns>Un nuevo <see cref="CanalVenta"/>.</returns>
    public static CanalVenta Crear(
        int id,
        string aka,
        string nombre,
        string texto) =>
        new()
        {
            Id = id,
            Aka = aka,
            Nombre = nombre,
            Texto = texto,
            EsActivo = true
        };
}