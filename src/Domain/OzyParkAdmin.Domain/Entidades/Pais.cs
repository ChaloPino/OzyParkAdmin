namespace OzyParkAdmin.Domain.Entidades;

/// <summary>
/// Entidad país.
/// </summary>
public sealed class Pais
{
    /// <summary>
    /// Identificador único del país.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Código ISO del país.
    /// </summary>
    public string Aka { get; private set; } = string.Empty;

    /// <summary>
    /// Nombre del país.
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// Gentilicio del país.
    /// </summary>
    public string Gentilicio { get; private set; } = string.Empty;

    /// <summary>
    /// Orden de presentación del país para las listas.
    /// </summary>
    public int? Orden { get; private set; }

    /// <summary>
    /// Crea un nuevo país.
    /// </summary>
    /// <param name="id">Identificador único.</param>
    /// <param name="aka">Aka.</param>
    /// <param name="nombre">Nombre.</param>
    /// <param name="gentilicio">Gentilicio.</param>
    /// <param name="orden">Orden.</param>
    /// <returns>Un nuevo <see cref="Pais"/>.</returns>
    public static Pais Crear(
        int id,
        string aka,
        string nombre,
        string gentilicio,
        int? orden) =>
        new()
        {
            Id = id,
            Aka = aka,
            Nombre = nombre,
            Gentilicio = gentilicio,
            Orden = orden
        };
}
