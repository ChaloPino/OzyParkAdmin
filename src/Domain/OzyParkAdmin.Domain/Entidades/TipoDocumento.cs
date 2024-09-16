namespace OzyParkAdmin.Domain.Entidades;

/// <summary>
/// Tipo de documento.
/// </summary>
public sealed class TipoDocumento
{
    /// <summary>
    /// Identificador único del tipo de documento.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Nombre del tipo de documento.
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// Crea un nuevo tipo de documento.
    /// </summary>
    /// <param name="id">Identificador único.</param>
    /// <param name="nombre">Nombre.</param>
    /// <returns>Un nuevo <see cref="TipoDocumento"/>.</returns>
    public static TipoDocumento Crear(
        int id,
        string nombre) =>
        new()
        {
            Id = id,
            Nombre = nombre
        };
}