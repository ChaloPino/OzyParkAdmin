namespace OzyParkAdmin.Domain.Entidades;

/// <summary>
/// Entidad ciudad.
/// </summary>
public sealed class Ciudad
{
    /// <summary>
    /// Identificador únido de la ciudad.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Aka de la ciudad.
    /// </summary>
    public string Aka { get; private set; } = string.Empty;

    /// <summary>
    /// Nombre de la ciudad.
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// País al que pertenece la ciudad.
    /// </summary>
    public Pais Pais { get; private set; } = default!;

    /// <summary>
    /// Crea una nueva ciudad.
    /// </summary>
    /// <param name="id">Identificador único.</param>
    /// <param name="aka">Aka.</param>
    /// <param name="nombre">Nombre.</param>
    /// <param name="pais">País.</param>
    /// <returns>Una nueva <see cref="Ciudad"/>.</returns>
    public static Ciudad Crear(
        int id,
        string aka,
        string nombre,
        Pais pais) =>
        new()
        {
            Id = id,
            Aka = aka,
            Nombre = nombre,
            Pais = pais
        };
}
