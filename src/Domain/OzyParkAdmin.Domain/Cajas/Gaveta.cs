namespace OzyParkAdmin.Domain.Cajas;

/// <summary>
/// Gaveta de una caja.
/// </summary>
public sealed class Gaveta
{
    /// <summary>
    /// Identificador único de la gaveta.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Caja de la gaveta.
    /// </summary>
    public Caja Caja { get; private set; } = default!;

    /// <summary>
    /// Aka de la gaveta.
    /// </summary>
    public string Aka { get; private set; } = string.Empty;

    /// <summary>
    /// Descripción de la gaveta.
    /// </summary>
    public string Descripcion { get; private set; } = string.Empty;

    /// <summary>
    /// Si la gaveta está activa o no.
    /// </summary>
    public bool EsActivo { get; private set; }

    internal static Gaveta Crear(
        int id,
        Caja caja,
        string aka,
        string descripcion) =>
        new()
        {
            Id = id,
            Caja = caja,
            Aka = aka,
            Descripcion = descripcion,
            EsActivo = true,
        };
}
