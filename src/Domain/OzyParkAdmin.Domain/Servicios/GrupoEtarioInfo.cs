namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Información del grupo etario.
/// </summary>
public sealed record GrupoEtarioInfo
{
    /// <summary>
    /// Id del grupo etario.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// El aka del grupo etario.
    /// </summary>
    public string Aka { get; init; } = default!;

    /// <summary>
    /// La descripción del grupo etario.
    /// </summary>
    public string Descripcion { get; init; } = default!;
}