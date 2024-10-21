namespace OzyParkAdmin.Domain.Franquicias;

/// <summary>
/// La información de una franquicia.
/// </summary>
public sealed record FranquiciaInfo
{
    /// <summary>
    /// El id de la franquicia.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// El nombre de la franquicia.
    /// </summary>
    public required string Nombre { get; init; }

    /// <summary>
    /// Si la franquicia está activa.
    /// </summary>
    public required bool EsActivo { get; init; }
}
